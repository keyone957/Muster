using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UIAccSetting : UIWindow
{
    [SerializeField]
    private Button _btnClose = null;
    [SerializeField]
    private Button _btnStick = null;
    [SerializeField]
    private Button _btnBadge = null;
    private void Awake()
    {
        _btnClose.onClick.AddListener(OnClickBack);
        _btnStick.onClick.AddListener(OnClickStick);
        _btnBadge.onClick.AddListener(OnClickBadge);
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
        //TitleSystem._Instance._UIManager.CloseAccSetting();
    }
    private void OnClickStick()
    {
        //TitleSystem._Instance._UIManager.OpenStickList();
        //TitleSystem._Instance._UIManager.CloseAccSetting();
    }
    private void OnClickBadge()
    {
    }
}