

using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Video;
using System.Collections;
using TMPro;
using System;
using System.IO;
using System.Collections.Generic;


public class MyVideoPlayer : MonoBehaviour
{

    public GameObject cinemaPlane;
    public Sprite imagePlay;
    public Sprite imagePause;
    public GameObject knob;
    public GameObject progressBar;
    public GameObject progressBarBG;

    private float maxKnobValue;
    private float newKnobX;
    private float maxKnobX;
    private float minKnobX;
    private float knobPosY;
    private float simpleKnobValue;
    private float knobValue;
    private float progressBarWidth;
    private bool knobIsDragging;
    private bool videoIsJumping = false;
    private bool videoIsPlaying = false;

    public VideoPlayer videoPlayer;
    public TextMeshProUGUI nowTime;
    public TextMeshProUGUI VideoTime;
    public List<VideoClip> clips;
    public Button PlayButton;
    public Camera UICamera;
    private String VideoPath = "Assets/Resources/UI/Seqeuence/Video/";
    private String sVideoPath = "UI/Seqeuence/Video/";
    private void Start()
    {
        knobPosY = knob.transform.localPosition.y;
        videoPlayer = GetComponent<VideoPlayer>();
        PlayButton.GetComponent<Image>().sprite = imagePlay;

        videoPlayer.frame = 100;
        PlayButton.onClick.AddListener(BtnPlayVideo);

        progressBarWidth = progressBarBG.GetComponent<SpriteRenderer>().transform.localScale.x;
        //videoPlayer.clip = clips[0];
        if (videoPlayer.clip != null)
        {
            string nhh = String.Format("{0:00}", (int)videoPlayer.length / 60);
            string nss = String.Format("{0:00}", (int)videoPlayer.length % 60);
            string ms = ((int)(videoPlayer.length * 100) % 100).ToString("D2");
            VideoTime.text = nhh + ":" + nss + ":" + ms;
        }
        if (UISequencer.instance)
        {
            UISequencer.instance.setClip(getClipNames(), true);
            UICamera = UISequencer.instance.targetCamera.GetComponent<Camera>();
        }
        else
            Debug.Log("instance is null");
    }

    private void Update()
    {
        //진행바 진행
        if (!knobIsDragging && !videoIsJumping)
        {
            if (videoPlayer.frameCount > 0)
            {
                float progress = (float)videoPlayer.time / (float)videoPlayer.length;
                //Debug.Log(progressBarWidth +" * "+ progress);
                progressBar.transform.localScale = new Vector3(progressBarWidth * progress, progressBar.transform.localScale.y, 0);
                knob.transform.localPosition = new Vector2(progressBar.transform.localPosition.x + (progressBarWidth * progress), knob.transform.localPosition.y);
            }
        }
        string nhh = String.Format("{0:00}", (int)videoPlayer.time / 60);
        string nss = String.Format("{0:00}", (int)videoPlayer.time % 60);
        string ms = ((int)(videoPlayer.time * 100) % 100).ToString("D2");
        nowTime.text = nhh + ":" + nss + ":" + ms;
    }
    bool Approximately(float a, float b, float epsilon = 0.001f)
    {
        return Mathf.Abs(a - b) < epsilon;
    }

    public void KnobOnPressDown()
    {
        VideoStop();
        minKnobX = progressBar.transform.localPosition.x;
        maxKnobX = minKnobX + progressBarWidth;
    }

    public void KnobOnRelease()
    {
        Debug.Log("VVVVVV");
        knobIsDragging = false;
        CalcKnobSimpleValue();
        VideoPlay();
        VideoJump();
        StartCoroutine(DelayedSetVideoIsJumpingToFalse());
    }

    IEnumerator DelayedSetVideoIsJumpingToFalse()
    {
        yield return new WaitForSeconds(2);
        SetVideoIsJumpingToFalse();
    }

    public void KnobOnDrag()
    {
        knobIsDragging = true;
        videoIsJumping = true;
        Vector3 curScreenPoint = new Vector2(Input.mousePosition.x, Input.mousePosition.y);
        curScreenPoint.z = knob.transform.position.z - 9;
        Vector3 curPosition = UICamera.ScreenToWorldPoint(curScreenPoint);
        //Vector3 curPosition = Camera.main.ScreenToWorldPoint(curScreenPoint);
        knob.transform.position = new Vector2(curPosition.x, curPosition.y);
        newKnobX = knob.transform.localPosition.x;

        if (newKnobX > maxKnobX) { newKnobX = maxKnobX; }
        if (newKnobX < minKnobX) { newKnobX = minKnobX; }
        knob.transform.localPosition = new Vector2(newKnobX, knobPosY);
        CalcKnobSimpleValue();
        progressBar.transform.localScale = new Vector3(simpleKnobValue * progressBarWidth, progressBar.transform.localScale.y, 0);
    }

    private void SetVideoIsJumpingToFalse()
    {
        videoIsJumping = false;
    }

    private void CalcKnobSimpleValue()
    {
        maxKnobValue = maxKnobX - minKnobX;
        knobValue = knob.transform.localPosition.x - minKnobX;
        simpleKnobValue = knobValue / maxKnobValue;
    }

    private void VideoJump()
    {
        var frame = videoPlayer.frameCount * simpleKnobValue;
        videoPlayer.frame = (long)frame;
    }

    private void BtnPlayVideo()
    {
        if (videoIsPlaying)
        {
            VideoStop();
        }
        else
        {
            VideoPlay();
        }
    }

    private void VideoStop()
    {
        videoIsPlaying = false;
        videoPlayer.Pause();
        PlayButton.GetComponent<Image>().sprite = imagePlay;
        LineEventPlayer.instance.stopAni();
    }

    private void VideoPlay()
    {
        videoIsPlaying = true;
        videoPlayer.Play();
        PlayButton.GetComponent<Image>().sprite = imagePause;
        videoPlayer.time = 0;
        string nhh = String.Format("{0:00}", (int)videoPlayer.time / 60);
        string nss = String.Format("{0:00}", (int)videoPlayer.time % 60);
        string ms = ((int)(videoPlayer.time * 100) % 100).ToString("D2");
        nowTime.text = nhh + ":" + nss + ":" + ms;

        string hh = String.Format("{0:00}", (int)videoPlayer.length / 60);
        string ss = String.Format("{0:00}", (int)videoPlayer.length % 60);
        string mm = ((int)(videoPlayer.length * 100) % 100).ToString("D2");
        VideoTime.text = hh + ":" + ss + ":" + mm;
        LineEventPlayer.instance.startAni(videoPlayer.clip.name, simpleKnobValue);
    }
    public void setClip(int index)
    {
        videoPlayer.clip = clips[index];
        VideoTime.text = Math.Round(videoPlayer.length, 2).ToString();
        string hh = String.Format("{0:00}", (int)videoPlayer.length / 60);
        string ss = String.Format("{0:00}", (int)videoPlayer.length % 60);
        string mm = ((int)(videoPlayer.length * 100) % 100).ToString("D2");
        VideoTime.text = hh + ":" + ss + ":" + mm;
        VideoStop();
    }

    public List<String> getClipNames()
    {
        List<String> names = new List<String>();
        string streamingAssetsPath = Application.streamingAssetsPath;
        string[] videoFiles = Directory.GetFiles(streamingAssetsPath, "*.mp4");
        Debug.Log(videoFiles);
        // clips.Clear();
        
        // for (int i = 0; i < videoFiles.Length; i++)
        // {
        //     string videoName = "";
        //     string[] paths = videoFiles[i].Split("/");
        //     videoName = paths[paths.Length - 1];
        //     names.Add(videoName);
        //     string origin = Path.ChangeExtension(videoName, null);
        //     var video = Resources.Load<VideoClip>(sVideoPath + origin);
        //     clips.Add(video);
        // }
        
        for(int i=0;i<clips.Count;i++)
        {
            Debug.Log( i+":"+clips[i].name);
            string videoName = "";
            string[] paths = clips[i].name.Split("/");
            videoName = paths[paths.Length - 1];
            names.Add(videoName);
        }
        if(clips.Count > 0)
            videoPlayer.clip = clips[clips.Count - 1];
        return names;
    }
    public string getClipName()
    {
        if (videoPlayer.clip != null)
            return videoPlayer.clip.name;
        else
            return "";
    }
}
