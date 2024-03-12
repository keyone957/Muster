using Cysharp.Threading.Tasks;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum ActionType
{
    Idle,
    Cheering,
}

// Audience Dummy 객체를 조작하는 스크립트
public class AudienceDummy : MonoBehaviour
{
    Animator animator;
    private void Awake()
    {
        animator = GetComponent<Animator>();
    }
    public async void Action(ActionType type)
    {
        // 0.1초 ~ 0.3초 사이의 랜덤한 시간동안 Delay
        var rnd = Random.Range(0.1f, 0.5f);
        await UniTask.Delay((int)(rnd * 1000));
        animator.Play(type.ToString());
    }
}
