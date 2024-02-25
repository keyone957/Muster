using System.Drawing;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

// 아이돌, 관리자용 무대 세팅 화면 UI 관리용
// 최초 작성자 : 김효중
// 수정자 : 김기홍
// 최종 수정일 : 2024-01-12
public class UITitleIdolMenu : UIWindow
{
    private StartSceneManager startSceneManager;

    [SerializeField] private Button _btnClose = null;
    [SerializeField] private Button _btnAvatar = null;
    [SerializeField] private Button _btnStick = null;
    [SerializeField] private Button _btnGudge = null;
    [SerializeField] private Button _btnVideo = null;
    [SerializeField] private Button _btnToken = null;
    [SerializeField] private Button _btnPassive = null;
    [SerializeField] private Button _btnActivie = null;
    [SerializeField] private Button _btnExtent = null;
    [SerializeField] private Button _btnStart = null;


    private void Awake()
    {
        startSceneManager = FindObjectOfType<StartSceneManager>();  

        _btnClose.onClick.AddListener(OnClickBack);
        _btnAvatar.onClick.AddListener(OnClickAvatar);
        _btnStick.onClick.AddListener(OnClickToken);
        _btnGudge.onClick.AddListener(OnClickGudge);
        _btnVideo.onClick.AddListener(OnClickVideo);
        _btnToken.onClick.AddListener(OnClickToken);
        _btnPassive.onClick.AddListener(OnClickToken);
        _btnActivie.onClick.AddListener(OnClickToken);
        _btnExtent.onClick.AddListener(OnClickToken);
        _btnStart.onClick.AddListener(OnClickStart);
    }
    public override bool OnKeyInput()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            OnClickBack();
        }

        return true;
    }

    private void OnClickBack()
    {
        startSceneManager._UIManager.Pop();
        startSceneManager._UIManager.OpenTitleMenu();
    }
    private async void OnClickStart()
    {
        await NetworkManager._instance.ConnectToLobby("Idol");

        await SceneLoader._instance.LoadScene(SceneName.Stage);
    }
    // Not Used
    #region Not Used
    private void OnClickAvatar()
    {
        startSceneManager._UIManager.OpenAvatarSetting();
    }
    private void OnClickGudge()
    {
        startSceneManager._UIManager.OpenGudgeSetting();
    }
    private void OnClickVideo()
    {
        startSceneManager._UIManager.OpenVideoSetting();
    }
    private void OnClickToken()
    {
        startSceneManager._UIManager.OpenTokenSetting();
    }
    private void OnClickPassive()
    {   
        startSceneManager._UIManager.OpenPassiveMenu();
    }
    private void OnClicActive()
    {
        // TitleSystem._Instance._UIManager.OpenStageMenu();
    }
    private void OnClickExtent()
    {
        // TitleSystem._Instance._UIManager.();
    }
    #endregion
}