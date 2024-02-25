using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Fusion;
using UnityEngine.UI;
using TMPro;

public class TestClientScript : NetworkBehaviour
{
    static int playerNum = 0;
    int n = 0;
    private void Awake()
    {
        n = playerNum++;
        FindObjectOfType<TestCode>().networkCode = this;
    }
    [Rpc]
    public void Rpc_All(RpcInfo info = default)
    {
        FindObjectOfType<Text>().text += n;
    }
    [Rpc(RpcSources.StateAuthority, RpcTargets.StateAuthority | RpcTargets.InputAuthority)]
    public void Rpc_Input(RpcInfo info = default)
    {
        FindObjectOfType<TMP_Text>().text += n;
        FindObjectOfType<Image>().color = new Color(Random.value, Random.value, Random.value, 1f);
    }
}
