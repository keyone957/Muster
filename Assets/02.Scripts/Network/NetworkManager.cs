using System;
using System.Linq;
using System.Collections.Generic;
using UnityEngine;
using Fusion;
using Fusion.Sockets;
using Cysharp.Threading.Tasks;
using Photon.Voice.Unity;

public struct ENetworkInfo
{
    public string playerName;
}

// 네트워크 매니저
// 최초 작성자 : 김기홍
// 수정자 : 
// 최종 수정일 : 2024-02-12
public class NetworkManager : MonoBehaviour, INetworkRunnerCallbacks
{
    // Singleton
    public static NetworkManager _instance = null;

    // Network
    public NetworkRunner _runner { get; private set; }
    public Recorder _recorder;
    // Player Prefab
    [SerializeField] NetworkObject _audiencePrefab;
    [SerializeField] NetworkObject _idolPrefab;

    // Networked Managers (Server만 접근)
    [SerializeField] NetworkObject _networkDataManagerPrefab;

    // Variables
    public const int MAX_AUDIENCE = 10;
    public PlayerRef CurPlayerRef { get; private set; }
    public int CurPlayerID { get; private set; }
    public PlayerRef IdolRef { get; private set; }


    [Header("for Debug")]
    [SerializeField] AudienceHardwareRig _audienceRig;
    [SerializeField] IdolManager _idolRig;


    [Header("Session List")]
    private List<SessionInfo> sessions = new List<SessionInfo>();
    public List<SessionInfo> Sessions { get { return sessions; } }


    private void Awake()
    {
        if (_instance == null)
        {
            _instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }

        _runner = GetComponent<NetworkRunner>();
        if (_runner == null)
        {
            _runner = gameObject.AddComponent<NetworkRunner>();
        }
    }

    // Lobby 입장
    public async UniTask ConnectToLobby(string _playername)
    {
        if (_runner == null)
        {
            _runner = gameObject.AddComponent<NetworkRunner>();
        }

        Debug.Log("Join Lobby : Loading");
        await _runner.JoinSessionLobby(SessionLobby.ClientServer);
        Debug.Log("Join Lobby : Success To Connect");
    }

    // Session 생성 (Host)
    public async UniTask CreateSession(string _roomName)
    {
        if (_runner == null)
        {
            _runner = gameObject.AddComponent<NetworkRunner>();
        }
        if (_idolRig == null)
        {
            _idolRig = FindObjectOfType<IdolManager>();
        }
        if (_audienceRig == null)
        {
            _audienceRig = FindObjectOfType<AudienceManager>().GetComponent<AudienceHardwareRig>();
        }

        _idolRig.gameObject.SetActive(true);
        _audienceRig.gameObject.SetActive(false);
        //_recorder.TransmitEnabled = true;


        Debug.Log("Create Session(Room) : Loading");
        await _runner.StartGame(new StartGameArgs()
        {
            GameMode = GameMode.Host,
            SessionName = _roomName,
            PlayerCount = 20,
        });
        Debug.Log("Create Session(Room) : Success To Create");

        _runner.AddSimulationBehaviour(GetComponent<NetworkDataManager>());
    }

    // Session 입장 (Client)
    public async UniTask ConnectToSession(string _roomName)
    {
        if (_runner == null)
        {
            _runner = gameObject.AddComponent<NetworkRunner>();
        }
        if (_idolRig == null)
        {
            _idolRig = FindObjectOfType<IdolManager>();
        }
        if (_audienceRig == null)
        {
            _audienceRig = FindObjectOfType<AudienceManager>().GetComponent<AudienceHardwareRig>();
        }

        _idolRig.gameObject.SetActive(false);
        _audienceRig.gameObject.SetActive(true);

        Debug.Log("Connect Session(Room) : Loading");
        await _runner.StartGame(new StartGameArgs()
        {
            GameMode = GameMode.Client,
            SessionName = _roomName,
        });
        Debug.Log("Connect Session(Room) : Success To Connect");
    }
    public async UniTask DisconnectToSession()
    {
        Destroy(_runner);

        _runner = gameObject.AddComponent<NetworkRunner>();
        await _runner.JoinSessionLobby(SessionLobby.ClientServer);
    }

    #region INetworkRunnerCallbacks - Player
    public void OnPlayerJoined(NetworkRunner runner, PlayerRef player)
    {
        Debug.Log($"OnPlayerJoined. PlayerId: {player.PlayerId}");
        CurPlayerRef = player;
        CurPlayerID = player.PlayerId;
        NetworkObject networkPlayerObject = null;

        // Server Setting
        if (runner.IsServer)
        {
            if (NetworkDataManager.SpawnedUsers == null || NetworkDataManager.SpawnedUsers.Count == 0)
            {
                networkPlayerObject =
                    runner.Spawn
                    (
                        _idolPrefab,
                        position: new Vector3(0,100,0),
                        rotation: transform.rotation,
                        inputAuthority: player,
                        (runner, obj) => { }
                    );

                // Idol Network RIg 초기화
                // networkPlayerObject.transform.position = new Vector3(0,100,0);
                NetworkDataManager.IdolRef = player;
            }
            else
            {
                networkPlayerObject =
                    runner.Spawn
                    (
                        _audiencePrefab,
                        position: new Vector3(0, 100, 0),
                        rotation: transform.rotation,
                        inputAuthority: player,
                        (runner, obj) => { }
                    );
            }
            if (NetworkDataManager.SpawnedUsers == null)
            {
                NetworkDataManager.SpawnedUsers = new Dictionary<PlayerRef, NetworkObject>();
            }
            NetworkDataManager.SpawnedUsers.Add(player, networkPlayerObject);

            var keys = NetworkDataManager.SpawnedUsers.Keys.ToArray();
            var values = NetworkDataManager.SpawnedUsers.Values.ToArray();
            NetworkDataManager.Rpc_SyncSpawnedUsers(runner, keys, values);
            NetworkDataManager.Rpc_SyncIdolRef(runner, NetworkDataManager.IdolRef);
        }

        // Local Setting
        if (runner.IsPlayer)
        {
            FindObjectOfType<PlayerManager>().SetEntryOrder(NetworkDataManager.SpawnedUsers.Count - 1);
        }
    }

    public void OnPlayerLeft(NetworkRunner runner, PlayerRef player)
    {
        Debug.Log($"OnPlayerLeft. PlayerId: {player.PlayerId}.");
        if (runner.IsServer)
        {
            // Find and remove the players avatar (only the host would have stored the spawned game object)

            if (NetworkDataManager.SpawnedUsers.TryGetValue(player, out NetworkObject networkObject))
            {
                if (networkObject != null)
                    runner.Despawn(networkObject);
                NetworkDataManager.SpawnedUsers.Remove(player);

                var keys = NetworkDataManager.SpawnedUsers.Keys.ToArray();
                var values = NetworkDataManager.SpawnedUsers.Values.ToArray();
                NetworkDataManager.Rpc_SyncSpawnedUsers(runner, keys, values);
            }
        }
    }
    #endregion

    #region INetworkRunnerCallbacks - Connect
    public void OnSessionListUpdated(NetworkRunner runner, List<SessionInfo> sessionList)
    {
        sessions.Clear();
        sessions = sessionList;
    }
    #endregion

    #region INetworkRunnerCallbacks (debug log only)
    public void OnConnectRequest(NetworkRunner runner, NetworkRunnerCallbackArgs.ConnectRequest request, byte[] token)
    {
        Debug.Log("OnConnectRequest");
    }
    public void OnConnectedToServer(NetworkRunner runner)
    {
        Debug.Log("OnConnectedToServer");

    }
    public void OnShutdown(NetworkRunner runner, ShutdownReason shutdownReason)
    {
        Debug.Log("Shutdown: " + shutdownReason);
    }
    public void OnDisconnectedFromServer(NetworkRunner runner)
    {
        Debug.Log("OnDisconnectedFromServer");
    }
    public void OnConnectFailed(NetworkRunner runner, NetAddress remoteAddress, NetConnectFailedReason reason)
    {
        Debug.Log("OnConnectFailed: " + reason);
    }

    public void OnInput(NetworkRunner runner, NetworkInput input)
    {
        // Debug.Log("OnInput");
    }
    public void OnInputMissing(NetworkRunner runner, PlayerRef player, NetworkInput input)
    {
        Debug.Log("OnInputMissing");
    }
    #endregion

    #region Not Use - INetworkRunnerCallbacks
    public void OnCustomAuthenticationResponse(NetworkRunner runner, Dictionary<string, object> data)
    {
        throw new NotImplementedException();
    }

    public void OnHostMigration(NetworkRunner runner, HostMigrationToken hostMigrationToken)
    {
        throw new NotImplementedException();
    }


    public void OnReliableDataReceived(NetworkRunner runner, PlayerRef player, ArraySegment<byte> data)
    {
        throw new NotImplementedException();
    }

    public void OnSceneLoadDone(NetworkRunner runner)
    {
        throw new NotImplementedException();
    }

    public void OnSceneLoadStart(NetworkRunner runner)
    {
        throw new NotImplementedException();
    }

    public void OnUserSimulationMessage(NetworkRunner runner, SimulationMessagePtr message)
    {
        throw new NotImplementedException();
    }
    #endregion
}
