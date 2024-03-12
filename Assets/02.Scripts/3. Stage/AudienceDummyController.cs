using Fusion;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.GraphicsBuffer;

// AudienceDummy"들"을 관리하는 스크립트
// 최초 작성자 : 김기홍
// 수정자 : -
// 최종 수정일 : 2024-02-19

public class AudienceDummyController : MonoBehaviour
{
    public static AudienceDummyController instance;
    [SerializeField] AudienceDummy[] audienceDummies = null;

    static NetworkRunner _runner;

    bool lookIdol;
    Transform target;
    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(this);
        }
        _runner = FindObjectOfType<NetworkRunner>();
    }
    private void Start()
    {
        audienceDummies = FindObjectsOfType<AudienceDummy>();
        Local_DoIdle();
    }
    public void Server_DoCheering()
    {
        NetworkAudienceDummyController.Rpc_DoCheering(_runner);
    }
    public void Server_DoIdle()
    {
        NetworkAudienceDummyController.Rpc_DoIdle(_runner);
    }
    public void Server_LookTarget(bool _on)
    {
        NetworkAudienceDummyController.Rpc_LookTarget(_runner, _on);
    }
    public void Local_DoIdle()
    {
        foreach (var au in audienceDummies)
        {
            au.Action(ActionType.Idle);
        }
    }
    public void Local_DoCheering()
    {
        foreach (var au in audienceDummies)
        {
            au.Action(ActionType.Cheering);
        }
    }
    private void FixedUpdate()
    {
        if (lookIdol == false) return;

        foreach (var au in audienceDummies)
        {
            if (target == null) au.transform.rotation = Quaternion.Euler(0, 180, 0);
            else if (target.transform.GetChild(0).transform.position.y > 80) au.transform.rotation = Quaternion.Euler(0, 180, 0);
            else au.transform.LookAt(target);
        }
    }
    public void LookTarget(bool _on)
    {
        lookIdol = _on;
        target = NetworkDataManager.GetNetworkObject(NetworkDataManager.IdolRef).transform.GetChild(0);
    }
}
