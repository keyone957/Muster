using UnityEngine;

// 여러 Gamification을 관리하는 스크립트, 각 Gamification의 시작을 담당한다.
// 최초 작성자 : 김기홍
// 수정자 : -
// 최종 수정일 : 2024-01-29

public class GamificationManager : MonoBehaviour
{
    #region Singleton
    public static GamificationManager instance;
    private void Awake()
    {
        // Singleton
        if(instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(gameObject);
        }

        // Audience인 경우 비활성화
        if (SettingManager._instance.role == PlayerManager.Role.Audience)
        {
            gameObject.SetActive(false);
            return;
        }
    }
    #endregion

    // Server(Idol)이 호출하는 함수이다
    public void StartGamification<T>() where T : Gamification
    {
        // Server가 아닌 경우 종료
        if (NetworkManager._instance._runner.IsServer == false) return;

        // T 타입의 인스턴스 생성
        T gamificationInstance = GetComponent<T>();
        if(gamificationInstance == null)
        {
            gamificationInstance = gameObject.AddComponent<T>();
        }

        // 생성된 인스턴스의 StartGamification 함수 호출
        gamificationInstance.StartGamification();
    }
}
