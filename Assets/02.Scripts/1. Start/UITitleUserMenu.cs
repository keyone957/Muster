using UnityEngine;
using UnityEngine.UI;
using TMPro;

// 유저 입장전 UI 관리용
// 최초 작성자 : 김효중
// 수정자 : 김기홍
// 최종 수정일 : 2024-01-15
public class UITitleUserMenu : UIWindow
{
    private StartSceneManager startSceneManager;

    [SerializeField] Button _btnClose = null;
    [SerializeField] Button _btnEnter = null;
    [SerializeField] Button _btnWarning = null;
    [SerializeField] TMP_InputField inputSession = null;
    private void Awake()
    {
        _btnClose.onClick.AddListener(OnClickBack);
        _btnEnter.onClick.AddListener(OnClickEnter);
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
    public async void OnClickEnter()
    {
        var rnd = Random.Range(0, 1000);

        await NetworkManager._instance.ConnectToLobby("User" + rnd);

        await SceneLoader._instance.LoadScene(SceneName.Stage);
    }
}