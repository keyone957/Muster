using Fusion;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

// Networked Property들을 저장
// 최초 작성자 : 김기홍
// 수정자 : -
// 최종 수정일 : 2024-02-12
public class NetworkDataManager : SimulationBehaviour
{
    ////////////////////////////////////////////
    // User Info
    #region User Info
    public static Dictionary<PlayerRef, NetworkObject> SpawnedUsers { get; set; } = default; // 현재 오류 있음! NetworkObject가 비어있음
    public static PlayerRef IdolRef { get; set; } = default;

    [Rpc]
    public static void Rpc_SyncSpawnedUsers(NetworkRunner runner, PlayerRef[] keys, NetworkObject[] values, RpcInfo info = default)
    {
        if (runner.IsServer)
        {
            Debug.Log("IsLocalCalled : Rpc_SyncSpawnedUsers");
        }
        else
        {
            Debug.Log("IsCalled : Rpc_SyncSpawnedUsers, Size = " + keys.Length);
            if (SpawnedUsers == null)
            {
                SpawnedUsers = new Dictionary<PlayerRef, NetworkObject>();
            }
            for (int i = 0; i < keys.Length; i++)
            {
                if (SpawnedUsers.ContainsKey(keys[i]) == false)
                {
                    // 빈 값이다..
                    SpawnedUsers.Add(keys[i], values[i]);
                }

                // 몰라 강제로 찾아
                /*
                NetworkObject networkObj = null;
                Debug.Log("Length : " + FindObjectsOfType<NetworkGamification_Crazy>().Length);
                foreach (var p in FindObjectsOfType<NetworkGamification_Crazy>())
                {
                    Debug.Log("adsfasdf : " +  p);  
                    var pno = p.GetComponent<NetworkObject>();
                    if (pno.InputAuthority.PlayerId == keys[i].PlayerId)
                    {
                        networkObj = pno;
                        break;
                    }
                }
                Debug.Log($"Founded Network Object: {networkObj}");
                SpawnedUsers.Add(keys[i], networkObj);
                */
            }
        }
    }
    [Rpc]
    public static void Rpc_SyncIdolRef(NetworkRunner runner, PlayerRef i, RpcInfo info = default)
    {
        if (runner.IsServer)
            Debug.Log("IsLocalCalled : Rpc_SyncIdolRef");
        else
        {
            Debug.Log("IsCalled : Rpc_SyncIdolRef, Ref = " + i);
            IdolRef = i;
        }
    }
    public static NetworkObject GetNetworkObject(PlayerRef key)
    {
        // 강제로 찾기
        foreach (var p in FindObjectsOfType<NetworkObject>())
        {
            if (p.InputAuthority.PlayerId == key.PlayerId)
            {
                return p;
            }
        }
        return null;
    }
    #endregion

    ////////////////////////////////////////////
    // Gamification_Crazy Data
    // 관련 데이터는 Server만 접근하도록 해야함
    #region GAMIFICATION_CRAZY
    // 개인 점수 저장
    public static Dictionary<PlayerRef, int> Scores { get; private set; } = new Dictionary<PlayerRef, int>();
    // 총 점수 저장
    public static int[] TeamScore { get; private set; } = new int[3] { 0, 0, 0 };

    [Rpc(RpcSources.InputAuthority,RpcTargets.StateAuthority)]
    public static void Rpc_AddScore(NetworkRunner runner, int playerID, int teamID, int score, RpcInfo info = default)
    {
        Debug.Log($"Add Score : {score}");
        int curScore;
        if(Scores.TryGetValue(playerID, out curScore))
        {
            Scores[playerID] += score;
            Rpc_SyncPersonalScore(runner, Scores.Keys.ToArray(), Scores.Values.ToArray());
        }
        else
        {
            Scores.Add(playerID,score);
            Rpc_SyncPersonalScore(runner, Scores.Keys.ToArray(), Scores.Values.ToArray());
        }
        TeamScore[teamID] += score;
        Rpc_SyncTeamScore(runner, TeamScore);
    }
    [Rpc]
    public static void Rpc_SyncPersonalScore(NetworkRunner runner, PlayerRef[] keys, int[] values, RpcInfo info = default)
    {
        if(Scores == null)
            Scores = new Dictionary<PlayerRef, int>();
        for(int i = 0; i < keys.Length; i++)
        {
            if (Scores.ContainsKey(keys[i]))
            {
                Scores[keys[i]] = values[i];
            }
            else
            {
                Scores.Add(keys[i], values[i]);
            }
        }
    }
    [Rpc]
    public static void Rpc_SyncTeamScore(NetworkRunner runner, int[] values, RpcInfo info = default)
    {
        if (TeamScore == null)
            TeamScore = new int[3] {0,0,0};
        for (int i = 0; i < values.Length; i++)
        {
            TeamScore[i] =  values[i];
        }
    }
    #endregion
}
