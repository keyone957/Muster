using UnityEngine;
using UnityEngine.UI;

// 역할 선택 UI
// 최초 작성자 : 김효중
// 수정자 : 김기홍
// 최종 수정일 : 2024-01-16

public class UITitleSelect : UIWindow
{
    private StartSceneManager startSceneManager;

    [SerializeField] private Button _btnIdol = null;
    [SerializeField] private Button _btnStaff = null;
    [SerializeField] private Button _btnPlayer = null;
    [SerializeField] private Button _btnExit = null;

    private void Awake()
    {
        startSceneManager = FindObjectOfType<StartSceneManager>();

        _btnIdol.onClick.AddListener(OnClickIdol);
        _btnStaff.onClick.AddListener(OnClickStaff);
        _btnPlayer.onClick.AddListener(OnClickUser);
        _btnExit.onClick.AddListener(OnClickExit);
    }

    private async void OnClickIdol()
    {
        SettingManager._instance.role = PlayerManager.Role.Idol;
        startSceneManager._UIManager.CloseTitleMenu();
        // TitleSystem._Instance._UIManager.OpenLogin();
        // startSceneManager._UIManager.OpenIdolMenu();

        // FlowChart 수정에 따른 코드 수정
        // - UITitleIdolMenu에서 OnClickStart() 함수의 코드를 가져옴
        // - UIStageIdolEnter에서 Start 함수의 Room(Session)생성 코드를 가져옴
        await NetworkManager._instance.ConnectToLobby("Idol");
        SettingManager._instance.roomCode = StringUtils.GeneratePassword(8);
        await SceneLoader._instance.LoadScene(SceneName.Stage);
        await NetworkManager._instance.CreateSession(SettingManager._instance.roomCode);
        StageSceneManager.instance._UIManager.OpenActiveMenu(); // 아이돌 입장
    }

    private async void OnClickUser()
    {
        SettingManager._instance.role = PlayerManager.Role.Audience;
        startSceneManager._UIManager.CloseTitleMenu();
        // startSceneManager._UIManager.OpenUserMenu();

        // FlowChart 수정에 따른 코드 수정
        // - UITitleUserMenu의 OnClickEnter()함수의 코드를 가져옴
        var rnd = Random.Range(0, 1000);
        await NetworkManager._instance.ConnectToLobby("User" + rnd);
        await SceneLoader._instance.LoadScene(SceneName.Stage);
    }

    private void OnClickExit()
    {
        startSceneManager.DoExit();
    }
    // Not Used
    private void OnClickStaff()
    {
        startSceneManager._UIManager.CloseTitleMenu();
        startSceneManager._UIManager.OpenStaffMenu();
    }
}