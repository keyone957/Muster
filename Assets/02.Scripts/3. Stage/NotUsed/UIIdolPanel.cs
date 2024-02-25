using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;
using UnityEngine.XR.Management;
//테스트 위한 ui비활
//최초 작성자: 홍원기
//수정자:
//최종 수정일: 2023-12-04
public class UIIdolPanel : UIWindow
{
    [SerializeField]
    private Button _startFlyCamBtn;
    [SerializeField]
    private Button _endFlyCameBtn;
    [SerializeField]
    private GameObject _mainPanel;
    [SerializeField]
    private GameObject _flyCamPanel;
    [SerializeField]
    private GameObject _connectPanel;
    [SerializeField]
    private TextMeshProUGUI _quest2Connect;
    [SerializeField]
    private TextMeshProUGUI _HMDMounted;
    [SerializeField]
    private TextMeshProUGUI _mocopiConnect;
    private bool isFlyCamMode=false;

    private bool isReady = false;
    [SerializeField]
    private Button _disableDevice;
    [SerializeField]
    private Button _enableDevice;

    private void Start()
    {
        _startFlyCamBtn.onClick.AddListener(StartFreeFlyMode);
        _endFlyCameBtn.onClick.AddListener(EndFreeFlyCamMode);
        _enableDevice.onClick.AddListener(StartStage);
        _flyCamPanel.SetActive(false);
    }
    //메인 카메라 변경 후 플레이어 xrRig활성화
    public void EnableXR()
    {
        StageCameraManager.instance.SetPlayerCamera();
        UIStageStartMenu._Instance._idolPanel.SetActivate(false);
        isReady = false;
    }
    //모든 세팅을 맞추고 기기 연결 확인을 위한함수
    //아직 미완
    private void StartStage()
    {
        _connectPanel.SetActive(true);
        isReady = true;
    }
    //자유시점 모드일때 esc키 입력 받고 EnableMouseCursor함수 실행
    private void Update()
    {
        if (isFlyCamMode && Keyboard.current.escapeKey.wasPressedThisFrame)
        {
            EnableMouseCursor();
        }
        //if (isReady&&InputManager.instance._isHMDTracking)
        //{ 
        //    EnableXR();
        //}
    }
    // 자유시점 모드 활성화
    private void StartFreeFlyMode()
    {
        isFlyCamMode = true;
        UIStageStartMenu._Instance._freeCamObj.SetActive(true);
        UIStageStartMenu._Instance._freeCamObj.GetComponent<FreeFlyCamera>()._active = true;
        SetPanelAlpha(0.0f);
        _mainPanel.SetActive(false);
        _flyCamPanel.SetActive(true);
    }
    //자유시점 모드 비활성화
    private void EndFreeFlyCamMode()
    {
        isFlyCamMode = false;
        UIStageStartMenu._Instance._freeCamObj.SetActive(false);
        SetPanelAlpha(0.3f);
        _mainPanel.SetActive(true);
        _flyCamPanel.SetActive(false);

    }

    //메인 패널 투명도 조절
    private void SetPanelAlpha(float alpha)
    {
        Color mainPanelColor = gameObject.GetComponent<Image>().color;
        mainPanelColor.a = alpha;
        gameObject.GetComponent<Image>().color = mainPanelColor;
    }

    //esc키 누르면 자유시점 모드에서 주변 UI버튼 누를수 있게 함
    private void EnableMouseCursor()
    {
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible= true;
        UIStageStartMenu._Instance._freeCamObj.GetComponent<FreeFlyCamera>()._active = false;
    }
    private void SetText(TextMeshProUGUI textObj, string text )
    {
        textObj.text = text;
    }
    //사용자의 퀘스트2 연결상태 , HMD착용 여부에 따른 텍스트 변경
    private void CheckDeviceState()
    {
        bool isConnectDevice = InputManager.instance.IsDeviceConnceted();
        bool isHMDTracking=InputManager.instance.IsHMDTracking();
        if (isConnectDevice)
        {
            SetText(_quest2Connect, "퀘스트2 연결 완료");
        }
        if (isHMDTracking)
        {
            SetText(_HMDMounted, "VR 헤드셋 착용 완료");
        }
        //TODO : 모코피 연결 관련 bool값에 따라 텍스트 변경
    }
}
