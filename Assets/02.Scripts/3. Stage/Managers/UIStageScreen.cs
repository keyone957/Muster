using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;

//UIStageScreen 무대의 UI에 부여 되며, 자식들을 관리
//최초 작성자: 김효중
//수정자:
//최종 수정일: 2023-11-07
public class UIStageScreen : UIWindow
{
    [SerializeField]
    private TextMeshProUGUI _midText = null;
    [SerializeField]
    private TextMeshProUGUI _subText = null;
    [SerializeField]
    private Slider _slider = null;
    private bool FirstLineMid = true;
    private bool FirstLineSub = true;
    public void SetMidText(string txt)
    {
        if (_midText != null)
            _midText.text = txt;
    }
    
    public void SetSubText(string txt)
    {
        if (_subText != null)
            _subText.text = txt;
    }
    public void AddLineMain(string txt)
    {
        if (_midText != null)
        {
             if (FirstLineMid )
            {
                _midText.text = txt;
                FirstLineMid = false;
            }
            else
            {
                _midText.text = _midText.text + "\r\n" + txt ;  
            }
           
        }
       
    }
    public void AddLineSub(string txt)
    {
        if (_subText != null)
        {
            if (FirstLineSub && _subText != null)
            {
                _subText.text = txt;
                FirstLineSub = false;
            } 
            else
            {
                _subText.text = _subText.text + "\r\n" + txt ;    
            }
        }
    }
    
    public void SetSlider(float now, float max)
    {
        if (_slider != null)
        {
            _slider.value = now/max;
        }
    }
    public void ActivateSlider(bool activate)
    {
        if (_slider != null)
        {
            _slider.gameObject.SetActive(activate);
        }
    }
    
    public void clear()
    {
        if (_midText != null)
            _midText.text = "";
        if (_subText != null)
            _subText.text = "";
            FirstLineMid = true;
            FirstLineSub = true;
    }
}