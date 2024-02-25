using System.Collections;
using System.Collections.Generic;
using UnityEngine;
//버튼 생성
//최초 작성자: 홍원기
//수정자:
//최종 수정일: 2023-12-04
public class UIStageSeat : MonoBehaviour
{
    [SerializeField]
    private List<GameObject> SeatGroup;
    [SerializeField]
    private GameObject uiSeatBtnPrefab;

    void Start()
    {
        InitSeatBtn();
    }
    private void InitSeatBtn()
    {
        CloneBtn(1, 10, SeatGroup[0].transform);
        CloneBtn(11, 20, SeatGroup[1].transform);
        CloneBtn(21, 40, SeatGroup[2].transform);
        CloneBtn(41, 60, SeatGroup[3].transform);
        CloneBtn(61, 80, SeatGroup[4].transform);
        CloneBtn(81, 100, SeatGroup[5].transform);

    }
    private void CloneBtn(int startNum, int endNum, Transform parentGroup)
    {
        for (int i = startNum; i <= endNum; i++)
        {
            GameObject seat = Instantiate(uiSeatBtnPrefab, parentGroup);
            seat.name = i.ToString();
        }
    }

}
