using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using Unity.VisualScripting;
using TMPro;
using System;


// 응원봉 리스트의 한 항목(JSON)으로 텍스트 설정
public class FileListItem : MonoBehaviour
{
    [SerializeField]
    private Button _btn = null;

    [SerializeField]
    private TextMeshProUGUI _name = null;
    [SerializeField]
    private TextMeshProUGUI _mainText = null;
    [SerializeField]
    private TextMeshProUGUI _subText = null;
    [SerializeField]
    private Image _image = null;

    private const string _spriteRoot = "Sprite";
    public void Set(String name, UnityAction onClick)
    {
        _name.text = name;
        _btn.onClick.AddListener(onClick);
    }
    
}
