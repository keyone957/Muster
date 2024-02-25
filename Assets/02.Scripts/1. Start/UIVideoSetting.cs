using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.UI;

public class UIVideoSetting : UIWindow
{
    StartSceneManager startSceneManager;

    [SerializeField] Button _btnClose = null;
    [SerializeField] Transform content;
    [SerializeField] Button _btnAdd;
    [SerializeField] Button _btnClear;
    [SerializeField] Button _btnOK;
    [SerializeField] GameObject prefab;

    private List<String> urls;
    private void Awake()
    {
        // urls = SettingManager._instance.urls;
        _btnClose.onClick.AddListener(delegate { Destroy(gameObject); });
        _btnAdd.onClick.AddListener(AddPrefab);
        _btnClear.onClick.AddListener(Clear);
        _btnOK.onClick.AddListener(SaveInputFieldValues);
    }
    void Start()
    {
        if (urls != null)
        {
            for (int i = 0; i < urls.Count; i++)
            {
                AddPrefab();
            }
            int cnt = 0;
            foreach (Transform child in content)
            {
                TMP_InputField inputField = child.GetComponentInChildren<TMP_InputField>();
                if (inputField != null)
                {
                    inputField.text = urls[cnt];
                    cnt++;
                }
            }
        }

    }
    public void AddPrefab()
    {
        GameObject input = Instantiate(prefab);
        input.transform.SetParent(content);
        _btnAdd.transform.SetParent(_btnClear.transform);
        _btnAdd.transform.SetParent(content);
    }
    public void SaveInputFieldValues()
    {
        urls.Clear(); // 배열 초기화
        foreach (Transform child in content)
        {
            TMP_InputField inputField = child.GetComponentInChildren<TMP_InputField>();
            if (inputField != null)
            {
                urls.Add(inputField.text); // 값을 배열에 추가
            }
        }
        // SettingManager._instance.urls = urls;
        startSceneManager._UIManager.Pop();
    }
    public void Clear()
    {
        foreach (Transform child in content)
        {
            TMP_InputField inputField = child.GetComponentInChildren<TMP_InputField>();
            if (inputField != null)
            {
                child.GetComponent<videoItem>().Pop();
            }
        }
        _btnAdd.transform.SetParent(_btnClear.transform);
        _btnAdd.transform.SetParent(content);
    }
}