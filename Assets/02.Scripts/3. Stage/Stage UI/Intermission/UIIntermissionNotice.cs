using TMPro;
using UnityEngine;
using UnityEngine.UI;

// Intermission에서 Stage로 넘어갈때 발생하는 알림창
// HMD 착용여부나 공연 중임을 알려줌
// 최초 작성자 : 김기홍
// 수정자 : -
// 최종 수정일 : 2024-01-26
public class UIIntermissionNotice : UIWindow
{
    private StageSceneManager stageSceneManager;

    [SerializeField] Button _reEnterBtn;
    [SerializeField] TMP_Text _noticeText;
    private void Awake()
    {
        stageSceneManager = FindObjectOfType<StageSceneManager>();

        // Set Btn
        _reEnterBtn.gameObject.SetActive(true);
        _reEnterBtn.onClick.AddListener(OnClickReEnter);

        // Set Text
        if (InputManager.instance.IsHMDTracking())
        {
            HMDWearingSuccessNotice();
            _reEnterBtn.gameObject.SetActive(false);
        }
        else if (!InputManager.instance.IsHMDTracking())
        {
            HMDWearingFailureWarning();
        }
    }
    void OnClickReEnter()
    {
        stageSceneManager.EnterStage();
    }
    void HMDWearingSuccessNotice()
    {
        _noticeText.text = "공연을 즐기고 있습니다";
    }
    void HMDWearingFailureWarning()
    {
        _noticeText.text = "HMD를 착용해주세요";
    }
}
