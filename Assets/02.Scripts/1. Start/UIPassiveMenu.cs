using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

// 아이돌, 관리자의 무대 세팅 UI
// 최초 작성자 : 김효중
// 수정자 : 
// 최종 수정일 : 2024-01-15
public class UIPassiveMenu :  UIWindow
{
    private StartSceneManager startSceneManager;

    [SerializeField] private Button _btnClose = null;
    [SerializeField] private Button _btnA1 = null;
    [SerializeField] private Button _btnA2 = null;
    [SerializeField] private Button _btnA3 = null;

    private void Awake()
    {
        startSceneManager = FindObjectOfType<StartSceneManager>();

        _btnClose.onClick.AddListener(startSceneManager._UIManager.Pop);
        _btnA1.onClick.AddListener(voidButton);
        _btnA2.onClick.AddListener(voidButton);
        _btnA3.onClick.AddListener(voidButton);
    }
    public void voidButton()
    {
        
    }

}