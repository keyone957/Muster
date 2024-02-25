using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;
public class UITitleStaffMenu : UIWindow
{
    private StartSceneManager startSceneManager;

    [SerializeField] Button _btnClose = null;
    [SerializeField] Button _btnEnter = null;
    [SerializeField] TextMeshProUGUI Error = null;
    [SerializeField] TMP_InputField ConcertID = null;
    [SerializeField] TMP_InputField ManageID = null;

    private void Awake()
    {
        startSceneManager = FindObjectOfType<StartSceneManager>();

        _btnClose.onClick.AddListener(OnClickBack);
        _btnEnter.onClick.AddListener(OnClickEnter);
        _btnEnter.interactable = false;
        ConcertID.onValueChanged.AddListener(HandleInputValueChanged);
        ManageID.onValueChanged.AddListener(HandleInputValueChanged);
    }
    public override bool OnKeyInput()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            OnClickBack();
        }
        return true;
    }
    private void HandleInputValueChanged(string text)
    {
        if(ConcertID.text != "" && ManageID.text != "")
        {
            _btnEnter.interactable = true;
        }
        Error.text = "";
    }
    private void OnClickBack()
    {
        startSceneManager._UIManager.Pop();
        //TitleSystem._Instance._UIManager.CloseStaffMenu();
        startSceneManager._UIManager.OpenTitleMenu();
    }
    public void OnClickEnter()
    {
        if(ConcertID.text == "bWFuZHVJZG9s" && ManageID.text == "U3RhZmZNYW5kdQ==")
        {
            SettingManager._instance.role = PlayerManager.Role.Staff;
            SceneManager.LoadScene("StageScene");
        }
        else
        {
            Error.text = "입장 실패";
        }
    }
}