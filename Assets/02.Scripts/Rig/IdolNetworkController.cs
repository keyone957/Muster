using Fusion;
using Oculus.Movement.AnimationRigging;
using System;
using System.Diagnostics;
using UnityEngine;
using UnityEngine.Animations.Rigging;
using Debug = UnityEngine.Debug;
using UnityEngine.XR;


//아이돌 버튼 눌러서 말할수 있게 하기
//최초 작성자: 홍원기
//수정자: 
//최종 수정일: 2024-02-18

public class IdolNetworkController : NetworkBehaviour
{
    public RigBuilder rigBuilder;
    public RetargetingLayer retargetingLayer;
    public FacialTrackingNetworkController facialTracking;
    public override void Spawned()
    {
        SetIdolIOBT(Object);
    }
    private void SetIdolIOBT(NetworkObject playerObject)
    {
        if (playerObject.HasStateAuthority)
        {
            rigBuilder.enabled = true;
            retargetingLayer.enabled = true;
            facialTracking.enabled = true;
        }
    }
    private void Update()
    {
        SetIdolVoice(Object);
    }
    private void SetIdolVoice(NetworkObject playerObject)
    {
    //    if (playerObject.HasStateAuthority)
    //    {
    //        if (InputManager.instance.GetDeviceBtn(InputManager.instance._rightController, CommonUsages.primaryButton))
    //        {
    //            NetworkManager._instance._recorder.TransmitEnabled = true;
    //        }
    //        else
    //        {
    //            NetworkManager._instance._recorder.TransmitEnabled = false;
    //        }
    //    }
    }
}
