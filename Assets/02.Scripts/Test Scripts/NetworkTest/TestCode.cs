using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestCode : MonoBehaviour
{
    public TestClientScript networkCode;
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            networkCode.Rpc_All();
        }
        if (Input.GetKeyDown(KeyCode.Tab))
        {
            Debug.Log("Controll");
            networkCode.Rpc_Input();
        }
    }
}
