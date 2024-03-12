using TMPro;
using UnityEngine;
using UnityEngine.UI;

// Intermission 전용 UI, 설정 등을 할 수 있다. (Audience)
// 최초 작성자 : 김기홍
// 수정자 : -
// 최종 수정일 : 2024-01-24
public class UIIntermissionUser : UIWindow
{
    private StageSceneManager stageSceneManager;
    
    [SerializeField] Button _btnEnter = null;
    [SerializeField] Button _btnExit = null;
    [SerializeField] Button _btnSetting = null;
    private void Awake()
    {
        stageSceneManager = FindObjectOfType<StageSceneManager>();

        // Set Button
        _btnEnter.onClick.AddListener(OnClickEnterStage);
        _btnExit.onClick.AddListener(OnClickEnterItermisson);
        _btnSetting.onClick.AddListener(delegate{StageSceneUIManager._instance.OpenSoundSetting();});
    }

    public void OnClickEnterStage()
    {
        _btnEnter.interactable = false;
        // 초기설정이 완료되면 버튼 활성화 (로딩시간이 필요함)
        stageSceneManager.EnterStage();
        _btnEnter.interactable = true;
    }
    public async void OnClickEnterItermisson()
    {
        _btnExit.interactable = false;
        NetworkManager._instance.DisconnectToSession();
        await SceneLoader._instance.LoadScene(SceneName.Start);
    }
}
