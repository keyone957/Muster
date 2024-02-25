using UnityEngine;
using Fusion;

// 공연 VFX 관리용
// 최초 작성자 : 김효중
// 수정자 : 김기홍
// 최종 수정일 : 20204-02-04
// 최종 수정 내용
//  - 네트워크상에서 작동하도록 수정
//  - Server만 사용할 수 있도록 함수 수정
public class StageVFXManager : MonoBehaviour
{
    // Singleton
    public static StageVFXManager instance = null;

    private void Awake()
    {
        #region Singleton
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(this.gameObject);
        }
        #endregion
    }

    //////////////////////////////////////////
    // Server에서 호출하는 함수
    #region SERVER FUNCTION
    /// <summary> Server(=Idol)만 호출가능한 함수 </summary>
    public void Server_ActiveFire(bool activate)
    {
        if (NetworkManager._instance._runner.IsServer == false)
            return;
        Rpc_ActiveFire(activate);
    }
    #endregion

    //////////////////////////////////////////
    // Rpc에서 호출하는 함수
    #region RPC FUNCTION
    /// <summary> Server(=Idol)만 호출가능한 함수 </summary>
    [Rpc(RpcSources.InputAuthority,RpcTargets.InputAuthority)]
    private void Rpc_ActiveFire(bool activate, RpcInfo info = default) => Local_ActiveFire(activate);
    #endregion

    //////////////////////////////////////////
    // Local에서 호출하는 함수
    #region LOCAL FUNCTION
    /// <summary> Server(=Idol)만 호출가능한 함수 </summary>
    public void Local_ActiveFire(bool active)
    {
        Debug.Log(active ? "Fire On" : "Fire Off");
    }
    #endregion
}
