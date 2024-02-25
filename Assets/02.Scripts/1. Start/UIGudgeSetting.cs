using UnityEngine;
using UnityEngine.UI;

public class UIGudgeSetting : UIWindow
{
    private StartSceneManager startSceneManager;
    [SerializeField] private Button _btnClose = null;
    private void Awake()
    {
        startSceneManager = FindObjectOfType<StartSceneManager>();
        _btnClose.onClick.AddListener(startSceneManager._UIManager.Pop);
    }

}