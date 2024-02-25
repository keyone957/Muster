using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;

public class UIConcertList : UIWindow
{
    private const string _spriteRoot = "Sprite";
    [SerializeField]
    private RectTransform _contents = null;
    [SerializeField] 
    Button _btnClose = null;
    [SerializeField]
    private GameObject _itemPrefab = null;
    [SerializeField]
    private Button _btnOK = null;
    [SerializeField]
    private Image _image = null;
    [SerializeField]
    private TextMeshProUGUI _title = null;
    [SerializeField]
    private TextMeshProUGUI _name = null;
    [SerializeField]
    private TextMeshProUGUI _people = null;
    [SerializeField]
    private TextMeshProUGUI _time = null;
    void Awake()
    {
        InitializeItems();
        _btnClose.onClick.AddListener(OnClickClose);
        _btnOK.onClick.AddListener(OnClickOK);
    }
    public override bool OnKeyInput()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            OnClickClose();
        }

        return true;
    }
    private void InitializeItems()
    {
        TextAsset loadedText = Resources.Load<TextAsset>(Define._concertListPath);
        ConcertList concertList = JsonUtility.FromJson<ConcertList>(loadedText.text);

        for (int i = 0; i < concertList._infos.Count; ++i)
        {
            AddItem(concertList._infos[i]);
        }
    }
    private void AddItem(ConcertList.Info info)
    {
        UIConsertItem itemUI = UIManager.CreateWidget<UIConsertItem>(_itemPrefab, _contents);
        itemUI.Set(info
            , delegate { OnItemSelected(info); }
            );
    }
    public void OnClickClose()
    {
        //TitleSystem._Instance._UIManager.CloseConcertList();
    }
     private void OnClickOK()
    {
        Debug.Log("OK");
        // TODO: 선택 적용
        SceneManager.LoadScene("StageScene");
    }
    private void OnItemSelected(ConcertList.Info info)
    {
        _title.text = info._title;
        _name.text = info._name;
        _time.text = info._starTime;
         _people.text = info._nowPeoples + "/" + info._maxPeoples;
        if (Resources.Load<Sprite>(_spriteRoot + "/" + info._posterPath) != null)
            _image.sprite = Resources.Load<Sprite>(_spriteRoot + "/" + info._posterPath);
       
    }
}
