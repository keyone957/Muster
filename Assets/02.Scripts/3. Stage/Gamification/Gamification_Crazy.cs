using System;
using Fusion;
using UnityEngine;
using UniRx;
using UniRx.Triggers;
using System.Linq;

// Gamification 중 하나인 "열광"을 담당하는 스크립트
// 최초 작성자 : 김기홍
// 수정자 : -
// 최종 수정일 : 2024-02-04

// 참고사항
//  - Gamification은 Server에 의해서만 호출된다.


public class Gamification_Crazy : Gamification
{
    private float elapsedTime = 0;
    [SerializeField] private float gameDuration = 10;
    [SerializeField] private float readyDuration = 5;
    static NetworkRunner _runner;
    private void Awake()
    {
        _runner = FindObjectOfType<NetworkRunner>();
    }

    public override void StartGamification()
    {
        Ready();
    }

    ////////////////////////////////////
    ////////////////////////////////////
    // Sequence, Server에서 실행
    private void Ready()
    {
        Debug.Log("Ready");

        // Set Team
        NetworkGamification_Crazy.Rpc_SetTeam(_runner);

        // Set Screen (Video, Material)
        MediaManager.Instance.Server_TurnOffAll(); // 영상이 재생중이라면 종료, 스크린 오브젝트(메테리얼 적용) Off

        // Set Screen (Server)
        StageScreenUIManager.Instance.Server_ActiveMainScreen(true);
        StageScreenUIManager.Instance.Server_ActiveLeftScreen(true);
        StageScreenUIManager.Instance.Server_ActiveRightScreen(true);
        StageScreenUIManager.Instance.Server_ActiveSlider(true);

        // Set Text in Screen
        Server_SetStageScreenUI_Ready();
        StageScreenUIManager.Instance.Server_SetLeftScreenText("", "");
        StageScreenUIManager.Instance.Server_SetRightScreenText("", "");

          // UniRx
          var updateFunction =
            this.UpdateAsObservable()
            .TakeWhile(_ => elapsedTime < readyDuration);

        var startTime = DateTime.Now;
        updateFunction.Subscribe(_ =>
        {
            elapsedTime = (float)(DateTime.Now - startTime).TotalSeconds; // 경과 시간

            // Update Screen UI
            UpdateStageScreenUI_Ready(elapsedTime,readyDuration);

            // Gamification
            NetworkGamification_Crazy.Rpc_GetInput(_runner); // Input
        },
            _ => Debug.Log("Error"),
            () =>
            {
                StartGame();
            })
            .AddTo(gameObject);
    }
    private void StartGame()
    {
        Debug.Log("Game Start");

        var updateFunction =
            this.UpdateAsObservable()
            .TakeWhile(_ => elapsedTime < gameDuration);

        // Set Text in Screen
        Server_SetStageScreenUI_Game();

        // UniRx
        var startTime = DateTime.Now;
        updateFunction.Subscribe(_ =>
        {
            elapsedTime = (float)(DateTime.Now - startTime).TotalSeconds; // 경과 시간

            // Update Screen UI
            UpdateStageScreenUI_Game(elapsedTime,gameDuration);

            // Gamification
            NetworkGamification_Crazy.Rpc_GetInput(_runner); // Input
        },
            _ => Debug.Log("Error"),
            () =>
            {
                // End Gamification
                Debug.Log("End Game");
                NetworkGamification_Crazy.Rpc_EndGamification(_runner);
                ShowGameResult();
            })
            .AddTo(gameObject);
    }
    // StartGame에서 진행된 게이미피케이션의 결과 출력
    private void ShowGameResult()
    {
        // 게임 결과 메인 화면에 송출
        var team1Score = NetworkDataManager.TeamScore[1];
        var team2Score = NetworkDataManager.TeamScore[2];
        var winTeam = team1Score > team2Score ? 1 : 2;
        StageScreenUIManager.Instance.Server_SetMainScreenText((winTeam == 1 ? "A" : "B") + "팀의 승리\n총 점수: " + NetworkDataManager.TeamScore[winTeam]);

        // 불기둥 작동
        StageVFXManager.instance.Server_ActiveFire(true);
        Observable.Timer(TimeSpan.FromSeconds(2))
            .Do(_ => StageVFXManager.instance.Server_ActiveFire(false))
            .Subscribe().AddTo(this);

        // 응원봉 깜빡임
        var startTime = DateTime.Now;
        Observable.Interval(TimeSpan.FromSeconds(0.125))
           .TakeWhile(_ => (DateTime.Now - startTime).TotalSeconds <= 3) // 3초 동안 실행
            .Subscribe(
            _ =>
            {
                // 응원봉 깜빡이는 함수
            },
            _ => Debug.Log("Error"),
            () =>
            {
                // 게이미피케이션 종료
                Debug.Log("게임 종료");

                // Set Screen (화면 Off)
                StageScreenUIManager.Instance.Server_ActiveMainScreen(false);
                StageScreenUIManager.Instance.Server_ActiveLeftScreen(false);
                StageScreenUIManager.Instance.Server_ActiveRightScreen(false);
                MediaManager.Instance.Server_PlayerVideo();

                // Set Screen (Video, Material)
                MediaManager.Instance.Server_PlayerVideo(); // 영상이 재생, 스크린 오브젝트(메테리얼 적용) Off
            }
            ).AddTo(this);
    }

    /////////////////////////////////////////
    // Suppport Function
    private void Server_SetStageScreenUI_Ready()
    {
        StageScreenUIManager.Instance.Server_SetMainScreenText("곧 게임이 시작됩니다!\n응원봉을 흔들 준비를 해주세요");
    }
    private void UpdateStageScreenUI_Ready(float _elapsed, float _total)
    {
        // Slider
        StageScreenUIManager.Instance.Server_SetSlider(_elapsed, _total);
    }
    private void Server_SetStageScreenUI_Game()
    {
        StageScreenUIManager.Instance.Server_SetMainScreenText("응원봉을 흔들어 점수를 획득하세요!");
    }
    private void UpdateStageScreenUI_Game(float _elapsed, float _total)
    {
        // Slider
        StageScreenUIManager.Instance.Server_SetSlider(_elapsed, _total);

        // Show Score At Screen
        NetworkGamification_Crazy.Rpc_UpdateStageScreenUI_Game(_runner);
    }
}