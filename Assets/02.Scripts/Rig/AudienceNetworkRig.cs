using Fusion;
using Fusion.XR.Host.Rig;
using UnityEngine;

[RequireComponent(typeof(NetworkTransform))]
public class AudienceNetworkRig : NetworkBehaviour
{
    public AudienceHardwareRig hardwareRig;
    public AudienceNetworkHand leftHand;
    public AudienceNetworkHand rightHand;
    public NetworkHeadset headset;

    [HideInInspector]
    public NetworkTransform networkTransform;

    private void Awake()
    {
        networkTransform = GetComponent<NetworkTransform>();
    }

    // As we are in host topology, we use the input authority to track which player is the local user
    public bool IsLocalNetworkRig => Object.HasInputAuthority;

    public override void Spawned()
    {
        base.Spawned();
        if (IsLocalNetworkRig)
        {
            hardwareRig = FindObjectOfType<AudienceHardwareRig>();
            if (hardwareRig == null) Debug.LogError("Missing HardwareRig in the scene");
        }
        
    }

    public override void FixedUpdateNetwork()
    {
        base.FixedUpdateNetwork();

        // update the rig at each network tick
        if (GetInput<RigInput>(out var input))
        {
            transform.position = input.playAreaPosition;
            transform.rotation = input.playAreaRotation;

            leftHand.transform.position = input.leftHandPosition;
            leftHand.transform.rotation = input.leftHandRotation;
            rightHand.transform.position = input.rightHandPosition;
            rightHand.transform.rotation = input.rightHandRotation;

            headset.transform.position = input.headsetPosition;
            headset.transform.rotation = input.headsetRotation;
            // we update the hand pose info. It will trigger on network hands OnHandCommandChange on all clients, and update the hand representation accordingly
            // leftHand.HandCommand = input.leftHandCommand;
            // rightHand.HandCommand = input.rightHandCommand;
        }
    }

    public override void Render()
    {
        base.Render();
        if (IsLocalNetworkRig)
        {
            // Extrapolate for local user:
            // we want to have the visual at the good position as soon as possible, so we force the visuals to follow the most fresh hardware positions
            // To update the visual object, and not the actual networked position, we move the interpolation targets
            networkTransform.InterpolationTarget.position = hardwareRig.transform.position;
            networkTransform.InterpolationTarget.rotation = hardwareRig.transform.rotation;

            leftHand.networkTransform.InterpolationTarget.position = hardwareRig.leftHand.transform.position;
            leftHand.networkTransform.InterpolationTarget.rotation = hardwareRig.leftHand.transform.rotation;
            rightHand.networkTransform.InterpolationTarget.position = hardwareRig.rightHand.transform.position;
            rightHand.networkTransform.InterpolationTarget.rotation = hardwareRig.rightHand.transform.rotation;

            headset.networkTransform.InterpolationTarget.position = hardwareRig.headset.transform.position;
            headset.networkTransform.InterpolationTarget.rotation = hardwareRig.headset.transform.rotation;
        }
    }
}
