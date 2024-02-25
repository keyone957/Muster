using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class videoItem : MonoBehaviour
{
    public Button _btnSub;
    void Start()
    {
        _btnSub.onClick.AddListener(Pop);
    }

    public void Pop()
    {
        Destroy(gameObject);
    }
}
