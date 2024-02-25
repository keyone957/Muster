using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using Unity.VisualScripting;
using TMPro;


// 응원봉 리스트의 한 항목(JSON)으로 텍스트 설정
public class UIConsertItem : MonoBehaviour
{
    [SerializeField]
    private Button _btn = null;

    [SerializeField]
    private TextMeshProUGUI _title = null;
    [SerializeField]
    private Image _image = null;
    [SerializeField]
    private TextMeshProUGUI _people = null;
    private const string _spriteRoot = "Sprite";
    public void Set(ConcertList.Info info, UnityAction onClick)
    {
        _title.text = info._title;
        if (Resources.Load<Sprite>(_spriteRoot + "/" + info._posterPath) != null)
            _image.sprite = Resources.Load<Sprite>(_spriteRoot + "/" + info._posterPath);
        _people.text = info._nowPeoples + "/" + info._maxPeoples;
        _btn.onClick.AddListener(onClick);
    }
    
}
