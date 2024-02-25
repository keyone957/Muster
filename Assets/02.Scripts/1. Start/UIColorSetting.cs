using UnityEngine;
using UnityEngine.UI;

public class UIColorSetting : UIWindow
{
    private StartSceneManager startSceneManager;

    [SerializeField] private Button _btnClose = null;
    private void Awake()
    {
        _btnClose.onClick.AddListener(startSceneManager._UIManager.Pop);
    }

}