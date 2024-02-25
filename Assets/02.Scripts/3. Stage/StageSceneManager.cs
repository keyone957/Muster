using Fusion;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;

// StageScene을 총괄하는 스크립트
// 최초 작성자 : 김기홍
// 수정자 : 김효중
// 최종 수정일 : 2024-02-15
public class StageSceneManager : MonoBehaviour
{
    // Singleton
    public static StageSceneManager instance;
    // 
    public StageSceneUIManager _UIManager { get; private set; }

    // Player 관련 정보 (EnterStage()함수가 호출된 이후에 초기화 됨)
    public PlayerManager playerManager;
    public PlayerManager.Role PlayerRole { get; private set; }
    public int PlayerNetworkID { get; private set; }
    public PlayerRef PlayerRef { get; private set; }

    // Variables 
    public bool IsDataInitialized() => playerManager != null;
    int call = 0;


    // Stage Level
    public enum StageLevel
    {
        Enter,
        Intermission,
        Stage
    }
    public StageLevel stageLevel { get; private set; }

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
        _UIManager = FindObjectOfType<StageSceneUIManager>();
    }
    private void Update()
    {
        // 공연 시작과 종료
        if (call % 2 == 0 && Input.GetKeyDown(KeyCode.Space) && PlayerRole == PlayerManager.Role.Idol)
        {
            call++;
            LineEventPlayer.instance.startAni("", 0);
        }
        else if(call % 2 == 1 && Input.GetKeyDown(KeyCode.Space) && PlayerRole == PlayerManager.Role.Idol)
        {
            call++;
            LineEventPlayer.instance.stopAni();
        }

        // 인터미션 입장
        if (Input.GetKeyDown(KeyCode.Escape) && stageLevel == StageLevel.Stage)
        {
            EnterIntermission();
        }
    }
    public void EnterSession()
    {
        _UIManager.CloseCurWindow();

        // 초기 설정
        /*
        OVRManager.HMDMounted += EnterStage;
        OVRManager.HMDUnmounted += EnterIntermission;
        */
        playerManager = FindObjectOfType<PlayerManager>();
        PlayerRole = playerManager.GetRole();

        // Idol은 NULL
        PlayerRef = NetworkManager._instance.CurPlayerRef;
        PlayerNetworkID = NetworkManager._instance.CurPlayerID; // User마다 어떤 값을 가지는지 확인해야함
        playerManager.SetPlayerID(PlayerNetworkID);

        // Idol 전용 초기 설정
        if (PlayerRole == PlayerManager.Role.Idol)
        {
            playerManager.transform.GetChild(0).localPosition = new Vector3(0, -0.2f, -0.16f);
        }
        EnterIntermission();

        stageLevel = StageLevel.Enter;
    }
    public void EnterIntermission()
    {
        _UIManager.OpenIntermissionUI();
        playerManager.MovePlayerToIntermission();
        if (PlayerRole == PlayerManager.Role.Idol)
        {
            UIActiveMenu.instrance.SetUIOff();
        }
        else if (PlayerRole == PlayerManager.Role.Audience)
        {
            GameObject targetCamera = null;
            if (targetCamera == null) { targetCamera = GameObject.Find("PC UI Cam"); }
            if (targetCamera != null)
            {
                targetCamera.GetComponent<Camera>().enabled = true;
            }
        }

        stageLevel = StageLevel.Intermission;
    }
    public void EnterStage()
    {
        // PC UI 세팅 (Notice)
        // if (_UIManager.OpenNotice() == false) return;

        // 공용
        playerManager.MovePlayerToStage(); // Player 이동
        stageLevel = StageLevel.Stage;

        // 아이돌
        if (PlayerRole == PlayerManager.Role.Idol)
        {
            OVRBody.SuggestBodyTrackingCalibrationOverride(UIActiveMenu.instrance.userHeight); // 아이돌 키 설정
            UIActiveMenu.instrance.SetUIOn();
        }

        // 관객
        else
        {
            _UIManager.CloseCurWindow();
            GameObject targetCamera = null;
            if (targetCamera == null) { targetCamera = GameObject.Find("PC UI Cam"); }
            if (targetCamera != null)
            {
                targetCamera.GetComponent<Camera>().enabled = false;
            }
        }

    }
}
