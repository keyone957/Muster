using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Video;
using FMODUnity;
public class SoundSetting : UIWindow
{
    [SerializeField] Button backButton;
    [SerializeField] Button acceptButton;
    [SerializeField] Slider soundSlider;
    [SerializeField] Slider musicSlider;
    StudioEventEmitter emitter1;
    // Start is called before the first frame update
    void Start()
    {
        backButton.onClick.AddListener(onClickBack);
        acceptButton.onClick.AddListener(onClickAccept);
    }
    public void onClickBack()
    {
        Destroy(gameObject);
    }
    public void onClickAccept()
    {
        Debug.Log(soundSlider.value + " - " + musicSlider.value);
        // TODO: 마이크 설정 <- 아이돌의 AudioSource -> soundSlider.value  조절
        //AudioSource audioSource = new AudioSource();
        //audioSource.volume = soundSlider.value;
        // 노래 소리 설정 <= publick으로 바꿀것
        //MediaManager.Instance.emitter1.SetParameter("Volume", musicSlider.value);
        //MediaManager.Instance.emitter2.SetParameter("Volume", musicSlider.value);
        Destroy(gameObject);
    }
}
