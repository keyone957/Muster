using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UIStageSetting : UIWindow
{
    [SerializeField] private Button _btnClose = null;
    [SerializeField] private Button _btnOK = null;
    [SerializeField] private TextMeshProUGUI _label = null;

    private void Awake()
    {
        _btnClose.onClick.AddListener(OnClickBack);
        _btnOK.onClick.AddListener(OnClickOk);
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
        //TitleSystem._Instance._UIManager.CloseStagetSetting();
    }
     private void OnClickOk()
    {
        //TODO: 무대 설정 적용
        //TitleSystem._Instance._UIManager.CloseStagetSetting();
    }
    public void ValueChanged(int val)
    {
        _label.text = val.ToString();
    }
}