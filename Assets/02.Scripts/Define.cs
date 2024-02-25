public static class Define
{
    public static class SceneName
    {
        public const string _title = "Title";
        public const string _start = "Start";
        public const string _game = "Stage";
    }

    public const string _systemRoot = "System";
    public const string _soundManagerPrefabPath = _systemRoot + "/SoundManager";
    public const string _bgmRoot = "BGM";
    public const string _soundRoot = "Sound";
    private const string _stickListFileName = "StickList";
    private const string _concertListFileName = "ConcertList";
    private const string _fileListFileName = "FileList";
    public const string _stickListPath = _systemRoot + "/" + _stickListFileName;
    public const string _concertListPath = _systemRoot + "/" + _concertListFileName;
    public const string _fileListPath = _systemRoot + "/" + _fileListFileName;
    public const string _modelRoot = "Model";
    public const string _freeCamPrefab = "Stage/FreeCam";
    public const string _ESFileName = "Squencer.json";

    // 사운드 리소스
    public const string _titleBGM = "titleBGM";
    public const string _menuSelectSound = "decide";
    public const string _selectSound = "Select";

    // Start UI 리소스
    public const string _uiTitleSelectPrefabPath = "Title/UITitleSelect";
    public const string _uiLoginPrefabPath = "Title/UITitleLogin";
    public const string _uiAccSettingPrefabPath = "Title/UIAccSetting";
    public const string _uiIdolMenuPrefabPath = "Title/UITitleIdolMenu";
    public const string _uiStaffMenuPrefabPath = "Title/UITitleStaffMenu";
    public const string _uiUserMenuPrefabPath = "Title/UITitleUserMenu";
    public const string _uiConcertSettingPrefabPath = "Title/UIConcertSetting";
    public const string _uiStageSettingPrefabPath = "Title/UIStageSetting";
    public const string _uiUserSettingPrefabPath = "Title/UIUserSetting";
    public const string _uiAvatarSettingPrefabPath = "Title/UIAvatarSetting";
    public const string _uiStickSettingPrefabPath = "Title/UIStickSetting";
    public const string _uiStickItemPrefabPath = "Title/StickItem";
    public const string _uiConcertListPrefabPath = "Title/ConcertList";
    public const string _uiFileListgPrefabPath = "Title/FileList";
    public const string _uiGudgeSettingPrefabPath = "Title/UIGudgeSetting";
    public const string _uiColorSettingPrefabPath = "Title/UIColorSetting";
    public const string _uiVideoSettingPrefabPath = "Title/UIVideoSetting";
    public const string _uiSubVideoSettingPrefabPath = "Title/UISubVideoSetting";
    public const string _uiPassiveMenuPrefabPath = "Title/UIPassiveMenu";

    //Stage UI 리소스
    public const string _uiIdolPanelpPrefabPath = "Stage/UIIdolPanel";
    public const string _uiAudiencePrefabPath = "Stage/UIAudiencePanel";
    public const string _uiTokenSettingPrefabPath = "Title/UITokenSetting";
    public const string _uiSeatBtnPrefabPath = "Stage/UISeatBtn";

    public const string _uiUserEnter = "Stage/UIStageUserEnter";
    public const string _uiIdolEnter = "Stage/UIStageIdolEnter";

    public const string _uiIntermissionNotice = "Stage/UIIntermissionNotice";
    public const string _uiIntermissionIdol = "Stage/UIIntermissionIdol";
    public const string _uiIntermissionUser = "Stage/UIIntermissionUser";
    public const string _uiAudienceStage = "Stage/UIAudienceStage";
    public const string _uiSoundSetting = "Stage/UISoundSetting";
    public const string _uiActiveMenu = "Title/UIActiveMenu";
    public const string _uiAvartaSetting = "Title/UIAvatarSetting";
    public const string _uiPlayerSetting = "Title/UIPlayerSetting";
    public const string _uiStageSetting = "Title/UIStageSetting";
    public const string _uiVideoSetting = "Title/UIVideoSetting";
    // public const string _uiSquencerPrefab = "Seqeuence/Squencer"; 비디오 시퀀스
    public const string _uiSquencerPrefab = "Seqeuence/AudioSequencer";

    // 배경 리소스
    public const string _backgroundPrefabPath = _systemRoot + "/Background";
    public const string _backgroundRoot = "Texture";
    public const string _spritePrefabPath = _systemRoot + "/Sprite";
    public const string _spriteRoot = "Texture/Sprite";
    public const string _foregroundPrefabPath = _systemRoot + "/Foreground";

    // 게임 규칙
    public const float _foregroundCoverDefaultDuration = 1.0f;
}