using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using DG.Tweening.Core.Easing;
using Unity.VisualScripting;
using TMPro;
public class UIActiveMenu : UIWindow
{
    public static UIActiveMenu instrance { get; private set; } = null;
    [SerializeField] private Button _btnStart;
    [SerializeField] private Button _btnBack;
    [SerializeField] private Button _btnAvarta;
    [SerializeField] private Button _btnStick;
    [SerializeField] private Button _btnPlayer;
    [SerializeField] private Button _btnStage;
    [SerializeField] private Button _btnVideo;
    [SerializeField] private Button _btnStop;
    [SerializeField] private Button _btnMessage;
    [SerializeField] private Button _btnSetting;
    [SerializeField] private RawImage _preview;
    [SerializeField] public TMP_InputField _inputHeight;
    public string VideoName = "";
    public float userHeight;
    private StageSceneManager stageSceneManager;
    private bool set = false;
    private bool setHeight = false;
    private RenderTexture renderTexture;
    private GameObject targetCamera;
    // Start is called before the first frame update
    void Awake()
    {
        if (instrance == null)
        {
            instrance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }
    void Start()
    {
        _btnMessage.onClick.AddListener(delegate { _btnMessage.gameObject.SetActive(false); });
        _btnMessage.gameObject.SetActive(false);
        stageSceneManager = FindObjectOfType<StageSceneManager>();
        _btnStart.onClick.AddListener(StartStage);
        _btnBack.onClick.AddListener(onClickBack);
        _btnAvarta.onClick.AddListener(delegate { StageSceneUIManager._instance.OpenAvartarSetting(); });
        _btnStick.onClick.AddListener(delegate { StageSceneUIManager._instance.OpenStickSetting(); });
        _btnPlayer.onClick.AddListener(delegate { StageSceneUIManager._instance.OpenPlayerSetting(); });
        _btnStage.onClick.AddListener(delegate { StageSceneUIManager._instance.OpenStageSetting(); });
        _btnVideo.onClick.AddListener(delegate { StageSceneUIManager._instance.OpenVideoSetting(); });
        _btnSetting.onClick.AddListener(delegate { StageSceneUIManager._instance.OpenSoundSetting(); });
        _btnStop.onClick.AddListener(StopStage);
        _btnStart.gameObject.SetActive(true);
        _btnStop.gameObject.SetActive(false);
        renderTexture = new RenderTexture(Screen.width, Screen.height, 24);
        if (targetCamera == null) { targetCamera = GameObject.Find("AvaCamera"); }
        if (targetCamera != null)
        {
            targetCamera.GetComponent<Camera>().enabled = true;
            targetCamera.GetComponent<Camera>().targetTexture = renderTexture; // 카메라가 렌더링한 결과를 이 RenderTexture로 지정
            _preview.texture = renderTexture;
        }
    }
    public async void onClickBack()
    {
        await SceneLoader._instance.LoadScene(SceneName.Start);
    }
    public void onSet()
    {
        set = true;
    }
    private bool onSetHeihgt()
    {
        if (string.IsNullOrEmpty(_inputHeight.text))
        {
            return false;
        }
        else 
        {
            userHeight = float.Parse(_inputHeight.text) / 100.0f;
            return true;
        }
    }
    public void StartStage()
    {
        if (!set||onSetHeihgt()==false)
        {
            _btnMessage.gameObject.SetActive(true);
            return;
        }
        stageSceneManager.EnterStage();
    }
    public void StopStage()
    {
        stageSceneManager.EnterIntermission();
    }
    public void SetUIOff()
    {
        // ���� ȿ���� �־�������
        _btnStop.gameObject.SetActive(false);
        _btnStart.gameObject.SetActive(true);
    }
    public void SetUIOn()
    {
        // ���� ȿ���� �־�������
        _btnStop.gameObject.SetActive(true);
        _btnStart.gameObject.SetActive(false);
    }
}
