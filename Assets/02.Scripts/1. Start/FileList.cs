using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.IO;
using System;
public class FileList : UIWindow
{
    [SerializeField]
    private RectTransform _contents = null;
    [SerializeField]
    private GameObject _itemPrefab = null;
    [SerializeField]
    private Button _btnClose = null;
    [SerializeField]
    public TextMeshProUGUI _fileName = null;
    public TextMeshProUGUI _folderName = null;
    public Button _btnOK = null;
    public Button _btnUp = null;
    private DirectoryInfo cuurentDirectory; // 현재 폴더
    private void Awake()
    {
        _btnClose.onClick.AddListener(OnClickBack);
        _btnOK.onClick.AddListener(OnClickOK);
         // 백그라운드 진행
        Application.runInBackground = true;
        // 최초는 바탕 화면
        String desktopFolder = Environment.GetFolderPath(Environment.SpecialFolder.Desktop);
        cuurentDirectory = new DirectoryInfo(desktopFolder);
        // 현재 폴더로 변경
        UpdateDirectory(cuurentDirectory);
    }
    public override bool OnKeyInput()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            OnClickBack();
        }

        return true;
    }
    private void UpdateDirectory(DirectoryInfo directory)
    {
        cuurentDirectory = directory;
        _folderName.text = cuurentDirectory.Name;
        Debug.Log($"현재폴더명 :  {cuurentDirectory.Name}");
        // 현재 폴더에 존재하는 모든 폴더 이름 출력
        foreach (DirectoryInfo dir in cuurentDirectory.GetDirectories())
        {
            Debug.Log(dir.Name);
            //AddItem(badgeList._items[i]);
        }
        // 현재 폴더에 존재하는 모든 파일 이름 출력
        foreach(FileInfo file in cuurentDirectory.GetFiles())
        {
            Debug.Log(file.Name);
            AddItem(file.Name);
        }
    }

    private void OnClickBack()
    {
        Destroy(gameObject);
    }
    private void AddItem(string name)
    {
        FileListItem itemUI = UIManager.CreateWidget<FileListItem>(_itemPrefab, _contents);
        itemUI.Set(name
            , delegate { OnItemSelected(name); }
            );
    }
    private void OnClickOK()
    {
        // SettingManager._instance._idolPrefabName = name;
        Destroy(gameObject);
    }
    private void OnItemSelected(string name)
    {
        _fileName.text = name;
    }
}
