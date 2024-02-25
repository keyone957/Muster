using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
//버튼 이벤트 연결
//최초 작성자: 홍원기
//수정자:
//최종 수정일: 2023-12-04
public class UIAudiencePanel : UIWindow
{
    [SerializeField]
    private Button _exitBtn;
    [SerializeField]
    private Button _selectSeatBtn;
    [SerializeField]
    private Button _optionBtn;
    [SerializeField]
    private TextMeshProUGUI _quest2Connect;
    [SerializeField]
    private TextMeshProUGUI _HMDMounted;
    [SerializeField]
    public GameObject _seatImage;
    void Start()
    {
        _selectSeatBtn.onClick.AddListener(OnClickSeatSelectImage);
    }

    private void SetText(TextMeshProUGUI textObj, string text)
    { 
        textObj.text = text;
    }
    private void CheckDeviceState()
    {
        bool isConnectDevice = InputManager.instance.IsDeviceConnceted();
        bool isHMDTracking = InputManager.instance.IsHMDTracking();
        if (isConnectDevice)
        {
            SetText(_quest2Connect, "퀘스트2 연결 완료");
        }
        if (isHMDTracking)
        {
            SetText(_HMDMounted, "VR 헤드셋 착용 완료");
        }
    }
    private void OnClickSeatSelectImage()
    {

        if (_seatImage.activeSelf)
        {
            _seatImage.SetActive(false);
        }
        else
        {
            _seatImage.SetActive(true);
        }
    }
}
