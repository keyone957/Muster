using UniRx;
using UnityEngine;

// 공연장의 스크린 UI 관리용, StageSceneUIManager와 헷갈리지 않도록 주의
// 최초 작성자 : 김효중
// 수정자 : 김기홍
// 최종 수정일 : 20204-02-08
// 최종 수정 내용
//  - 네트워크 기능 분할 (NetworkStageScreenUI 생성)
public class StageScreenUIManager : UIManager
{
    public static StageScreenUIManager Instance { get; private set; }
    private NetworkStageScreenUIManager networkedManager;

    [SerializeField] private UIStageScreen _mainScreen = null;
    [SerializeField] private UIStageScreen _leftScreen = null;
    [SerializeField] private UIStageScreen _rightScreen = null;

    private void Awake()
    {
        if (Instance == null)
            Instance = this;
        else
            Destroy(this.gameObject);
    }
    //////////////////////////////////////////
    // Server에서 호출하는 함수
    #region SERVER FUNCTION
    /// <summary> Server(=Idol)만 호출가능한 함수 </summary>
    public void Server_ActiveMainScreen(bool activate)
    {
        if(networkedManager == null)
            networkedManager = FindObjectOfType<NetworkStageScreenUIManager>();
        networkedManager.Rpc_ActiveMainScreen(activate);
    }
    /// <summary> Server(=Idol)만 호출가능한 함수, 관객 기준 왼쪽 화면 </summary>
    public void Server_ActiveLeftScreen(bool activate)
    {
        if (networkedManager == null)
            networkedManager = FindObjectOfType<NetworkStageScreenUIManager>();
        networkedManager.Rpc_ActiveLeftScreen(activate);
    }
    /// <summary> Server(=Idol)만 호출가능한 함수, 관객 기준 오른쪽 화면 </summary>
    public void Server_ActiveRightScreen(bool activate)
    {
        if (networkedManager == null)
            networkedManager = FindObjectOfType<NetworkStageScreenUIManager>();
        networkedManager.Rpc_ActiveRightScreen(activate);
    }
    /// <summary> Server(=Idol)만 호출가능한 함수 </summary>
    public void Server_SetMainScreenText<T>(T midText)
    {
        if (networkedManager == null)
            networkedManager = FindObjectOfType<NetworkStageScreenUIManager>();
        networkedManager.Rpc_SetMainScreenText(midText.ToString());
    }
    /// <summary> Server(=Idol)만 호출가능한 함수 </summary>
    public void Server_SetMainScreenText<T>(T midText, T subText)
    {
        if (networkedManager == null)
            networkedManager = FindObjectOfType<NetworkStageScreenUIManager>();
        networkedManager.Rpc_SetMainScreenText(midText.ToString(), subText.ToString());
    }
    /// <summary> Server(=Idol)만 호출가능한 함수, 관객 기준 왼쪽 화면 </summary>
    public void Server_SetLeftScreenText<T>(T midText)
    {
        if (networkedManager == null)
            networkedManager = FindObjectOfType<NetworkStageScreenUIManager>();
        networkedManager.Rpc_SetLeftScreenText(midText.ToString());
    }
    /// <summary> Server(=Idol)만 호출가능한 함수, 관객 기준 왼쪽 화면 </summary>
    public void Server_SetLeftScreenText<T>(T midText, T subText)
    {
        if (networkedManager == null)
            networkedManager = FindObjectOfType<NetworkStageScreenUIManager>();
        networkedManager.Rpc_SetLeftScreenText(midText.ToString(), subText.ToString());
    }
    /// <summary> Server(=Idol)만 호출가능한 함수, 관객 기준 오른쪽 화면 </summary>
    public void Server_SetRightScreenText<T>(T midText)
    {
        if (networkedManager == null)
            networkedManager = FindObjectOfType<NetworkStageScreenUIManager>();
        networkedManager.Rpc_SetRightScreenText(midText.ToString());
    }
    /// <summary> Server(=Idol)만 호출가능한 함수, 관객 기준 오른쪽 화면 </summary>
    public void Server_SetRightScreenText<T>(T midText, T subText)
    {
        networkedManager.Rpc_SetRightScreenText(midText.ToString(), subText.ToString());
    }
    /// <summary> Server(=Idol)만 호출가능한 함수 </summary>
    public void Server_ActiveSlider(bool activate)
    {
        networkedManager.Rpc_ActivateSlider(activate);
    }
    /// <summary> Server(=Idol)만 호출가능한 함수 </summary>
    public void Server_SetSlider(float now, float max)
    {
        networkedManager  .Rpc_SetSlider(now, max);
    }
    #endregion
    ////////////////////////////////////////
    // Local에서 호출하는 함수
    #region LOCAL FUNCTION
    public void Local_ActiveMainScreen(bool activate) => _mainScreen.GetComponentInChildren<Canvas>().enabled = activate;
    public void Local_ActiveLeftScreen(bool activate) => _leftScreen.GetComponentInChildren<Canvas>().enabled = activate;
    public void Local_ActiveRightScreen(bool activate) => _rightScreen.GetComponentInChildren<Canvas>().enabled = activate;
    public void Local_SetMainScreenText<T>(T midText) => _mainScreen.SetMidText(midText.ToString());
    public void Local_SetMainScreenText<T>(T midText, T subText)
    {
        _mainScreen.SetMidText(midText.ToString());
        _mainScreen.SetSubText(subText.ToString());
    }
    /// <summary> 관객 기준 왼쪽 화면 </summary>
    public void Local_SetLeftScreenText<T>(T midText)
    {
        _leftScreen.SetMidText(midText.ToString());
        _leftScreen.SetSubText("");
    }
    /// <summary> 관객 기준 왼쪽 화면 </summary>
    public void Local_SetLeftScreenText<T>(T midText, T subText)
    {
        _leftScreen.SetMidText(midText.ToString());
        _leftScreen.SetSubText(subText.ToString());
    }
    /// <summary> 관객 기준 왼쪽 화면 </summary>
    public void Local_SetLeftScreenSubText<T>(T subText)
    {
        _leftScreen.SetSubText(subText.ToString());
    }
    /// <summary> 관객 기준 오른쪽 화면 </summary>
    public void Local_SetRightScreenText<T>(T midText)
    {
        _rightScreen.SetMidText(midText.ToString());
        _rightScreen.SetSubText("");
    }
    /// <summary> 관객 기준 오른쪽 화면 </summary>
    public void Local_SetRightScreenText<T>(T midText, T subText)
    {
        _rightScreen.SetMidText(midText.ToString());
        _rightScreen.SetSubText(subText.ToString());
    }
    /// <summary> 관객 기준 오른쪽 화면 </summary>
    public void Local_SetRightScreenSubText<T>(T subText)
    {
        _rightScreen.SetSubText(subText.ToString());
    }
    public void Local_SetSlider(float now, float max)
    {
        _mainScreen.SetSlider(now, max);
    }
    public void Local_ActivateSlider(bool activate) => _mainScreen.ActivateSlider(activate);
    #endregion
}