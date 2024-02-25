using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// 특정 ParticleController를 찾기위해 만든 스크립트
// 최초 작성자 : 김기홍
// 수정자 : -
// 최종 수정일 : 2024-02-01
public class LightParticleController : ParticleController
{
    // AttractorMove
    Vector3[] thirdPoints;
    [SerializeField] float speed;
    Transform target;
    private int numParticlesAlive;

    protected new void Start()
    {
        base.Start();
        thirdPoints = new Vector3[m_particleSystem.main.maxParticles];
        m_particleSystem.Stop();
    }
    void Update()
    {
        numParticlesAlive = m_particleSystem.GetParticles(m_Particles);
        if (numParticlesAlive == 0) return;
        for (int i = 0; i < numParticlesAlive; i++)
        {
            if (m_Particles[i].velocity.magnitude > 0)
            {
                thirdPoints[i] = m_Particles[i].velocity.normalized * speed;
                m_Particles[i].velocity = Vector3.zero;
            }
            var ratio = 1 - (m_Particles[i].remainingLifetime / m_Particles[i].startLifetime);
            var p1 = transform.position;
            var p2 = thirdPoints[i];
            var p3 = target.position;
            var p4 = Vector3.Lerp(p1, p2, ratio);
            var p5 = Vector3.Lerp(p2, p3, ratio);
            m_Particles[i].position = Vector3.Lerp(p4, p5, ratio);
        }
        m_particleSystem.SetParticles(m_Particles, numParticlesAlive);
    }
    public void InitAttractorMove(Transform _target)
    {
        target = _target;
    }
}
