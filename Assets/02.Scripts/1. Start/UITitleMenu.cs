using UnityEngine;
using UnityEngine.UI;

public class UITitleMenu : UIWindow
{
    private StartSceneManager startSceneManager;

    [SerializeField] private Button _btnMakeConcert = null;
    [SerializeField] private Button _btnEnterConcert = null;
    [SerializeField] private Button _btnSetting = null;
    [SerializeField] private Button _btnExit = null;

    private void Awake()
    {
        _btnMakeConcert.onClick.AddListener(OnClickMakeConcert);
        _btnEnterConcert.onClick.AddListener(OnClickEnterConcert);
        _btnSetting.onClick.AddListener(OnClickUserSetting);
        _btnExit.onClick.AddListener(OnClickExit);
    }

    private void OnClickMakeConcert()
    {
        PlaySelectSound();
        //TitleSystem._Instance._UIManager.CloseTitleMenu();
        //TitleSystem._Instance._UIManager.OpenConcertSetting();
    }

    private void OnClickEnterConcert()
    {
        PlaySelectSound();
        //TitleSystem._Instance._UIManager.OpenConcertList();
    }
    private void OnClickUserSetting()
    {
        PlaySelectSound();
        //TitleSystem._Instance._UIManager.CloseTitleMenu();
        //TitleSystem._Instance._UIManager.OpenUeserSetting();
    }

    private void OnClickExit()
    {
        PlaySelectSound();
        startSceneManager.DoExit();
    }

    private void PlaySelectSound()
    {
        //SoundManager._Instance.PlaySound(Define._menuSelectSound);
    }
}