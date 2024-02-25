using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using Unity.VisualScripting;
using TMPro;


// 응원봉 리스트의 한 항목(JSON)으로 텍스트 설정
public class UISticItem : MonoBehaviour
{
    [SerializeField] private Button _btn = null;
    [SerializeField] private TextMeshProUGUI _name = null;
    [SerializeField] private TextMeshProUGUI _mainText = null;
    [SerializeField] private TextMeshProUGUI _subText = null;

    public void Set(StickList.Item item, UnityAction onClick)
    {
        _name.text = item._name;
        _mainText.text = item._MainText;
        if (item._subText != "") 
        {
            _subText.enabled = true;
            _subText.text = item._subText;
        }
        _btn.onClick.AddListener(onClick);
    }
}
