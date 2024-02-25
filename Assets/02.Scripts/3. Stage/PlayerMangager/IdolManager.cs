using UnityEngine;

public class IdolManager : PlayerManager
{
    [SerializeField] protected Transform spawnPosition;
    [SerializeField] bool game = false;
    private void Awake()
    {
        playerRole = Role.Idol;
    }

    public override void MovePlayerToIntermission()
    {
        // Get NetworkRig
        var idolRef = NetworkDataManager.IdolRef;
        var networkRig = NetworkDataManager.GetNetworkObject(idolRef);

        // Set NetworkRig Position
        networkRig.transform.position = intermissionPosition;
        networkRig.transform.rotation = Quaternion.identity;

        // Set OVRCameraRig Position
        transform.position = intermissionPosition;
        transform.rotation = Quaternion.identity;
    }
    public override void MovePlayerToStage()
    {
        // Get NetworkRig
        var idolRef = NetworkDataManager.IdolRef;
        var networkRig = NetworkDataManager.GetNetworkObject(idolRef);

        // Set NetworkRig Position
        networkRig.transform.position = spawnPosition.position;
        networkRig.transform.rotation = spawnPosition.rotation;

        // Set OVRCameraRig Position
        transform.position = spawnPosition.position;
        transform.rotation = spawnPosition.rotation;
    }

}
