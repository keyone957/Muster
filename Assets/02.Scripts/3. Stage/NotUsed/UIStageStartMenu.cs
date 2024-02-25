using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.Management;
//테스트위한 Initialize 비활
//최초 작성자: 홍원기
//수정자:
//최종 수정일: 2023-12-04
public class UIStageStartMenu : UIManager
{
    [SerializeField] PlayerManager playerManager;
    public UIIdolPanel _idolPanel;
    public UIAudiencePanel _audiencePaenl;
    public GameObject _freeCamObj;
    public static UIStageStartMenu _Instance { get; private set; }
    [SerializeField]
    public GameObject xrRig;
    private void Awake()
    {
        _Instance = this;
    }
    private void Start()
    {
        //Initialize();
    }
    private void OnDestroy()
    {
        Clear();
    }
    private void Clear()
    {
        _Instance = null;
    }
    //각 플레이어의 Role 값에 따라서 다른 UI를 생성
    //ui뒤에 보여지는 화면을 다르게 하기위함
    //2d ui를 사용하기 위해 xrRig 비활성화
    public void Initialize()
    {
        xrRig.SetActive(false);
        StageCameraManager.instance.SetMainCamera();

        if (playerManager.GetRole() == PlayerManager.Role.Idol)
        {
            OpenStageIdolMenu();
            _freeCamObj = Instantiate(Resources.Load(Define._freeCamPrefab), new Vector3(0, 3.47f, 14.23f), Quaternion.Euler(0, -180, 0), transform.parent)as GameObject;
            _freeCamObj.SetActive(false);
        }
        else if (playerManager.GetRole() == PlayerManager.Role.Audience)
        {
            OpenAudienceMenu();
        }
    }
    private void OpenStageIdolMenu()
    {
        if (_idolPanel == null)
        {
            _idolPanel = OpenWindow(Define._uiIdolPanelpPrefabPath) as UIIdolPanel;
        }
    }
    private void CloseStageIdolMenu()
    {
        if (_idolPanel != null)
        {
            CloseWindow(_idolPanel);
            _idolPanel = null;
        }
    }

    private void OpenAudienceMenu()
    {
        if (_audiencePaenl == null)
        {
            _audiencePaenl = OpenWindow(Define._uiAudiencePrefabPath) as UIAudiencePanel;
        }
    }
}
