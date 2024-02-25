using UnityEngine;
using Oculus.Platform;

public class IdolFollowCamera : MonoBehaviour
{
    Transform targetObject;  // 바라볼 대상 오브젝트의 Transfor
    void Update()
    {
        FollowIdol();// 카메라가 오브젝트를 바라보도록 설정
    }
    public void FollowIdol()
    {
        targetObject = FindTarget();
        if (targetObject == null) return;
        transform.LookAt(targetObject);
    }
    Transform FindTarget()
    {
        var idolRef = NetworkDataManager.IdolRef;
        if (idolRef == Fusion.PlayerRef.None) return null;
        var networkObject = NetworkDataManager.GetNetworkObject(idolRef);
        if (networkObject == null) return null;
        if (networkObject.transform.position.y > 80) return null; // 아이돌이 Intermission 중인 경우 null
        return networkObject.transform.GetChild(0).transform.GetChild(0) ;
    }
}
