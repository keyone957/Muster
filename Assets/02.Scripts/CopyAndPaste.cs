using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
public class CopyAndPast : MonoBehaviour
{
    public TMP_InputField copyField;
    public TMP_InputField pasteField;
    public void CopyToClipbord()
    {
        // 입력창에 있는 텍스트를 클립보드에 바로 복사
        TextEditor textEditor = new TextEditor();
        textEditor.text = copyField.text;
        textEditor.SelectAll();
        textEditor.Copy();
    }
    public void PasteFromClipboard()
    {
        //클립보드에 저장 된 내용을 바로 붙여넣기
        TextEditor textEditor = new TextEditor();
        textEditor.multiline = true;
        textEditor.Paste(); Debug.Log(textEditor.text);
        pasteField.text =textEditor.text;
    }
    
}
