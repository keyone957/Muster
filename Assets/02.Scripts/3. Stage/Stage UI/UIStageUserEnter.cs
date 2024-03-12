using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Collections.Generic;
using System;

// 유저가 Room에 접근하기 위한 UI
// 최초 작성자 : 김기홍
// 수정자 : -
// 최종 수정일 : 2024-01-16
public class UIStageUserEnter : UIWindow
{
    private StageSceneManager stageSceneManager;

    [SerializeField] Button _btnClose = null;
    [SerializeField] Button _btnEnter = null;
    [SerializeField] Button _btnWarning = null;
    [SerializeField] TMP_InputField _inputSession = null;

    // Debug + ShowCase
    [SerializeField] TMP_Dropdown _dropdownRoomList = null;
    string[] sessionNames;
    Tuple<int, int>[] sessionCount;


    private void Awake()
    {
        stageSceneManager = FindObjectOfType<StageSceneManager>();

        _btnClose.onClick.AddListener(OnClickBack);
        _btnEnter.onClick.AddListener(OnClickEnter);
        _btnWarning.onClick.AddListener(CloseWarning);
    }
    private void Start()
    {
        Refresh();
    }
    public void Refresh()
    {
        _dropdownRoomList.options.Clear();
        sessionNames = new string[NetworkManager._instance.Sessions.Count];
        sessionCount = new Tuple<int, int>[NetworkManager._instance.Sessions.Count];

        int i = 0;
        foreach (var session in NetworkManager._instance.Sessions)
        {
            TMP_Dropdown.OptionData optionData = new TMP_Dropdown.OptionData();
            optionData.text = $"{session.Name} ({session.PlayerCount}/{session.MaxPlayers})";
            
            _dropdownRoomList.options.Add(optionData);
            sessionNames[i] = session.Name;
            sessionCount[i] = new Tuple<int, int>(session.PlayerCount, session.MaxPlayers);
        }
        _dropdownRoomList.RefreshShownValue();
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
        NetworkManager._instance.DisconnectToLobby();
        await SceneLoader._instance.LoadScene(SceneName.Start);
    }
    public async void OnClickEnter()
    {
        if (_dropdownRoomList.options.Count == 0) return;
        _btnEnter.interactable = false;
#if true // Debug + ShowCase
        Debug.Log("Dropdown Value: " + _dropdownRoomList.options[_dropdownRoomList.value].text + ", List Selected: " + (_dropdownRoomList.value + 1));

        if(sessionCount[_dropdownRoomList.value].Item1 < sessionCount[_dropdownRoomList.value].Item2)
        {
            await NetworkManager._instance.ConnectToSession(sessionNames[_dropdownRoomList.value]);
            stageSceneManager.EnterSession();
        }
        else
        {
            // 방이 가득 찼다는 오류 출력
            _btnEnter.interactable = true;
        }
#endif

#if RELEASE
        string sessionCode = inputSession.text;
        if (CheckSession(sessionCode))
        {
            // Session 입장
            await NetworkManager._instance.ConnectToSession(sessionCode);
            stageSceneManager.EnterIntermission();

        }
        else if(inputSession.text.Length == 0)
        {
            ShowError("Room Code를 입력해주세요.");
        }
        else
        {
            ShowError("해당 방을 찾을 수 없습니다.");
        }
#endif
    }
    public bool CheckSession(string sessionCode)
    {
        foreach (var session in NetworkManager._instance.Sessions)
        {
            Debug.Log(session.Name);
            if (session.Name.CompareTo(sessionCode) == 0) return true;
        }
        return false;
    }
    public void CloseWarning()
    {
        _btnWarning.gameObject.SetActive(false);
    }
    public void ShowError(string msg)
    {
        _inputSession.text = "";
        _btnWarning.gameObject.SetActive(true);
        _btnWarning.GetComponentInChildren<TextMeshProUGUI>().text = "경고!\n" + msg;
    }
}