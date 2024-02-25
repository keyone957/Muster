using DG.Tweening;
using UnityEngine;

// Stage 씬의 UI (Intermission 등) 관리, StageUIManager와 헷갈리지 않도록 주의
// 최초 작성자 : 김기홍
// 수정자 : 김효중 
// 최종 수정일 : 2024-01-31

public class StageSceneUIManager : UIManager
{
    public static StageSceneUIManager _instance;
    // UI
    public FadeOverlay _FadeOverlay { get; private set; }
    [SerializeField] private Transform _mainPanel = null;

    // 
    private UIStageUserEnter _stageUserEnterMenu;
    private UIStageIdolEnter _stageIdolEnterMenu;
    private UIIntermissionIdol _intermissionIdol;
    private UIIntermissionUser _intermissionUser;
    private UIIntermissionNotice _intermissionNotice;
    private UIActiveMenu _activeMenu;
    private UIAvatarSetting uIAvatarSetting;
    private UIStickList uIStickList;
    private UIAvatarSetting uiPlayerSetting;
    private UIStickList uiStageSetting;
    private UIVideoSetting uIVideoSetting;
    private UISequencer uISequencer;
    private AudienceStage audienceStage;
    private SoundSetting soundSetting;
    public UIWindow _curOpenWindow;

    private void Awake()
    {
        _instance = this;
        _FadeOverlay = FindObjectOfType<FadeOverlay>();
    }
    void Start()
    {
        switch (SettingManager._instance.role)
        {
            case PlayerManager.Role.Idol:
                // FlowChart 수정에 따른 코드 수정
                // - Enter 제거, Intermission으로 바로 향하도록 코드 수정
                /*
                if (_stageIdolMenu == null)
                {
                    _stageIdolMenu = OpenWindow(Define._uiIdolEnter, _mainPanel) as UIStageIdolEnter;
                    _curOpenWindow = _stageIdolMenu;
                }
                */

                // _intermissionIdol = OpenWindow(Define._uiIntermissionIdol, _mainPanel) as UIIntermissionIdol;
                // _curOpenWindow = _intermissionIdol;
                // activeMenu로 변경
                // OpenActiveMenu();
                break;

            case PlayerManager.Role.Audience:
                if (_stageUserEnterMenu == null)
                {
                    _stageUserEnterMenu = OpenWindow(Define._uiUserEnter, _mainPanel) as UIStageUserEnter;
                    _curOpenWindow = _stageUserEnterMenu;
                }
                break;
        }
    }
    public void OpenIntermissionUI()
    {
        //CloseCurWindow();
        switch (SettingManager._instance.role)
        {
            case PlayerManager.Role.Idol:
                // if (_intermissionIdol == null)
                // {
                //     _intermissionIdol = OpenWindow(Define._uiIntermissionIdol, _mainPanel) as UIIntermissionIdol;
                //     _curOpenWindow = _intermissionIdol;
                // }
                // activeMenu로 변경
                //OpenActiveMenu();
                break;

            case PlayerManager.Role.Audience:
                if (_intermissionUser == null)
                {
                    _intermissionUser = OpenWindow(Define._uiIntermissionUser, _mainPanel) as UIIntermissionUser;
                    _curOpenWindow = _intermissionUser;
                }
                break;
        }
    }
    // 입장 조건 만족 시 true (HMD기기를 착용하고 있다면 true)
    public bool OpenNotice()
    {
        CloseCurWindow();

        _intermissionNotice = OpenWindow(Define._uiIntermissionNotice, _mainPanel) as UIIntermissionNotice;
        _curOpenWindow = _intermissionNotice;
        return InputManager.instance.IsHMDTracking();
    }
    public void CloseCurWindow()
    {
        if (_curOpenWindow != null)
        {
            CloseWindow(_curOpenWindow);
        }
    }

    //////////////////////////////////////////////////////////////////

    // 세팅 추가
    public void OpenActiveMenu()
    {
        if (_activeMenu == null)
        {
            _activeMenu = OpenWindow(Define._uiActiveMenu, _mainPanel) as UIActiveMenu;
            StageSceneManager.instance.EnterSession();
            _curOpenWindow = _activeMenu;
        }
    }
    public void CloseActiveMenu()
    {
        if (_curOpenWindow != null)
        {
            CloseWindow(_curOpenWindow);
        }
    }
    public void OpenAvartarSetting()
    {
        if (uIAvatarSetting == null)
        {
            uIAvatarSetting = OpenWindow(Define._uiAvartaSetting, _mainPanel) as UIAvatarSetting;
            _curOpenWindow = uIAvatarSetting;
        }
    }
    public void CloseAvartaSetting()
    {
        if (_curOpenWindow != null)
        {
            CloseWindow(_curOpenWindow);
        }
    }
    public void OpenStickSetting()
    {
        if (uIStickList == null)
        {
            uIStickList = OpenWindow(Define._uiStickSettingPrefabPath, _mainPanel) as UIStickList;
            _curOpenWindow = uIStickList;
        }
    }
    public void CloseStickSetting()
    {
        if (_curOpenWindow != null)
        {
            CloseWindow(_curOpenWindow);
        }
    }
    public void OpenVideoSetting()
    {
        if (uISequencer == null)
        {
            uISequencer = OpenWindow(Define._uiSquencerPrefab, _mainPanel) as UISequencer;
            _curOpenWindow = uISequencer;
        }
        else
        {
            Vector3 newPos = uISequencer._RectTrans.transform.position;
            newPos.z -= 10;
            uISequencer._RectTrans.transform.position = newPos;
        }
    }
    public void CloseVideoSetting()
    {
        if (_curOpenWindow != null)
        {
            CloseWindow(_curOpenWindow);
        }
    }
    public void OpenAudienceStage()
    {
        if (audienceStage == null)
        {
            audienceStage = OpenWindow(Define._uiAudienceStage, _mainPanel) as AudienceStage;
            _curOpenWindow = audienceStage;
        }
    }
    public void OpenSoundSetting()
    {
        if (soundSetting == null)
        {
            soundSetting = OpenWindow(Define._uiSoundSetting, _mainPanel) as SoundSetting;
            _curOpenWindow = soundSetting;
        }
    }
    // 이하 임시
    public void OpenPlayerSetting()
    {
        if (uiPlayerSetting == null)
        {
            uiPlayerSetting = OpenWindow(Define._uiPlayerSetting, _mainPanel) as UIAvatarSetting;
            _curOpenWindow = uiPlayerSetting;
        }
    }
    public void ClosePlayerSetting()
    {
        if (_curOpenWindow != null)
        {
            CloseWindow(_curOpenWindow);
        }
    }
    public void OpenStageSetting()
    {
        if (uiStageSetting == null)
        {
            uiStageSetting = OpenWindow(Define._uiStageSettingPrefabPath, _mainPanel) as UIStickList;
            _curOpenWindow = uiStageSetting;
        }
    }
    public void CloseStageSetting()
    {
        if (_curOpenWindow != null)
        {
            CloseWindow(_curOpenWindow);
        }
    }


}
