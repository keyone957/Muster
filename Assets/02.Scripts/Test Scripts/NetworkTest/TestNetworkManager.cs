using Fusion;
using System.Collections.Generic;
using Fusion.Sockets;
using UnityEngine;
using System;

public class TestNetworkManager : MonoBehaviour, INetworkRunnerCallbacks
{
    public NetworkRunner _runner { get; private set; }
    [SerializeField] NetworkObject _networkPrefab;

    [Networked] public Dictionary<PlayerRef, NetworkObject> SpawnedUsers { get; private set; } = new Dictionary<PlayerRef, NetworkObject>();

    private void Awake()
    {
        _runner = GetComponent<NetworkRunner>();
        if (_runner == null)
        {
            _runner = gameObject.AddComponent<NetworkRunner>();
        }
        JoinSession();
    }
    public async void JoinSession()
    {
        await _runner.StartGame(new StartGameArgs()
        {
            GameMode = GameMode.AutoHostOrClient,
            SessionName = "1",
        });
    }
    public void OnPlayerJoined(NetworkRunner runner, PlayerRef player)
    {
        Debug.Log("어 형이야");
        NetworkObject networkPlayerObject = null;
        if (runner.IsServer)
        {
            networkPlayerObject =
                    runner.Spawn
                    (
                        _networkPrefab,
                        position: transform.position,
                        rotation: transform.rotation,
                        inputAuthority: player,
                        (runner, obj) => { }
                    );
            SpawnedUsers.Add(player, networkPlayerObject);
        }
    }

    public void OnConnectedToServer(NetworkRunner runner)
    {
    }

    public void OnConnectFailed(NetworkRunner runner, NetAddress remoteAddress, NetConnectFailedReason reason)
    {
    }

    public void OnConnectRequest(NetworkRunner runner, NetworkRunnerCallbackArgs.ConnectRequest request, byte[] token)
    {
    }

    public void OnCustomAuthenticationResponse(NetworkRunner runner, Dictionary<string, object> data)
    {
    }

    public void OnDisconnectedFromServer(NetworkRunner runner)
    {
    }

    public void OnHostMigration(NetworkRunner runner, HostMigrationToken hostMigrationToken)
    {
    }

    public void OnInput(NetworkRunner runner, NetworkInput input)
    {
    }

    public void OnInputMissing(NetworkRunner runner, PlayerRef player, NetworkInput input)
    {
    }

    public void OnPlayerLeft(NetworkRunner runner, PlayerRef player)
    {
    }

    public void OnReliableDataReceived(NetworkRunner runner, PlayerRef player, ArraySegment<byte> data)
    {
    }

    public void OnSceneLoadDone(NetworkRunner runner)
    {
    }

    public void OnSceneLoadStart(NetworkRunner runner)
    {
    }

    public void OnSessionListUpdated(NetworkRunner runner, List<SessionInfo> sessionList)
    {
    }

    public void OnShutdown(NetworkRunner runner, ShutdownReason shutdownReason)
    {
    }

    public void OnUserSimulationMessage(NetworkRunner runner, SimulationMessagePtr message)
    {
    }
}
