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
        // FMOD �̺�Ʈ �ν��Ͻ� ����
        eventInstance = FMODUnity.RuntimeManager.CreateInstance(fmodEventPath);
        eventInstance.start();
    }

    void Update()
    {
        // ����: �Ķ���� "Change"�� ���� �����Ͽ� ������ ������ ����
        float parameterValue = Mathf.PingPong(Time.time, 1f); // ���÷� �ð��� ���� ���� ����
        eventInstance.setParameterByName("Change", parameterValue);
    }

    void OnDestroy()
    {
        // FMOD �̺�Ʈ �ν��Ͻ� ����
        eventInstance.stop(FMOD.Studio.STOP_MODE.ALLOWFADEOUT);
        eventInstance.release();
    }
}
