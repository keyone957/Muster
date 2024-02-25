using Fusion;
using UnityEngine;

// Network 환경에서 StageScreenUI를 관리하는 Manager
// 최초 작성자 : 김기홍
// 수정자 : -
// 최종 수정일 : 2024-02-08

// 최종 수정 내용
// - 함수 템플릿 사용 안하도록 변경 (RPC에서 사용못하는듯.. 오류생김)
public class NetworkStageScreenUIManager : NetworkBehaviour
{
    [Rpc(RpcSources.StateAuthority, RpcTargets.InputAuthority | RpcTargets.StateAuthority)]
    public void Rpc_ActiveMainScreen(bool activate, RpcInfo info = default) => StageScreenUIManager.Instance.Local_ActiveMainScreen(activate);

    [Rpc(RpcSources.StateAuthority, RpcTargets.InputAuthority | RpcTargets.StateAuthority)]
    public void Rpc_ActiveLeftScreen(bool activate, RpcInfo info = default) => StageScreenUIManager.Instance.Local_ActiveLeftScreen(activate);

    [Rpc(RpcSources.StateAuthority, RpcTargets.InputAuthority | RpcTargets.StateAuthority)]
    public void Rpc_ActiveRightScreen(bool activate, RpcInfo info = default) => StageScreenUIManager.Instance.Local_ActiveRightScreen(activate);

    [Rpc(RpcSources.StateAuthority, RpcTargets.InputAuthority | RpcTargets.StateAuthority)]
    public void Rpc_SetMainScreenText(string midText, RpcInfo info = default) => StageScreenUIManager.Instance.Local_SetMainScreenText(midText);

    [Rpc(RpcSources.StateAuthority, RpcTargets.InputAuthority | RpcTargets.StateAuthority)]
    public void Rpc_SetMainScreenText(string midText, string subText, RpcInfo info = default) => StageScreenUIManager.Instance.Local_SetMainScreenText(midText, subText);

    [Rpc(RpcSources.StateAuthority, RpcTargets.InputAuthority | RpcTargets.StateAuthority)]
    public void Rpc_SetLeftScreenText(string midText, RpcInfo info = default) => StageScreenUIManager.Instance.Local_SetLeftScreenText(midText);

    [Rpc(RpcSources.StateAuthority, RpcTargets.InputAuthority | RpcTargets.StateAuthority)]
    public void Rpc_SetLeftScreenText(string midText, string subText, RpcInfo info = default) => StageScreenUIManager.Instance.Local_SetLeftScreenText(midText, subText);

    [Rpc(RpcSources.StateAuthority, RpcTargets.InputAuthority | RpcTargets.StateAuthority)]
    public void Rpc_SetRightScreenText(string midText, RpcInfo info = default) => StageScreenUIManager.Instance.Local_SetRightScreenText(midText);

    [Rpc(RpcSources.StateAuthority, RpcTargets.InputAuthority | RpcTargets.StateAuthority)]
    public void Rpc_SetRightScreenText(string midText, string subText, RpcInfo info = default) => StageScreenUIManager.Instance.Local_SetRightScreenText(midText, subText);

    [Rpc(RpcSources.StateAuthority, RpcTargets.InputAuthority | RpcTargets.StateAuthority)]
    public void Rpc_SetSlider(float now, float max, RpcInfo info = default) => StageScreenUIManager.Instance.Local_SetSlider(now, max);

    [Rpc(RpcSources.StateAuthority, RpcTargets.InputAuthority | RpcTargets.StateAuthority)]
    public void Rpc_ActivateSlider(bool activate, RpcInfo info = default) => StageScreenUIManager.Instance.Local_ActivateSlider(activate);
}
