using UnityEngine;
using UnityEngine.UI;
using TMPro;

// 토큰설정용
// 최초 작성자 : 김효중
// 수정자 : 김기홍
// 최종 수정일 : 2024-01-15
public class UITokenSetting : UIWindow
{
    private StartSceneManager startSceneManager;

    [SerializeField] private Button _btnClose = null;
    [SerializeField] private Button _btnOK = null;
    [SerializeField] private Button _btnNone = null;

    [SerializeField] TMP_InputField _token;
    private void Awake()
    {
        _btnClose.onClick.AddListener(startSceneManager._UIManager.Pop);
        _btnOK.onClick.AddListener(onClickOk);
        _btnNone.onClick.AddListener(onClickNone);
    }
    private void onClickOk()
    {
        // SettingManager._instance._token = _token.text;
    }
    private void onClickNone()
    {
        _token.text = "";
        // SettingManager._instance._token = "";
    }
    

}