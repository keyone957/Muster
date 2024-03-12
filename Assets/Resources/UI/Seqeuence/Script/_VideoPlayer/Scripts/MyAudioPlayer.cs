using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Video;
using UnityEngine.UI;
using System;

public class MyAudioPlayer : MonoBehaviour
{
    // FMOD (Audio)
    FMOD.Studio.EventInstance musicEvent;
    [SerializeField] private int pausedTimelinePosition;
    [SerializeField] public int currentTimelinePosition;
    [SerializeField] private int fullLength;

    // UI - Progress Bar
    [SerializeField] Sprite imagePlay;
    [SerializeField] Sprite imagePause;
    [SerializeField] RectTransform knob;
    [SerializeField] RectTransform progressBar;
    [SerializeField] RectTransform progressBarBG;
    [SerializeField] TextMeshProUGUI nowTime;
    [SerializeField] TextMeshProUGUI audioLength;
    [SerializeField] Button PlayButton;
    [SerializeField] Button SkipButtonL;
    [SerializeField] Button SkipButtonR;
    [SerializeField] Dropdown music;

    // Values
    [SerializeField] float skipTime = 3;
    private float maxKnobX;
    private float minKnobX;
    private float knobY;

    private float progressBarWidth;

    private bool knobIsDragging;
    private bool isPlaying = false;

    [HideInInspector] public Camera UICamera;

    void Start()
    {
        // Button Event Setting
        PlayButton.GetComponent<Image>().sprite = imagePlay;
        PlayButton.onClick.AddListener(BtnPlayVideo);
        SkipButtonL.onClick.AddListener(SkipAudioL);
        SkipButtonR.onClick.AddListener(SkipAudioR);

        // FMOD 이벤트 인스턴스 생성 (음원을 재생하기 위해 해당 이벤트를 설정해야 함)
        musicEvent = FMODUnity.RuntimeManager.CreateInstance("event:/Stage/Song/IU - Hold My Hand 2D");
        // musicEvent = FMODUnity.RuntimeManager.CreateInstance("event:/Stage/Song/QWER - Discord 2D");

        // 음원의 길이를 얻기 위해 FMOD 이벤트 디스크립터 획득
        FMOD.Studio.EventDescription musicDescription;
        musicEvent.getDescription(out musicDescription);

        // 음원의 길이 (시간 단위)를 가져오기 위해 FMOD 이벤트 속성 획득
        musicDescription.getLength(out fullLength);
        currentTimelinePosition = 0;
        pausedTimelinePosition = 0;

        // Set UI
        string nhh = String.Format("{0:00}", fullLength / 60000);
        string nss = String.Format("{0:00}", (fullLength % 60000) / 1000f);
        string ms = ((fullLength % 1000)).ToString("D3");
        audioLength.text = nhh + ":" + nss + ":" + ms;
        SetAudioUI(0);

        // Init Value
        progressBarWidth = progressBarBG.sizeDelta.x;
        minKnobX = progressBar.position.x;
        maxKnobX = minKnobX + progressBarWidth;
        knobY = knob.transform.position.y;

        if (UISequencer.instance)
        {
            UICamera = UISequencer.instance.targetCamera.GetComponent<Camera>();
        }
        else
            Debug.Log("instance is null");
    }
    private void Update()
    {
        // AudioLength 계산
        if (isPlaying)
        {
            musicEvent.getTimelinePosition(out currentTimelinePosition);
            SetTimeLine(currentTimelinePosition);

            // Progress bar
            if (!knobIsDragging)
            {
                SetProgressbar(currentTimelinePosition);
            }
        }

        // Skip
        if (Input.GetKeyDown(KeyCode.RightArrow))
        {
            SkipAudioR();
        }
        if (Input.GetKeyDown(KeyCode.LeftArrow))
        {
            SkipAudioL();
        }
    }

    /////////////////////////////////////////////////
    /////////////////////////////////////////////////
    // Knob
    public void KnobOnPressDown()
    {
        StopAudioInSequencer();
    }

    public void KnobOnRelease()
    {
        knobIsDragging = false;

        PlayeAudioInSequencer();

    }
    public void KnobOnDrag()
    {
        knobIsDragging = true;

        // Get Screen Point
        Vector3 curScreenPoint = new Vector2(Input.mousePosition.x, Input.mousePosition.y);
        var newKnobX = UICamera.ScreenToWorldPoint(curScreenPoint).x;

        // Set knob Position
        var worldPosition = new Vector2(newKnobX, knobY);
        knob.anchoredPosition = UICamera.WorldToScreenPoint(worldPosition);

        // Knob To TimelinePosition
        var max = progressBarWidth;
        var cur = knob.transform.localPosition.x - minKnobX;
        pausedTimelinePosition = (int)(cur / max * fullLength);
        SetAudioUI(pausedTimelinePosition);
    }

    /////////////////////////////////////////////////
    /////////////////////////////////////////////////
    // UI
    private void BtnPlayVideo()
    {
        if (isPlaying)
        {
            StopAudioInSequencer();
        }
        else
        {
            PlayeAudioInSequencer();
        }
    }
    void SkipAudioL()
    {
        int skip_ms = (int)(skipTime * 1000);
        currentTimelinePosition = currentTimelinePosition - skip_ms < 0 ? 0 : currentTimelinePosition - skip_ms;
        musicEvent.setTimelinePosition(currentTimelinePosition);
        SetAudioUI(currentTimelinePosition);

        if (isPlaying == false) pausedTimelinePosition = currentTimelinePosition;
    }
    void SkipAudioR()
    {
        int skip_ms = (int)(skipTime * 1000);
        currentTimelinePosition = currentTimelinePosition + skip_ms > fullLength ? fullLength : currentTimelinePosition + skip_ms;
        musicEvent.setTimelinePosition(currentTimelinePosition);
        SetAudioUI(currentTimelinePosition);

        if (isPlaying == false) pausedTimelinePosition = currentTimelinePosition;
    }

    /////////////////////////////////////////////////
    /////////////////////////////////////////////////
    // Audio
    private void PlayeAudioInSequencer()
    {
        // 중단 위치 찾기
        currentTimelinePosition = pausedTimelinePosition;
        musicEvent.start();
        musicEvent.setTimelinePosition(currentTimelinePosition);

        PlayButton.GetComponent<Image>().sprite = imagePause;
        SetAudioUI(currentTimelinePosition);
        LineEventPlayer.instance.StartAniForTest("", pausedTimelinePosition/(float)fullLength);

        isPlaying = true;
    }
    private void StopAudioInSequencer()
    {
        // 음악 정지
        pausedTimelinePosition = currentTimelinePosition;
        musicEvent.stop(FMOD.Studio.STOP_MODE.IMMEDIATE);
        PlayButton.GetComponent<Image>().sprite = imagePlay;

        SetAudioUI(pausedTimelinePosition);

        isPlaying = false;
        LineEventPlayer linePlayer = GameObject.FindAnyObjectByType<LineEventPlayer>();
        linePlayer.stopAni();
    }
    public void StopAudio()
    {
        musicEvent.stop(FMOD.Studio.STOP_MODE.IMMEDIATE);
    }
    ////////////////////////////////////////////////
    // Set Progressbar
    void SetAudioUI(int cur)
    {
        SetProgressbar(cur);
        SetTimeLine(cur);
    }
    void SetProgressbar(int cur)
    {
        float progress = cur / (float)fullLength;
        progressBar.sizeDelta = new Vector2(progressBarWidth * progress, progressBar.sizeDelta.y);
        knob.anchoredPosition = new Vector2(progressBarWidth * progress, 0);
    }
    void SetTimeLine(int cur)
    {
        string nhh = String.Format("{0:00}", cur / 60000);
        string nss = String.Format("{0:00}", (cur % 60000) / 1000f);
        string ms = ((cur % 1000)).ToString("D3");
        nowTime.text = nhh + ":" + nss + ":" + ms;
    }
}
