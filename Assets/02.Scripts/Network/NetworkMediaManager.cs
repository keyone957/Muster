using Fusion;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Network 환경에서 MediaManager를 관리하는 Manager
// 최초 작성자 : 김기홍
// 수정자 : -
// 최종 수정일 : 2024-02-18

public class NetworkMediaManager : NetworkBehaviour
{
    [Rpc()]
    public void Rpc_PlayMusic(int index, float normalizedTime)
        =>MediaManager.Instance.Local_PlayMusic(index, normalizedTime);

    [Rpc()]
    public void Rpc_StopMusic()
        => MediaManager.Instance.Local_StopMusic();

    [Rpc()]
    public void Rpc_PlayVideo()
        => MediaManager.Instance.Local_PlayVideo();

    [Rpc()]
    public void Rpc_StopVideo()
        => MediaManager.Instance.Local_StopVideo();
    [Rpc()]
    public void Rpc_TurnOffAll()
        => MediaManager.Instance.Local_TurnOffAll();
}