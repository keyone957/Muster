using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System;
public class CircleHuePicker : MonoBehaviour
{
    public Image huePickerImage;
    public Image colorPreview;
    public UIHSVPallet uIHSVPallet;

    private RectTransform pickerRectTransform;

    void Start()
    {
        pickerRectTransform = huePickerImage.rectTransform;

        // Register event for drag input on the hue picker image
        var eventTrigger = huePickerImage.gameObject.AddComponent<EventTrigger>();
        EventTrigger.Entry entry = new EventTrigger.Entry { eventID = EventTriggerType.Drag };
        entry.callback.AddListener((data) => { OnDrag((PointerEventData)data); });
        eventTrigger.triggers.Add(entry);
        
        // Initialize color on start
        UpdateColor(0);
    }

    void OnDrag(PointerEventData eventData)
    {
        // Get the position of the drag relative to the hue picker image
        Vector2 localPosition;
        RectTransformUtility.ScreenPointToLocalPointInRectangle(pickerRectTransform, eventData.position, eventData.pressEventCamera, out localPosition);

        // Calculate the angle (hue) based on the position
        float angle = Mathf.Atan2(localPosition.y, localPosition.x) * Mathf.Rad2Deg;
        angle -= 90f;
        // Ensure the angle is positive
        if (angle < 0)
        {
            angle += 360f;
        }
        angle = Math.Abs(angle -360);
        // Normalize the angle to a range of 0 to 1
        float normalizedHue = angle / 360f;

        // Update color preview
        UpdateColor(normalizedHue);
    }

    void UpdateColor(float normalizedHue)
    {
        // Convert normalized hue to RGB
        Color selectedColor = Color.HSVToRGB(normalizedHue, 1f, 1f);
        uIHSVPallet.SetHue(normalizedHue*360);
        // Update color preview
    }
}
