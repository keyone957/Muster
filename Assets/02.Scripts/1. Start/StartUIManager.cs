using UnityEngine;

// 입장전 타이틀 UI 관리용
// 최초 작성자 : 김효중
// 수정자 : 
// 최종 수정일 : 2023-11-26
public class StartUIManager : UIManager
{
    // UI
    public FadeOverlay _FadeOverlay { get; private set; }
    [SerializeField] private Transform _mainPanel = null;

    // 
    private UITitleSelect _titleSelect = null;
    private UILongin _login = null;
    private UITitleIdolMenu _idolMenu = null;
    private UITitleStaffMenu _staffMenu = null;
    private UITitleUserMenu _userMenu = null;
    private UIStickList _stickList = null;
    private UIAvatarSetting _avatarSetting = null;
    private UIColorSetting _colorSetting = null;
    private UIGudgeSetting _gudgeSetting = null;
    private UIUserSetting _tokensetting = null;
    private UIVideoSetting _videoSetting = null;
    private UISubVideoSetting _subvideoSetting = null;
    private UIPassiveMenu _stageMenu = null;
    private UIPassiveMenu _passiveMenu = null;
    private FileList _fileList = null;


    public void Initailize()
    {
        _FadeOverlay = FindObjectOfType<FadeOverlay>(true);
        _FadeOverlay.DoFadeOut(0.0f);
    }

    protected override bool OnKeyInputOnComponent()
    {
        return _FadeOverlay.IsFading();
    }
    public void Pop()
    {
        this.CloseTop();
    }
    public void OpenTitleMenu()
    {
        if (_titleSelect == null)
        {
            _titleSelect = OpenWindow(Define._uiTitleSelectPrefabPath, _mainPanel) as UITitleSelect;
        }
    }

    public void CloseTitleMenu()
    {
        if (_titleSelect != null)
        {
            CloseWindow(_titleSelect);
            _titleSelect = null;
        }
    }
    public void EnableAllObjects()
    {
        // 현재 씬에서 모든 GameObject 찾기
        GameObject[] allObjects = GameObject.FindObjectsOfType<GameObject>();

        // 찾은 모든 GameObject에 대한 작업 수행
        foreach (var obj in allObjects)
        {
            // obj에 대한 작업 수행
            obj.SetActive(true);
        }
        _FadeOverlay.DoFadeOut(0.0f);
    }
    public void DisableAllObjects()
    {
        // 현재 씬에서 모든 GameObject 찾기
        GameObject[] allObjects = GameObject.FindObjectsOfType<GameObject>();

        // 찾은 모든 GameObject에 대한 작업 수행
        foreach (var obj in allObjects)
        {
            // obj에 대한 작업 수행
            obj.SetActive(false);
        }
    }
    #region Login
    public void OpenLogin()
    {
        if (_login == null)
        {
            _login = OpenWindow(Define._uiLoginPrefabPath, _mainPanel) as UILongin;
        }
    }

    public void CloseLogin()
    {
        if (_login != null)
        {
            CloseWindow(_login);
            _login = null;
        }
    }
    #endregion

    #region Menu by Role
    public void OpenIdolMenu()
    {
        if (_idolMenu == null)
        {
            _idolMenu = OpenWindow(Define._uiIdolMenuPrefabPath, _mainPanel) as UITitleIdolMenu;
        }
    }

    public void CloseIdolMenu()
    {
        if (_idolMenu != null)
        {
            CloseWindow(_idolMenu);
            _idolMenu = null;
        }
    }
    public void OpenStaffMenu()
    {
        if (_staffMenu == null)
        {
            _staffMenu = OpenWindow(Define._uiStaffMenuPrefabPath, _mainPanel) as UITitleStaffMenu;
        }
    }

    public void CloseStaffMenu()
    {
        if (_staffMenu != null)
        {
            CloseWindow(_staffMenu);
            _staffMenu = null;
        }
    }
    public void OpenUserMenu()
    {
        if (_userMenu == null)
        {
            _userMenu = OpenWindow(Define._uiUserMenuPrefabPath, _mainPanel) as UITitleUserMenu;
        }
    }

    public void CloseUserMenu()
    {
        if (_userMenu != null)
        {
            CloseWindow(_userMenu);
            _userMenu = null;
        }
    }
    #endregion

    #region Not Use
    public void OpenStickList()
    {
        if (_stickList == null)
        {
            _stickList = OpenWindow(Define._uiStickSettingPrefabPath, _mainPanel) as UIStickList;
        }
    }

    public void CloseStickList()
    {
        if (_stickList != null)
        {
            CloseWindow(_stickList);
            _stickList = null;
        }
    }
    public void OpenAvatarSetting()
    {
        if (_avatarSetting == null)
        {
            _avatarSetting = OpenWindow(Define._uiAvatarSettingPrefabPath, _mainPanel) as UIAvatarSetting;
        }
    }
    public void OpenColorSetting()
    {
        if (_colorSetting == null)
        {
            _colorSetting = OpenWindow(Define._uiColorSettingPrefabPath, _mainPanel) as UIColorSetting;
        }
    }
    public void OpenGudgeSetting()
    {
        if (_gudgeSetting == null)
        {
            _gudgeSetting = OpenWindow(Define._uiGudgeSettingPrefabPath, _mainPanel) as UIGudgeSetting;
        }
    }
    public void OpenSubVideoSetting()
    {
        if (_subvideoSetting == null)
        {
            _subvideoSetting = OpenWindow(Define._uiSubVideoSettingPrefabPath, _mainPanel) as UISubVideoSetting;
        }
    }
    public void OpenVideoSetting()
    {
        if (_videoSetting == null)
        {
            _videoSetting = OpenWindow(Define._uiVideoSettingPrefabPath, _mainPanel) as UIVideoSetting;
        }
    }
    public void OpenPassiveMenu()
    {
        if (_passiveMenu == null)
        {
            _passiveMenu = OpenWindow(Define._uiPassiveMenuPrefabPath, _mainPanel) as UIPassiveMenu;
        }
    }
    public void OpenStageMenu()
    {
        if (_stageMenu == null)
        {
            _stageMenu = OpenWindow(Define._uiTokenSettingPrefabPath, _mainPanel) as UIPassiveMenu;
        }
    }
    public void CloseStageMenu()
    {
        if (_stageMenu != null)
        {
            CloseWindow(_stageMenu);
            _stageMenu = null;
        }
    }
    public void OpenTokenSetting()
    {
        if (_tokensetting == null)
        {
            _tokensetting = OpenWindow(Define._uiTokenSettingPrefabPath, _mainPanel) as UIUserSetting;
        }
    }
    public void CloseTokenSetting()
    {
        if (_tokensetting != null)
        {
            CloseWindow(_tokensetting);
            _tokensetting = null;
        }
    }
    public void OpenFileList()
    {
        if (_fileList == null)
        {
            _fileList = OpenWindow(Define._uiFileListgPrefabPath, _mainPanel) as FileList;
        }
    }
    #endregion
}