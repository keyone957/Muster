using Fusion;
using UnityEngine;

// 무대 파티클 관리용
// 최초 작성자 : 김효중
// 수정자 : 
// 최종 수정일 : 2024-02-13
public class NetworkFlameManager : NetworkBehaviour
{
    public Transform FireCenter;
    public Transform FireLeft;
    public Transform FireRight;
    public static NetworkFlameManager __instance = null;

    ///////////////////////////////////////////////////////////
    [Rpc(RpcSources.StateAuthority, RpcTargets.InputAuthority | RpcTargets.StateAuthority)]
    public void Rpc_ActiveFlame(bool active)
        => FlameManager._instance.Local_ActiveFlame(active);

    [Rpc(RpcSources.StateAuthority, RpcTargets.InputAuthority | RpcTargets.StateAuthority)]
    public void Rpc_ActiveFireWork(bool active)
        => FlameManager._instance.Local_ActiveFireWork(active);
    [Rpc(RpcSources.StateAuthority, RpcTargets.InputAuthority | RpcTargets.StateAuthority)]
    public void Rpc_StopAll()
        => FlameManager._instance.Local_StopAll();

    ///////////////////////////////////////////////////////////

    // [Rpc(RpcSources.StateAuthority,RpcTargets.InputAuthority | RpcTargets.StateAuthority)]
    // public void Rpc_ActivateAllFire(bool activate, RpcInfo info = default)
    // {
    //     FindPoints();
    //     FlameManager.__instance.ActiveCenterFire(activate);
    //     FlameManager.__instance.ActiveLeftFire(activate);
    //     FlameManager.__instance.ActiveRightFire(activate);
    // }
    // [Rpc(RpcSources.StateAuthority,RpcTargets.InputAuthority | RpcTargets.StateAuthority)]
    // public void Rpc_ActivateAllFireWork(bool activate)
    // {
    //     FindPoints();
    //     FlameManager.__instance.ActiveCenterFireWork(activate);
    //     FlameManager.__instance.ActiveLeftFireWork(activate);
    //     FlameManager.__instance.ActiveRightFireWork(activate);
    // }


    void Start()
    {
        FindPoints();
    }
    private void FindPoints()
    {
        if(FireCenter == null){FireCenter = GameObject.Find("FireCenter").transform;}
        if(FireLeft == null){FireLeft = GameObject.Find("FireLeft").transform;}
        if(FireRight == null){FireRight = GameObject.Find("FireRight").transform;}
    }
    void ActivateAllParticles(Transform parentObcet,bool activate)
    {
        // 모든 자식 객체를 확인하며 파티클을 활성화
        foreach (Transform child in parentObcet)
        {
            ParticleSystem particleSystem = child.GetComponent<ParticleSystem>();
            if (particleSystem != null)
            {
                if(activate) particleSystem.Play(); // 파티클을 재생
                else particleSystem.Stop(); // 파티클 정지
            }
        }
    }
}
