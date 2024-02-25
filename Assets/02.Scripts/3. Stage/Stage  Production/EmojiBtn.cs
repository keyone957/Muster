using Oculus.Interaction.DebugTree;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
//각 버튼의 이름에 따라서 hover하면, 버튼 idxset해주기
//추후 로직변경 
// 최초 작성자 : 홍원기
// 수정자 : 
// 최종 수정일 : 2023-12-09
public class EmojiBtn : MonoBehaviour,IPointerEnterHandler
{
    [SerializeField] int index;
    public void OnPointerEnter(PointerEventData eventData)
    {
        EmojiController.Instance.curSelectEmojiIndex = index;
    }
}
