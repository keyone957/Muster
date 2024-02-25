using Fusion;
using UnityEngine;

// Network 환경에서 Emoji를 관리하는 Manager
// 최초 작성자 : 김기홍
// 수정자 : -
// 최종 수정일 : 2024-02-08

public class NetworkEmojiManager : NetworkBehaviour
{
    EmojiController localController;
    NetworkObject networkObject;
    [SerializeField] GameObject[] emoticonList_LeftHand;
    [SerializeField] GameObject[] emoticonList_RightHand;

    //////////////////////////////////////////////////
    // Particle이 생성될 위치
    [SerializeField] private Transform particlePosition_Left;
    [SerializeField] private Transform particlePosition_Right;
    private void Awake()
    {
        networkObject = GetComponent<NetworkObject>();
    }
    public override void Spawned()
    {
        base.Spawned();
        if (networkObject.HasInputAuthority)
        {
            localController = FindObjectOfType<EmojiController>();
            localController.NetworkManager = this;
        }
    }
    [Rpc(RpcSources.InputAuthority, RpcTargets.All)]
    public void RPC_UseEmoticon(int index, bool isRightHand, RpcInfo info = default)
    {
        if (isRightHand)
        {
            var RController = emoticonList_RightHand[index].GetComponentInChildren<ParticleController>();
            RController.transform.position = particlePosition_Right.position;
            RController.EmitParticle();
        }
        else
        {
            var LController = emoticonList_LeftHand[index].GetComponentInChildren<ParticleController>();
            LController.transform.position = particlePosition_Left.position;
            LController.EmitParticle();
        }
    }
}
