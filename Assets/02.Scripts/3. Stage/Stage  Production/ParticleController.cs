using Fusion;
using UnityEngine;

// Particle System을 조작하는 스크립트
// 최초 작성자 : 김기홍
// 수정자 : -
// 최종 수정일 : 2024-02-05
// 최종 수정 내용
//  - RequiredComponent 추가 및 멤버 변수 명 변경
//  - 함수의 보호수준 변경

[RequireComponent(typeof(ParticleSystem))]
public class ParticleController : MonoBehaviour
{
    protected ParticleSystem m_particleSystem;
    protected ParticleSystem.Particle[] m_Particles;


    protected void Awake()
    {
        m_particleSystem = GetComponent<ParticleSystem>();
        m_particleSystem.Stop();
    }

    protected void Start()
    {
        m_particleSystem = GetComponent<ParticleSystem>();
        m_Particles = new ParticleSystem.Particle[m_particleSystem.main.maxParticles];
    }
    public void EmitParticle()
    {
        m_particleSystem.Emit(1);
    }
}
