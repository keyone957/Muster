using Fusion.XR.Host.Grabbing;
using Fusion.XR.Host.Rig;
using Fusion;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.XR;
using Fusion.XR.Host;

public class AudienceHardwareHand : MonoBehaviour
{
    public RigPart side;
    public HandCommand handCommand;

    [Header("Hand pose input")]
    public InputActionProperty thumbAction;
    public InputActionProperty gripAction;
    public InputActionProperty triggerAction;
    public InputActionProperty indexAction;
    public int handPose = 0;

    //False for Desktop mode, true for VR mode: when the hand grab is triggered by other scripts (MouseTeleport in desktop mode), we do not want to update the isGrabbing. It should only be done in VR mode
    public bool updateGrabWithAction = true;
    public NetworkTransform networkTransform;

    public IHandRepresentation localHandRepresentation;

    private void Awake()
    {
        thumbAction.EnableWithDefaultXRBindings(side: side, new List<string> { "thumbstickTouched", "primaryTouched", "secondaryTouched" });
        gripAction.EnableWithDefaultXRBindings(side: side, new List<string> { "grip" });
        triggerAction.EnableWithDefaultXRBindings(side: side, new List<string> { "trigger" });
        indexAction.EnableWithDefaultXRBindings(side: side, new List<string> { "triggerTouched" });
        gripAction.EnableWithDefaultXRBindings(side: side, new List<string> { "grip" });

        localHandRepresentation = GetComponentInChildren<IHandRepresentation>();
        networkTransform = GetComponent<NetworkTransform>();
    }

    #region Haptic feedback (vibrations)
    private UnityEngine.XR.InputDevice? _device = null;
    private bool supportImpulse = false;

    // Find the device associated to a VR controller, to be able to send it haptic feedback (vibrations)
    public UnityEngine.XR.InputDevice? Device
    {
        get
        {
            if (_device == null)
            {
                InputDeviceCharacteristics sideCharacteristics = side == RigPart.LeftController ? InputDeviceCharacteristics.Left : InputDeviceCharacteristics.Right;
                InputDeviceCharacteristics trackedControllerFilter = InputDeviceCharacteristics.Controller | InputDeviceCharacteristics.TrackedDevice | sideCharacteristics;

                List<UnityEngine.XR.InputDevice> foundControllers = new List<UnityEngine.XR.InputDevice>();
                InputDevices.GetDevicesWithCharacteristics(trackedControllerFilter, foundControllers);

                if (foundControllers.Count > 0)
                {
                    var inputDevice = foundControllers[0];
                    _device = inputDevice;
                    if (inputDevice.TryGetHapticCapabilities(out var hapticCapabilities))
                    {
                        // We memorize if this device can support vibrations
                        supportImpulse = hapticCapabilities.supportsImpulse;
                    }
                }
            }
            return _device;
        }
    }

    // If a device supporting haptic feedback has been detected, send a vibration to it (here in the form of an impulse)
    public void SendHapticImpulse(float amplitude, float duration, uint channel = 0)
    {
        if (Device != null)
        {
            var inputDevice = Device.GetValueOrDefault();
            if (supportImpulse)
            {
                inputDevice.SendHapticImpulse(channel, amplitude, duration);
            }
        }
    }

    // If a device supporting haptic feedback has been detected, send a vibration to it (here in the form of a buffer describing the vibration data)
    public void SendHapticBuffer(byte[] buffer, uint channel = 0)
    {
        if (Device != null)
        {
            var inputDevice = Device.GetValueOrDefault();
            if (supportImpulse)
            {
                inputDevice.SendHapticBuffer(channel, buffer);
            }
        }
    }
    #endregion
}
