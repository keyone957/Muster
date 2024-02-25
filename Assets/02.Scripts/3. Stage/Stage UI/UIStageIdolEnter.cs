using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

// Stage(Enter, Intermission, Stage)중 Enter단계의 UI 관리
// 현재는 사용하지 않음
// 최초 작성자 : 김기홍
// 수정자 : -
// 최종 수정일 : 2024-01-26
public class UIStageIdolEnter : UIWindow
{
    [SerializeField] Button _btnClose = null;
    [SerializeField] Button _btnEnter = null;
    [SerializeField] TMP_Text _textRoomCode = null;

    private void Awake()
    {
        _btnClose.onClick.AddListener(OnClickBack);
        _btnEnter.onClick.AddListener(OnClickEnter);
    }
    private async void Start()
    {
        await NetworkManager._instance.CreateSession(SettingManager._instance.roomCode);

        SettingManager._instance.roomCode = StringUtils.GeneratePassword(8);
        _textRoomCode.text = "Room Code : " + SettingManager._instance.roomCode;
    }
    public override bool OnKeyInput()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            OnClickBack();
        }
        return true;
    }
    private async void OnClickBack()
    {
        await NetworkManager._instance.DisconnectToSession();
        await SceneLoader._instance.LoadScene(SceneName.Start);
    }
    public void OnClickEnter()
    {
        // TODO: Intermission UI 띄우기
    }
}
