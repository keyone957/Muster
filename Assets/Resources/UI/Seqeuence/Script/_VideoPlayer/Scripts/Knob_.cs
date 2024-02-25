using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Knob_ : MonoBehaviour
{
    [SerializeField] MyAudioPlayer audioPlayerScript;


    void OnMouseDown()
    {
        audioPlayerScript.KnobOnPressDown();
    }

    void OnMouseUp()
    {
        audioPlayerScript.KnobOnRelease();
    }

    void OnMouseDrag()
    {
        audioPlayerScript.KnobOnDrag();
    }
}
