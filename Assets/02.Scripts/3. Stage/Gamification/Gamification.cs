using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// 각종 Gamification 스크립트가 구현해야할 함수를 가리키는 인터페이스
// 최초 작성자 : 김기홍
// 수정자 : -
// 최종 수정일 : 2024-01-30

public abstract class Gamification : MonoBehaviour
{
    public abstract void StartGamification();
}
