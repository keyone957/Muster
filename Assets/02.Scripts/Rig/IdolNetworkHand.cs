using Fusion.XR.Host.Rig;
using Fusion;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[RequireComponent(typeof(NetworkTransform))]
[OrderAfter(typeof(NetworkRig))]
public class IdolNetworkHand : NetworkBehaviour
{
    [HideInInspector] public NetworkTransform networkTransform;
    [Networked(OnChanged = nameof(OnHandCommandChange))]
    public HandCommand HandCommand { get; set; }
    public RigPart side;
    IdolNetworkRig rig;
    IHandRepresentation handRepresentation;
    public bool IsLocalNetworkRig => rig.IsLocalNetworkRig;
    public IdolHardwareHand LocalHardwareHand => IsLocalNetworkRig ? (side == RigPart.LeftController ? rig.hardwareRig.leftHand : rig.hardwareRig.rightHand) : null;
    private void Awake()
    {
        rig = GetComponentInParent<IdolNetworkRig>();
        networkTransform = GetComponent<NetworkTransform>();
        handRepresentation = GetComponentInChildren<IHandRepresentation>();
    }
    public override void Render()
    {
        base.Render();
        if (IsLocalNetworkRig)
        {
            // Extrapolate for local user : we want to have the visual at the good position as soon as possible, so we force the visuals to follow the most fresh hand pose
            UpdateRepresentationWithLocalHardwareState();
        }
    }
    public static void OnHandCommandChange(Changed<IdolNetworkHand> changed)
    {
        // Will be called on all clients when the local user change the hand pose structure
        // We trigger here the actual animation update
        changed.Behaviour.UpdateHandRepresentationWithNetworkState();
    }
    // Update is called once per frame
    void UpdateHandRepresentationWithNetworkState()
    {
        if (handRepresentation != null) handRepresentation.SetHandCommand(HandCommand);
    }
    void UpdateRepresentationWithLocalHardwareState()
    {
        if (handRepresentation != null) handRepresentation.SetHandCommand(LocalHardwareHand.handCommand);
    }
}
