using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainSceneManager : MonoBehaviour
{
    private async void Start()
    {
        await SceneLoader._instance.LoadScene(SceneName.Title);
    }
}
