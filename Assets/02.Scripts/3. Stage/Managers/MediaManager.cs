using Cysharp.Threading.Tasks;
using FMODUnity;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Video;

// 음악 및 동영상 관리
// 최초 작성자 : 김기홍
// 수정자 : -
// 최종 수정일 : 20204-02-18

public enum MUSICLIST
{
    HOLDMYHAND = 0,
    DISCORD = 1,
}
public class MediaManager : MonoBehaviour
{
    public static MediaManager Instance { get; private set; }
    private NetworkMediaManager networkedManager;



    [SerializeField] public GameObject IdolSideL;
    [SerializeField] public GameObject IdolSideR;
    [SerializeField] public GameObject SubScreen1;
    [SerializeField] public GameObject SubScreen2;


    // 동영상이 나올 스크린
    [SerializeField] GameObject mainScreen;
    VideoPlayer mainScreenVideoPlayer;
    [SerializeField] GameObject subscreen1;
    VideoPlayer subscreen1ScreenVideoPlayer;
    [SerializeField] GameObject subscreen2;
    VideoPlayer subscreen2ScreenVideoPlayer;

    // 음악 재생될 스피커
    [SerializeField] GameObject speaker1;
    StudioEventEmitter[] emitter1;
    [SerializeField] GameObject speaker2;
    StudioEventEmitter[] emitter2;


    private void Awake()
    {
        if (Instance == null)
            Instance = this;
        else
            Destroy(this.gameObject);
    }
    private void Start()
    {
        // Set Video
        mainScreenVideoPlayer = mainScreen.GetComponent<VideoPlayer>();
        if (mainScreenVideoPlayer == null)
        {
            Debug.Log($"There is No Video Player : {mainScreen}");
        }
        subscreen1ScreenVideoPlayer = subscreen1.GetComponent<VideoPlayer>();
        if (subscreen1ScreenVideoPlayer == null)
        {
            Debug.Log($"There is No Video Player : {subscreen1}");
        }
        subscreen2ScreenVideoPlayer = subscreen2.GetComponent<VideoPlayer>();
        if (subscreen2ScreenVideoPlayer == null)
        {
            Debug.Log($"There is No Video Player : {subscreen2}");
        }

        // Set Audio
        emitter1 = speaker1.GetComponents<StudioEventEmitter>();
        if (emitter1 == null)
        {
            Debug.Log($"There is No Speaker : {speaker1}");
        }
        emitter2 = speaker2.GetComponents<StudioEventEmitter>();
        if (emitter2 == null)
        {
            Debug.Log($"There is No Speaker : {speaker2}");
        }
    }

    //////////////////////////////////////////
    // Server에서 호출하는 함수
    #region SERVER FUNCTION
    /// <summary> Server(=Idol)만 호출가능한 함수 </summary>
    public void Server_PlayMusic(float normalizedTime)
    {
        if (networkedManager == null)
            networkedManager = FindObjectOfType<NetworkMediaManager>();
        // networkedManager.Rpc_PlayMusic((int)MUSICLIST.DISCORD ,normalizedTime);
        networkedManager.Rpc_PlayMusic((int)MUSICLIST.HOLDMYHAND ,normalizedTime);
    }
    /// <summary> Server(=Idol)만 호출가능한 함수 </summary>
    public void Server_StopMusic()
    {
        if (networkedManager == null)
            networkedManager = FindObjectOfType<NetworkMediaManager>();
        networkedManager.Rpc_StopMusic();
    }
    /// <summary> Server(=Idol)만 호출가능한 함수 </summary>
    public void Server_PlayerVideo()
    {
        if (networkedManager == null)
            networkedManager = FindObjectOfType<NetworkMediaManager>();
        networkedManager.Rpc_PlayVideo();
    }
    /// <summary> Server(=Idol)만 호출가능한 함수 </summary>
    public void Server_StopVideo()
    {
        if (networkedManager == null)
            networkedManager = FindObjectOfType<NetworkMediaManager>();
        networkedManager.Rpc_StopVideo();
    }
    /// <summary> Server(=Idol)만 호출가능한 함수. PlayVideo 작동시 공연 중 환경으로, StopVideo 작동시 공연 아닌 환경으로 되돌아갈 수 있다. </summary>
    public void Server_TurnOffAll()
    {
        if (networkedManager == null)
            networkedManager = FindObjectOfType<NetworkMediaManager>();
        networkedManager.Rpc_TurnOffAll();
    }
    #endregion
    //////////////////////////////////////////
    // Local에서 호출하는 함수
    #region LOCAL FUNCTION
    public void Local_PlayMusic(int index, float normalizedTime)
    {
        Debug.Log("음악 재생");
        emitter1[index].Play();
        emitter2[index].Play();
    }
    public void Local_StopMusic()
    {
        Debug.Log("음악 정지");
        foreach (var e in emitter1)
        {
            e.Stop();
        }
        foreach (var e in emitter2)
        {
            e.Stop();
        }
    }
    public void Local_PlayVideo()
    {
        Debug.Log("동영상 재생");
        // 메인 스크린은 동영상 재생
        // 서브스크린 2개는 아이돌을 비춤
        IdolSideL.SetActive(true);
        IdolSideR.SetActive(true);
        SubScreen1.SetActive(false);
        SubScreen2.SetActive(false);
        StartCoroutine(MediaController.instance.FadeInCoroutine(1f));
    }
    public void Local_StopVideo()
    {
        Debug.Log("동영상 정지");
        IdolSideL.SetActive(false);
        IdolSideR.SetActive(false);
        SubScreen1.SetActive(true);
        SubScreen2.SetActive(true);
        mainScreen.SetActive(false);
        StartCoroutine(MediaController.instance.FadeOutCoroutine(1f));
    }
    public void Local_TurnOffAll()
    {
        IdolSideL.SetActive(false);
        IdolSideR.SetActive(false);
        SubScreen1.SetActive(false);
        SubScreen2.SetActive(false);
        StartCoroutine(MediaController.instance.FadeOutCoroutine(0.1f));
    }
    #endregion
}
