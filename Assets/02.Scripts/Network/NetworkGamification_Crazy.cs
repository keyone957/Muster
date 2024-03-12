using Fusion;
using System;
using UniRx;
using UnityEngine;

// Network 환경에서 Gamification_Crazy를 관리
// 최초 작성자 : 김기홍
// 수정자 : -
// 최종 수정일 : 2024-02-12
public class NetworkGamification_Crazy : SimulationBehaviour
{
    static NetworkObject playerNetworkObject;

    // Audience가 보유한 Local Data
    static int localScore = 0;
    static int playerId = 0;
    static int teamId = 0;

    static IDisposable syncScoreDisposable;

    public static void InitializeData()
    {
        localScore = 0;
        playerId = 0;
        teamId = 0;
        syncScoreDisposable = null;
    }

    /////////////////////////////////////////
    // Rpc Function
    [Rpc(RpcSources.StateAuthority, RpcTargets.InputAuthority | RpcTargets.StateAuthority)]
    public static void Rpc_SetTeam(NetworkRunner runner, RpcInfo info = default) // 모든 Client에게
    {
        if (StageSceneManager.instance.PlayerRole == PlayerManager.Role.Idol)
        {
            StageSceneManager.instance.playerManager.SetTeamID(0);
            return;
        }

        var playerRef = StageSceneManager.instance.PlayerRef;
        var idolRef = NetworkDataManager.IdolRef;
        playerNetworkObject = NetworkDataManager.GetNetworkObject(playerRef);

        // Determine Team
        teamId = DivideTeam(playerNetworkObject.transform.position); ;
        playerId = StageSceneManager.instance.PlayerNetworkID;
        StageSceneManager.instance.playerManager.SetTeamID(teamId);
        Debug.Log("Team : " + teamId);

        syncScoreDisposable = Observable.Interval(TimeSpan.FromSeconds(0.1f))
            .Subscribe(_ =>
            {
                if (localScore != 0 && teamId != 0)
                {
                    NetworkDataManager.Rpc_AddScore(runner, playerId, teamId, localScore/4);
                    Rpc_EmitLightParticle(runner);
                    localScore = 0;
                }
            });
    }
    [Rpc(RpcSources.StateAuthority, RpcTargets.InputAuthority | RpcTargets.StateAuthority)]
    public static void Rpc_EndGamification(NetworkRunner runner, RpcInfo info = default) // 모든 Client에게
    {
        if (StageSceneManager.instance.PlayerRole == PlayerManager.Role.Idol) return;
        syncScoreDisposable.Dispose();
    }

    [Rpc(RpcSources.StateAuthority, RpcTargets.InputAuthority | RpcTargets.StateAuthority)]
    public static void Rpc_UpdateStageScreenUI_Game(NetworkRunner runner, RpcInfo info = default)
    {
        // Score를 Screen에 나타내기
        var team1Score = NetworkDataManager.TeamScore[1];
        var team2Score = NetworkDataManager.TeamScore[2];

        StageScreenUIManager.Instance.Local_SetLeftScreenText(team1Score);
        StageScreenUIManager.Instance.Local_SetRightScreenText(team2Score);

        if (StageSceneManager.instance.PlayerRole == PlayerManager.Role.Idol) return;

        var teamId = StageSceneManager.instance.playerManager.GetTeamID();
        var playerId = StageSceneManager.instance.playerManager.GetPlayerID();
        int playerScore;
        if (NetworkDataManager.Scores.TryGetValue(playerId, out playerScore) == false)
        {
            playerScore = 0;
        }

        // 관객은 상대편 점수를 볼 수 없음
        // Idol 기준 오른쪽 팀
        if (teamId == 1)
        {
            StageScreenUIManager.Instance.Local_SetLeftScreenSubText(playerScore);
            StageScreenUIManager.Instance.Local_SetRightScreenText("?", "");
        }
        // Idol 기준 왼쪽 팀
        else if (teamId == 2)
        {
            StageScreenUIManager.Instance.Local_SetLeftScreenText("?", "");
            StageScreenUIManager.Instance.Local_SetRightScreenSubText(playerScore);
        }
    }

    [Rpc(RpcSources.StateAuthority, RpcTargets.InputAuthority)]
    public static void Rpc_GetInput(NetworkRunner runner, RpcInfo info = default)  // Audience만 
    {
        if (StageSceneManager.instance.PlayerRole == PlayerManager.Role.Idol) return;

        // Input Data
        var leftSpeedStream = InputManager.instance.GetDeviceSpeed(InputManager.instance._leftController, InputManager.instance.IsHandOverHMD);
        leftSpeedStream.Subscribe(speed =>
        {
            if (InputManager.instance.IsHandOverHMD(InputManager.instance._leftController))
            {
                localScore += (int)(speed);
            }
        }).Dispose();

        var rightSpeedStream = InputManager.instance.GetDeviceSpeed(InputManager.instance._rightController, InputManager.instance.IsHandOverHMD);
        rightSpeedStream.Subscribe(speed =>
        {
            if (InputManager.instance.IsHandOverHMD(InputManager.instance._rightController))
            {
                localScore += (int)(speed);
            }
        }).Dispose();
    }

    /////////////////////////////////////////
    // Light Particle Function
    [Rpc(RpcSources.StateAuthority, RpcTargets.InputAuthority)]
    public static void Rpc_EmitLightParticle(NetworkRunner runner, RpcInfo info = default)
    {
        Debug.Log("Gamification Manager Send Messege To Client : Emit LightParticle ");
        LightParticleManager lpm = FindObjectOfType<LightParticleManager>();
        if (lpm)
        {
            lpm.Server_EmitLightParticle();
        }
    }
    /////////////////////////////////////////
    // Supported Function

    // 위치를 기반으로 팀을 나누는 함수
    // 팀은 1팀 2팀이 존재하며, 무대를 바라보는 기준으로 왼쪽 1팀 - 오른쪽 2팀이다.
    private static int DivideTeam(Vector3 curPos)
    {
        if (curPos.x > 0)
        {
            return 1;
        }
        else
        {
            return 2;
        }
    }
}
