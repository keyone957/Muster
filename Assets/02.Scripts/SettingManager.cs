using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

// 설정 저장용
// 최초 작성자 : 김효중
// 수정자 : 김기홍
// 최종 수정일 : 2024-01-10
public class SettingManager : MonoBehaviour
{
    // Singleton
    public static SettingManager _instance = null;

    // Setting Data
    public PlayerManager.Role role;
    public string roomCode;
    // Not Use
    /* 
    public string _token;
    public GameObject _idolPrefab;
    public GameObject _AudiencePrefab;
    public string _idolPrefabName;
    public string _audiencePrefabName;
    public List<string> urls;
    */
    private void Awake()
    {
        if (_instance == null)
        {
            _instance = this;
            DontDestroyOnLoad(gameObject);
            // SceneManager.sceneLoaded += OnSceneLoaded;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    // 씬 로드 완료시 호출
    /*
    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        Debug.Log("Loaded: " + scene.name);
        if (scene.name == "StageScene")
        {
            // PlayerManager.instance.SetRole(role);
            // PlayerManager.instance.InitPosition(role);

        }
    }
    */
}