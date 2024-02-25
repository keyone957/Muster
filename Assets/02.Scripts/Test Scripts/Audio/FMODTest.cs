using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FMODTest : MonoBehaviour
{
    [FMODUnity.EventRef]
    public string fmodEventPath;

    FMOD.Studio.EventInstance eventInstance;

    void Start()
    {
        // FMOD 이벤트 인스턴스 생성
        eventInstance = FMODUnity.RuntimeManager.CreateInstance(fmodEventPath);
        eventInstance.start();
    }

    void Update()
    {
        // 예시: 파라미터 "Change"의 값을 조작하여 음악의 볼륨을 조절
        float parameterValue = Mathf.PingPong(Time.time, 1f); // 예시로 시간에 따라 값을 변경
        eventInstance.setParameterByName("Change", parameterValue);
    }

    void OnDestroy()
    {
        // FMOD 이벤트 인스턴스 해제
        eventInstance.stop(FMOD.Studio.STOP_MODE.ALLOWFADEOUT);
        eventInstance.release();
    }
}
