using UnityEngine;
using UnityEngine.UI;

// 서브 스크린 비디오 설정용
// 최초 작성자 : 김효중
// 수정자 : 김기홍
// 최종 수정일 : 2024-01-15
public class UISubVideoSetting : UIWindow
{
    private StartSceneManager startSceneManager;

    [SerializeField] private Button _btnClose = null;
   // [SerializeField] private Button _btnAdd = null;
   // [SerializeField] private GameObject inputPrefab = null;
    private void Awake()
    {
        startSceneManager = FindObjectOfType<StartSceneManager>();
        _btnClose.onClick.AddListener(startSceneManager._UIManager.Pop);
    }
    
}