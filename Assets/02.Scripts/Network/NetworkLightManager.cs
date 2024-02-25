using Fusion;
using System;
using UnityEngine;

// Network 환경에서 Light를 관리하는 Manager
// 최초 작성자 : 김기홍
// 수정자 : -
// 최종 수정일 : 20204-02-16
public class NetworkLightManager : NetworkBehaviour
{
    [Rpc(RpcSources.StateAuthority, RpcTargets.InputAuthority | RpcTargets.StateAuthority)]
    public void Rpc_ActiveMainLight(bool active, float duration)
        => LightManager.Instance.Local_ActiveMainLight(active, duration);

    [Rpc(RpcSources.StateAuthority, RpcTargets.InputAuthority | RpcTargets.StateAuthority)]
    public void Rpc_ActiveSpotLight(bool active, float duration)
        => LightManager.Instance.Local_ActiveSpotLight(active, duration);

    [Rpc(RpcSources.StateAuthority, RpcTargets.InputAuthority | RpcTargets.StateAuthority)]
    public void Rpc_ActiveUpperLight(int index, bool active, float duration)
        => LightManager.Instance.Local_ActiveUpperLight(index, active, duration);

    [Rpc(RpcSources.StateAuthority, RpcTargets.InputAuthority | RpcTargets.StateAuthority)]
    public void Rpc_ActiveUpperLightAll(bool active, float duration)
        => LightManager.Instance.Local_ActiveUpperLightAll(active, duration);

    [Rpc(RpcSources.StateAuthority, RpcTargets.InputAuthority | RpcTargets.StateAuthority)]
    public void Rpc_ActiveLowerLight(int index, bool active, float duration)
        => LightManager.Instance.Local_ActiveLowerLight(index, active, duration);

    [Rpc(RpcSources.StateAuthority, RpcTargets.InputAuthority | RpcTargets.StateAuthority)]
    public void Rpc_ActiveLowerLightAll(bool active, float duration)
        => LightManager.Instance.Local_ActiveLowerLightAll(active, duration);

    [Rpc(RpcSources.StateAuthority, RpcTargets.InputAuthority | RpcTargets.StateAuthority)]
    public void Rpc_ActiveBeamLight(bool active, float duration)
    => LightManager.Instance.Local_ActiveBeamLightAll(active, duration);

    [Rpc(RpcSources.StateAuthority, RpcTargets.InputAuthority | RpcTargets.StateAuthority)]
    public void Rpc_ActiveLongLightAll(bool active, float duration)
        => LightManager.Instance.Local_ActiveLongLightAll(active, duration);

    [Rpc(RpcSources.StateAuthority, RpcTargets.InputAuthority | RpcTargets.StateAuthority)]
    public void Rpc_SetInit()
        => LightManager.Instance.Local_SetInit();
}
