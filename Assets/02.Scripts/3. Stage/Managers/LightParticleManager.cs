using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LightParticleManager : MonoBehaviour
{
    public static LightParticleManager Instance { get; private set; }
    public NetworkLightParticleManager NetworkManager { private get; set; }

    public bool EmitSignal { private get; set; } = false;
    //////////////////////////////////////////////////
    // Unity Functions
    private void Awake()
    {
        if (Instance == null)
            Instance = this;
        else
            Destroy(Instance);

        if (SettingManager._instance.role == PlayerManager.Role.Idol)
        {
            gameObject.SetActive(false);
            return;
        }
    }
    private void Update()
    {
        if (EmitSignal)
        {
            NetworkManager.Rpc_EmitLightParticle();
            EmitSignal = false;
        }
    }
    public void Server_EmitLightParticle()
    {
        EmitSignal = true;
    }
}