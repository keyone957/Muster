using Fusion;
using UnityEngine;

// 무대 파티클 관리용
// 최초 작성자 : 김효중
// 수정자 : 
// 최종 수정일 : 2024-02-13
public class FlameManager : MonoBehaviour
{
    public Transform FireCenter;
    public Transform FireLeft;
    public Transform FireRight;
    public Transform FWorkCenter;
    public Transform FWorkLeft;
    public Transform FWorkRight;
    public static FlameManager _instance = null;
    private NetworkFlameManager networkFlameManager = null;
    private void Awake()
    {
        if (_instance == null)
        {
            _instance = this;
        }
        else
        {
            Destroy(this);
        }
    }
    private void Start()
    {
        FindPoints();
        networkFlameManager = FindObjectOfType<NetworkFlameManager>();
    }
    //////////////////////////////////////////
    // Server에서 호출하는 함수
    #region SERVER FUNCTION
    /// <summary> Server(=Idol)만 호출가능한 함수 </summary>
    public void Server_ActiveFlame(bool active)
    {
        if (networkFlameManager == null)
            networkFlameManager = FindObjectOfType<NetworkFlameManager>();
        networkFlameManager.Rpc_ActiveFlame(active);
    }
    /// <summary> Server(=Idol)만 호출가능한 함수 </summary>
    public void Server_ActiveFirework(bool active, float duration = 0.2f)
    {
        if (networkFlameManager == null)
            networkFlameManager = FindObjectOfType<NetworkFlameManager>();
        networkFlameManager.Rpc_ActiveFireWork(active);
    }
    public void Server_StopAll()
    {
        if (networkFlameManager == null)
            networkFlameManager = FindObjectOfType<NetworkFlameManager>();
        networkFlameManager.Rpc_StopAll();
    }
    #endregion

    /// <summary>
    //////////////////////////////////////////
    // Local에서 호출하는 함수
    #region LOCAL FUNCTION
    public void Local_ActiveFlame(bool active)
    {
        ActiveCenterFire(active);
        ActiveLeftFire(active);
        ActiveRightFire(active);

    }
    public void Local_ActiveFireWork(bool active)
    {
        ActiveCenterFireWork(active);
        ActiveLeftFireWork(active);
        ActiveRightFireWork(active);

    }
    public void Local_StopAll()
    {
        Local_ActiveFireWork(false);
        Local_ActiveFlame(false);
    }
    #endregion
    /// ////////////////////////////

    // public void ActivateAllFire(bool activate)
    // {
    //     Debug.Log("call");
    //     if (networkFlameManager == null)
    //     { networkFlameManager = FindObjectOfType<NetworkFlameManager>(); }
    //     if (networkFlameManager == null) Debug.Log("NOT FOUND");
    //     else Debug.Log(networkFlameManager.name);
    //     networkFlameManager.Rpc_ActivateAllFire(activate);
    // }
    // public void ActivateAllFireWork(bool activate)
    // {
    //     ActiveCenterFireWork(activate);
    //     ActiveLeftFireWork(activate);
    //     ActiveRightFireWork(activate);
    //     if (networkFlameManager == null)
    //         networkFlameManager = FindObjectOfType<NetworkFlameManager>();
    //     networkFlameManager.Rpc_ActivateAllFireWork(activate);
    // }
    public void ActiveCenterFire(bool activate) { ActivateAllParticles(FireCenter, activate); }
    public void ActiveLeftFire(bool activate) { ActivateAllParticles(FireLeft, activate); }
    public void ActiveRightFire(bool activate) { ActivateAllParticles(FireRight, activate); }
    public void ActiveCenterFireWork(bool activate)
    {
        if (activate) FWorkCenter.GetComponent<ParticleSystem>().Play(); // 파티클을 재생
        else FWorkCenter.GetComponent<ParticleSystem>().Stop();
    }
    public void ActiveLeftFireWork(bool activate)
    {
        if (activate) FWorkLeft.GetComponent<ParticleSystem>().Play(); // 파티클을 재생
        else FWorkLeft.GetComponent<ParticleSystem>().Stop();
    }
    public void ActiveRightFireWork(bool activate)
    {
        if (activate) FWorkRight.GetComponent<ParticleSystem>().Play(); // 파티클을 재생
        else FWorkRight.GetComponent<ParticleSystem>().Stop();
    }


    private void FindPoints()
    {
        if (FireCenter == null) { FireCenter = GameObject.Find("FireCenter").transform; }
        if (FireLeft == null) { FireLeft = GameObject.Find("FireLeft").transform; }
        if (FireRight == null) { FireRight = GameObject.Find("FireRight").transform; }
        if (FWorkCenter == null) { FWorkCenter = GameObject.Find("FWorkCenter").transform; }
        if (FWorkLeft == null) { FWorkLeft = GameObject.Find("FWorkLeft").transform; }
        if (FWorkRight == null) { FWorkRight = GameObject.Find("FWorkRight").transform; }
    }
    void ActivateAllParticles(Transform parentObcet, bool activate)
    {
        // 모든 자식 객체를 확인하며 파티클을 활성화
        foreach (Transform child in parentObcet)
        {
            ParticleSystem particleSystem = child.GetComponent<ParticleSystem>();
            if (particleSystem != null)
            {
                if (activate) particleSystem.Play(); // 파티클을 재생
                else particleSystem.Stop(); // 파티클 정지
            }
        }
    }
}
