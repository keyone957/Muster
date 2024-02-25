using TMPro;
using UnityEngine;
using UnityEngine.UI;

// Intermission 전용 UI, 설정 등을 할 수 있다.
// 최초 작성자 : 김기홍
// 수정자 : -
// 최종 수정일 : 2024-01-16
public class UIIntermissionIdol : UIWindow
{
    private StageSceneManager stageSceneManager;

    [SerializeField] Button _btnEnter = null;
    [SerializeField] TMP_Text _textRoomCode = null;
    private void Awake()
    {
        stageSceneManager = FindObjectOfType<StageSceneManager>();

        // Set Button
        _btnEnter.onClick.AddListener(OnClickEnter);

        // Set Text
        _textRoomCode.text = "RoomCode : " + SettingManager._instance.roomCode;
        _btnEnter.GetComponentInChildren<TMP_Text>().text = "공연장 들어가기";
    }

    public void OnClickEnter()
    {
        stageSceneManager.EnterStage();
    }
}
