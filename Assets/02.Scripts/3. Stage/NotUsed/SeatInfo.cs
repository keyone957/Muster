using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
//좌석 버튼들의 상태값 확인
//최초 작성자: 홍원기
//수정자:
//최종 수정일: 2023-12-04
public class SeatInfo : MonoBehaviour
{
    [SerializeField] AudienceManager playerManager;

    public Vector3 _playerSeatTrans;
    public bool _isSelect = false;

    private void Awake()
    {
        playerManager = FindObjectOfType<AudienceManager>();
    }
    void Start()
    {
        Button btn = GetComponent<Button>();
        btn.onClick.AddListener(OnClickSeatBtn);
    }
    private void OnClickSeatBtn()
    {
        if (!_isSelect && !playerManager._isPlayerSelectSeat)
        {
            _isSelect = true;
            playerManager._isPlayerSelectSeat = true;
            SetBtnColor(1.0f);
            // playerManager.SetPlayerTrans(new Vector3(6.4f, 1.89f, -22f));
        }
        else if (_isSelect && playerManager._isPlayerSelectSeat)
        {
            _isSelect = false;
            playerManager._isPlayerSelectSeat = false;
            SetBtnColor(0.0f);
        }
    }
    private void SetBtnColor(float alpha)
    {
        Image seat = GetComponent<Image>();
        Color color = seat.color;
        color.a = alpha;
        seat.color = color;
    }
}
