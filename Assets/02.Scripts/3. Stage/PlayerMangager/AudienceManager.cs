using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;
using static Oculus.Movement.Locomotion.ActivatableStateSet;

public class AudienceManager : PlayerManager
{
    [Header("Controller")]
    [SerializeField] private XRBaseController audienceLeftController;
    [SerializeField] private XRBaseController audienceRightController;

    [Header("Seat Info")]
    [SerializeField] protected Transform spawnPositionParent;
    [SerializeField] int seatIndex;
    //자리 선택했는지 안했는지에 대한 상태값
    public bool _isPlayerSelectSeat = false;

    private void Awake()
    {
        playerRole = Role.Audience;
    }

    //각 컨트롤러에 적용해 줄 프리펩 이름에 따라서 플레이어 응원봉 프리펩 변경하게. 
    //추후에 Define에 경로 넣어서 불러오게 할 예정.
    public void SetStickPrefab(string stickName)
    {
        audienceLeftController.modelPrefab = Resources.Load<GameObject>(stickName).transform;
        audienceRightController.modelPrefab = Resources.Load<GameObject>(stickName).transform;
    }
    public override void MovePlayerToIntermission()
    {
        transform.position = new Vector3(0, 100, 0);
        transform.rotation = Quaternion.identity;

        // 좌석 반납
        spawnPositionParent.GetChild(seatIndex).gameObject.SetActive(true);
    }
    public override void MovePlayerToStage()
    {
        transform.position = spawnPositionParent.GetChild(NetworkDataManager.SpawnedUsers.Count-1).position;
        transform.rotation = spawnPositionParent.GetChild(NetworkDataManager.SpawnedUsers.Count - 1).rotation;
        /*
        // 좌석 할당
        for (int i = 0; i < spawnPositionParent.childCount; i++)
        {
            if(spawnPositionParent.GetChild(i).gameObject.activeSelf == true)
            {
                transform.position = spawnPositionParent.GetChild(i).position;
                transform.rotation = spawnPositionParent.GetChild(i).rotation;
                spawnPositionParent.GetChild(i).gameObject.SetActive(false);
                seatIndex = i;
                break;
            }
        }*/
    }

}
