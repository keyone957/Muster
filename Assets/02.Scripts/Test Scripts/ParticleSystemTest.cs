using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParticleSystemTest : MonoBehaviour
{
    [SerializeField] LightParticleController lightParticleController;
    [SerializeField] Transform target;
    private void Start()
    {
        lightParticleController.InitAttractorMove(target);
    }
    private void Update()
    {
        lightParticleController.EmitParticle();
    }
}
