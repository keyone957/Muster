using UnityEngine;
using UnityEngine.SceneManagement;
using System;

public class StartSceneManager : MonoBehaviour
{
    public StartUIManager _UIManager { get; private set; }
    // Fade In 처리 시간
    [Range(0.5f, 2.0f)]
    [SerializeField] float _fadeDuration = 1.0f;

    [SerializeField] GameObject vrNotice;

    private void Awake()
    {
        // Initialize
        vrNotice.SetActive(true);

        // UI Manager 초기화 
        _UIManager = FindObjectOfType<StartUIManager>();
        _UIManager.Initailize();

        // SceneManager.sceneLoaded += OnSceneLoaded;

        SceneStartSequence();
    }

    public void DoExit()
    {
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#else
            Application.Quit();
#endif
    }

    /// <summary>
    /// 씬 시작 시퀀스
    /// </summary>
    void SceneStartSequence()
    {
        // Show Vr Notice
        _UIManager._FadeOverlay.DoFadeOut(0.5f);
        vrNotice.SetActive(true);

        // 로딩
        _UIManager._FadeOverlay.DoFadeIn(_fadeDuration);

        // Show Title Menu
        _UIManager.OpenTitleMenu();
    }
}