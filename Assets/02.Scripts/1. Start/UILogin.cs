using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;
using TMPro;
//using Firebase.Auth;
using System;


// 로그인창 관리용
// 최초 작성자 : 김효중
// 수정자 : 김기홍
// 최종 수정일 : 2024-01-15
public class UILongin : UIWindow
{
    [SerializeField] PlayerManager playerManager;
    private StartSceneManager startSceneManager;

    [SerializeField] Button _btnClose = null;
    [SerializeField] Button _btnYoutube = null;
    [SerializeField] Button _btnTwitch = null;
    [SerializeField] Button _btnEmail = null;
    [SerializeField] Button _btnGetKey = null;
    [SerializeField] Button _btnCreate = null;
    [SerializeField] Button _btnLogin = null;

    [SerializeField] GameObject _buttons;
    [SerializeField] GameObject _login;
    [SerializeField] GameObject _keys;

    [SerializeField] TMP_InputField email;
    [SerializeField] TMP_InputField password;
    [SerializeField] TMP_InputField idolKey;
    [SerializeField] TMP_InputField staffKey;
    [SerializeField] TextMeshProUGUI _errorMessag;
    //private FirebaseAuth auth; // 로그인, 회원가입 정보
    //private FirebaseUser user; // 유저 정보
    private bool login = false;

    private void Awake()
    {
        _btnClose.onClick.AddListener(OnClickClose);
        _btnYoutube.onClick.AddListener(OpenLogin);
        _btnTwitch.onClick.AddListener(OpenLogin);
        _btnEmail.onClick.AddListener(OpenLogin);
        _btnGetKey.onClick.AddListener(OpenKey);
        _btnCreate.onClick.AddListener(Create);
        _btnLogin.onClick.AddListener(Login);
        _buttons.SetActive(true);
        _login.SetActive(false);
        _keys.SetActive(false);

        playerManager = FindObjectOfType<PlayerManager>();
        startSceneManager = FindObjectOfType<StartSceneManager>();
    }
    // Start is called before the first frame update
    void Start()
    {
        this.login = false;
        //auth = FirebaseAuth.DefaultInstance;
    }
    public void Logout()
    {
        //auth.SignOut();
        Debug.Log("로그아웃");
    }
    public void OnClickClose()
    {
        startSceneManager._UIManager.CloseLogin();
        if (playerManager.GetRole() == PlayerManager.Role.Audience)
        {
            startSceneManager._UIManager.OpenTitleMenu();
        }
        if (playerManager.GetRole() == PlayerManager.Role.Idol)
        {
            startSceneManager._UIManager.OpenIdolMenu();
        }
    }
    public void onClickTwitch()
    {
        string clientId = "v4hiwe4shkcjrj2qyd6snidkcbmd7r";
        //fzb1swtnnyhinsonri8ssc4ql4wn72
        string redirectUri = "http://localhost/";
        string authorizeUrl = "https://id.twitch.tv/oauth2/authorize";

        string authUrl = $"{authorizeUrl}?client_id={clientId}&redirect_uri={redirectUri}&response_type=token&scope=openid";
        Application.OpenURL(authUrl);

    }
    public void Create()
    {
        //auth.CreateUserWithEmailAndPasswordAsync(email.text, password.text).ContinueWith(task =>
        //{
        //    ShowError("");
        //    // 가입 취소
        //    if (task.IsCanceled)
        //    {
        //        Debug.Log("가입 취소");
        //        CloseKey();
        //        return;
        //    }
        //    // 가입 실패 : 이메일 비정상, 비밀번호 문제 등등
        //    if (task.IsFaulted)
        //    {
        //        ShowError("회원가입실패");
        //        CloseKey();
        //        return;
        //    }
        //    if (task.IsCompleted)
        //    {
        //        // 취소, 오류 없이 진행
        //        //AuthResult authResult = task.Result;
        //        //FirebaseUser newUser = authResult.User;
        //        Debug.Log("회원가입완료");
        //    }
        //    else
        //    {
        //        ShowError("회원가입실패");
        //    }
        //});
    }
    public void Login()
    {
        //auth.SignInWithEmailAndPasswordAsync(email.text, password.text).ContinueWith(task =>
        //{
        //    ShowError("");
        //    // 가입 취소
        //    if (task.IsCanceled)
        //    {
        //        Debug.Log("로그인 취소");
        //        ShowError("로그인 취소");
        //        CloseKey();
        //        return;
        //    }
        //    // 가입 실패 : 이메일 비정상, 비밀번호 문제 등등
        //    if (task.IsFaulted)
        //    {
        //        ShowError("로그인 실패");
        //        CloseKey();
        //        return;
        //    }
        //    if (task.IsCompleted)
        //    {
        //        // 취소, 오류 없이 진행
        //        //AuthResult authResult = task.Result;
        //        //FirebaseUser newUser = authResult.User;
        //        Debug.Log("로그인 완료");
        //        SettingManager._instance.role = PlayerManager.Role.Idol;
        //        this.login = true;
        //        OpenKey();
        //    }

        //});
    }
    public void ShowError(String msg)
    {
        _errorMessag.text = msg;
    }
    public void OpenLogin()
    {
        _buttons.SetActive(false);
        _login.SetActive(true);
    }
    public void OpenKey()
    {
        if (this.login)
        {
            Debug.Log("키 생성");
            _keys.SetActive(true);
            idolKey.text = "bWFuZHVJZG9s";
            staffKey.text = "U3RhZmZNYW5kdQ==";
        }

    }
    public void CloseKey()
    {

        _keys.SetActive(false);
        idolKey.text = "";
        staffKey.text = "";
    }

}