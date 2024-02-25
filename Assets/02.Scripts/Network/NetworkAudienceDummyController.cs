using Fusion;
using UnityEngine;

// AudienceDummy 조작
// 최초 작성자 : 김기홍
// 수정자 : -
// 최종 수정일 : 2024-02-23

public class NetworkAudienceDummyController : SimulationBehaviour
{
    [Rpc(RpcSources.StateAuthority, RpcTargets.InputAuthority | RpcTargets.StateAuthority)]
    public static void Rpc_DoIdle(NetworkRunner runner, RpcInfo info = default)
        => AudienceDummyController.instance.Local_DoIdle();

    [Rpc(RpcSources.StateAuthority, RpcTargets.InputAuthority | RpcTargets.StateAuthority)]
    public static void Rpc_DoCheering(NetworkRunner runner, RpcInfo info = default)
        => AudienceDummyController.instance.Local_DoCheering();

    [Rpc(RpcSources.StateAuthority, RpcTargets.InputAuthority | RpcTargets.StateAuthority)]
    public static void Rpc_LookTarget(NetworkRunner runner, bool _on, RpcInfo info = default)
        => AudienceDummyController.instance.LookTarget(_on);
}
