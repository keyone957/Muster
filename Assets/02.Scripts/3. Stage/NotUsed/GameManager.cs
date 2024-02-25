using System;
using System.IO;
using UniRx;
using UniRx.Triggers;
using UnityEngine;
using UnityEngine.XR;

//////////////////////////////////////////////////
//////////////////////////////////////////////////
// 사용하지 않음
//////////////////////////////////////////////////
//////////////////////////////////////////////////


// InputManager 함수 변경에 따른 파라미터 값 변경
// 최초 작성자 : 김기홍
// 수정자 : 홍원기
// 최종 수정일 : 2023-12-09

public class GameManager : MonoBehaviour
{
    [SerializeField] private GameObject targetObj;
    [SerializeField] private GameObject fromObj;
    private bool canPlay;
    public float gameDuration = 10f;

    // Light Particle
    ParticleController lightParticleController = null;

    public static GameManager instance = null;
    private void Awake()
    {
        /*
        #region Singleton
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(this.gameObject);
        }
        #endregion
        */
    }

    // 입력과 그에 따른 행동 (함수 호출)을 담당함 (UniRx를 통해 구현)
    private void Start()
    {
        /*
        // Click을 하면 게임 준비
        canPlay = false;
        this.UpdateAsObservable()
            .Where(_ =>
            {
                // return InputManager.instance.GetDeviceBtn(InputManager.instance._leftController,CommonUsages.primaryButton);
                return Input.GetKeyDown(KeyCode.Space);
            })
            .Where(_ => canPlay)
            .Subscribe(_ =>
            {
                StartGame(gameDuration);
                canPlay = false;
            }).AddTo(this);
            
        this.UpdateAsObservable()
            .Where(_ =>
            {
                return InputManager.instance.GetDeviceBtnSec(InputManager.instance._rightController);
                //return Input.GetKeyDown(KeyCode.Space);
            })
            .Where(_ => canPlay)
            .Subscribe(_ =>
            {
                FlameManager._instance.ActivateAllFire(true);
            }).AddTo(this);
        */
    }

    // 현재 기획서에 적용된 게이미피케이션 시작
    private void StartGame(float duration)
    {
        /* 
        Debug.Log("Game Start");

        TimeSpan interval = TimeSpan.FromSeconds(Time.deltaTime);
        var startTime = DateTime.Now;

        //////////////////////////////
        /// For Test
        var playerRole = PlayerManager.Role.Audience;
        var playerID = 0;
        var playerTeamID = 0;
        /// 


        // 화면 On
        StageUIManager.instance.ActiveLeftScreen(true);
        StageUIManager.instance.ActiveRightScreen(true);
        StageUIManager.instance.ActiveMainScreen(true);


        if (playerRole == PlayerManager.Role.Audience)
        {
            // Light Particle
            if (lightParticleController == null)
            {
                string path = "Stage/LightParticle";
                GameObject obj = Instantiate(Resources.Load<GameObject>(path), fromObj.transform);
                lightParticleController = obj.GetComponent<ParticleController>();
            }
            lightParticleController.InitAttractorMove(targetObj.transform);

            StageUIManager.instance.SetRightScreenText("?");
            StageUIManager.instance.SetLeftScreenText("?");
            // 점수 측정
            if (playerTeamID == 0)
            {
                // Input Data
                var leftSpeedStream = InputManager.instance.GetDeviceSpeed(InputManager.instance._leftController, duration, InputManager.instance.IsHandOverHMD);
                leftSpeedStream.Subscribe(speed =>
                {
                    if (InputManager.instance.IsHandOverHMD(InputManager.instance._leftController))
                    {
                        var score = ScoreManager.instance.AddScore(playerID, playerTeamID, ScoreManager.instance.ConvertSpeedToScore(speed));
                        StageUIManager.instance.SetLeftScreenText(score);
                    }
                });
                var rightSpeedStream = InputManager.instance.GetDeviceSpeed(InputManager.instance._rightController, duration, InputManager.instance.IsHandOverHMD);
                rightSpeedStream.Subscribe(speed =>
                {
                    if (InputManager.instance.IsHandOverHMD(InputManager.instance._rightController))
                    {
                        var score = ScoreManager.instance.AddScore(playerID, playerTeamID, ScoreManager.instance.ConvertSpeedToScore(speed));
                        StageUIManager.instance.SetLeftScreenText(score);
                    }
                });
            }
            else if (playerTeamID == 1)
            {
                // Input Data
                var leftSpeedStream = InputManager.instance.GetDeviceSpeed(InputManager.instance._leftController, duration, InputManager.instance.IsHandOverHMD);
                leftSpeedStream.Subscribe(speed =>
                {
                    if (InputManager.instance.IsHandOverHMD(InputManager.instance._leftController))
                    {
                        var score = ScoreManager.instance.AddScore(playerID, playerTeamID, ScoreManager.instance.ConvertSpeedToScore(speed));
                        StageUIManager.instance.SetRightScreenText(score);
                    }
                });
                var rightSpeedStream = InputManager.instance.GetDeviceSpeed(InputManager.instance._rightController, duration, InputManager.instance.IsHandOverHMD);
                rightSpeedStream.Subscribe(speed =>
                {
                    if (InputManager.instance.IsHandOverHMD(InputManager.instance._rightController))
                    {
                        var score = ScoreManager.instance.AddScore(playerID, playerTeamID, ScoreManager.instance.ConvertSpeedToScore(speed));
                        StageUIManager.instance.SetRightScreenText(score);
                    }
                });
            }
        }

        // Progress Bar
        Observable
           .Interval(interval)
           .TakeWhile(_ => (DateTime.Now - startTime).TotalSeconds <= duration) // 10초 동안 실행
           .Subscribe(
            _ => {
                var elapsedTime = (float)(DateTime.Now - startTime).TotalSeconds;
                StageUIManager.instance.SetSlider(elapsedTime, duration);
                },
            _ => Debug.Log("Error"),
            () =>
            {
                Debug.Log("Complete"); // 총 점수 전달
                if(lightParticleController != null) lightParticleController.EndAttractorMove();
                ShowGameResult();
            }
            ).AddTo(this);
        */
    }

    // StartGame에서 진행된 게이미피케이션의 결과 출력
    private void ShowGameResult()
    {
        /*  
        // 게임 결과 메인 화면에 송출
        StageUIManager.instance.SetMainScreenText("A팀의 승리\n총 점수: " + ScoreManager.instance.GetTeamScore(0));

        // 불기둥 작동
        StageVFXManager.instance.ActiveFire(true);
        Observable.Timer(TimeSpan.FromSeconds(2))
            .Do(_ => StageVFXManager.instance.ActiveFire(false))
            .Subscribe().AddTo(this);

        // 응원봉 깜빡임
      var startTime = DateTime.Now;
        Observable.Interval(TimeSpan.FromSeconds(0.125))
           .TakeWhile(_ => (DateTime.Now - startTime).TotalSeconds <= 3) // 3초 동안 실행
            .Subscribe(
            _ => NetworkPlayerManager.instance.MakeAllPlayerLightStickBlink(),
            _ => Debug.Log("Error"),
            () => TerminateGame()
            ).AddTo(this);
        */
    }

    // 게이미피케이션 종료
    private void TerminateGame()
    {
        /*
        Debug.Log("게임 종료");

        canPlay = true;

        // 화면 On
        StageUIManager.instance.ActiveLeftScreen(false);
        StageUIManager.instance.ActiveRightScreen(false);
        StageUIManager.instance.ActiveMainScreen(false);
        */
    }

}
