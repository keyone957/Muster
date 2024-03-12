using Fusion;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Network 환경에서 LightParticle을 관리하는 Manager
// 최초 작성자 : 김기홍
// 수정자 : -
// 최종 수정일 : 2024-02-12
public class NetworkLightParticleManager : NetworkBehaviour
{
    LightParticleManager localController;
    NetworkObject networkObject;

    LightParticleController lightParticleController;
    private void Awake()
    {
        networkObject = GetComponent<NetworkObject>();
    }
    public override void Spawned()
    {
        base.Spawned();
        if (networkObject.HasInputAuthority)
        {
            localController = FindObjectOfType<LightParticleManager>();
            localController.NetworkManager = this;
        }
        lightParticleController = GetComponentInChildren<LightParticleController>();
        var idolRef = NetworkDataManager.IdolRef;
        NetworkObject idolNetworkObject = NetworkDataManager.GetNetworkObject(idolRef);
        lightParticleController.InitAttractorMove(idolNetworkObject.gameObject.transform.GetChild(0));
    }
    // [Rpc(RpcSources.InputAuthority, RpcTargets.InputAuthority)] // Audience만 
    [Rpc()]
    public void Rpc_EmitLightParticle(RpcInfo info = default)
    {
        Debug.Log("Emit");
        var lightParticleController = GetComponentInChildren<LightParticleController>();

        if (lightParticleController != null)
            lightParticleController.EmitParticle();
    }
}
