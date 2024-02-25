using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using Newtonsoft.Json;
public class UIHSVPallet : MonoBehaviour
{
    public Slider HueSlider;
    public Slider SaturationSlider;
    public Slider ValueSlider;
    public Image PalletImage;
    public TMP_InputField InputHue;
    public TMP_InputField InputSaturation;
    public TMP_InputField InputValue;
    public TMP_InputField InputHex;
    public Button ResetButton;
    public Button AcceptButton;
    public Transform ColorPalletScollViewContent;
    public GameObject ColorPalletPrefab;
    public List<MyColor> ColorList = new List<MyColor>();
    private GameObject SelectedColor;
    // ColorList

    private string path = "Assets/Resources/UI/Seqeuence/Lines/colors.json";
    private bool select = false;
    // Start is called before the first frame update
    void Start()
    {
        HueSlider.onValueChanged.AddListener(delegate { onChangeHueSlider(); });
        SaturationSlider.onValueChanged.AddListener(delegate { onChangeSaturationSlider(); });
        ValueSlider.onValueChanged.AddListener(delegate { onChangeValueSlider(); });
        InputHue.onEndEdit.AddListener(delegate { onChangeHueInput(); });
        InputSaturation.onEndEdit.AddListener(delegate { onChangeSaturationInput(); });
        InputValue.onEndEdit.AddListener(delegate { onChangeValueInput(); });
        InputHex.onEndEdit.AddListener(delegate { onChangeHexInput(); });
        ResetButton.onClick.AddListener(delegate { Reset(); });
        AcceptButton.onClick.AddListener(delegate { Accept(); });
        if( UISequencer.instance)
        {
            UISequencer.instance.activeSprite(false);
        }
        //ColorList = JsonConvert.DeserializeObject<List<Color>>(System.IO.File.ReadAllText(path));
        ColorList = UISequencer.instance.colorList;
        for (int i = 0; i < ColorList.Count; i++)
        {
            Color color = new Color();
            color.r = ColorList[i].r;
            color.g = ColorList[i].g;
            color.b = ColorList[i].b;
            color.a = ColorList[i].a;
            AddColor(new MyColor(color));
        }
        UpdateHSVColor();
    }
    public void UpdateHSVColor()
    {
        PalletImage.color = Color.HSVToRGB(HueSlider.value / 360, SaturationSlider.value / 100, ValueSlider.value / 100);
        InputHue.text = HueSlider.value.ToString();
        InputSaturation.text = SaturationSlider.value.ToString();
        InputValue.text = ValueSlider.value.ToString();
        InputHex.text = ColorUtility.ToHtmlStringRGB(PalletImage.color);
    }

    public void onChangeHueSlider()
    {
        UpdateHSVColor();
    }
    public void onChangeSaturationSlider()
    {
        UpdateHSVColor();
    }
    public void onChangeValueSlider()
    {
        UpdateHSVColor();
    }
    public void onChangeHueInput()
    {
        HueSlider.value = int.Parse(InputHue.text);
        UpdateHSVColor();
    }
    public void onChangeSaturationInput()
    {
        SaturationSlider.value = int.Parse(InputSaturation.text);
        UpdateHSVColor();
    }
    public void onChangeValueInput()
    {
        ValueSlider.value = int.Parse(InputValue.text);
        UpdateHSVColor();
    }
    public void onChangeHexInput()
    {
        Color color;
        ColorUtility.TryParseHtmlString(InputHex.text, out color);
        PalletImage.color = color;
        Color.RGBToHSV(color, out float H, out float S, out float V);
        HueSlider.value = H;
        SaturationSlider.value = S;
        ValueSlider.value = V;
        InputHue.text = HueSlider.value.ToString();
        InputSaturation.text = SaturationSlider.value.ToString();
        InputValue.text = ValueSlider.value.ToString();
    }
    public void Reset()
    {
        HueSlider.value = 0;
        SaturationSlider.value = 100;
        ValueSlider.value = 100;
        UpdateHSVColor();
    }
    public void Accept()
    {
        if (UISequencer.instance)
        {
            UISequencer.instance.AddColor(PalletImage.color);
            UISequencer.instance.activeSprite(true);
        }
        else
        {
            Debug.Log("UISequencer.instance is null");
        }
        Destroy(gameObject);
    }
    public void onClickColorPallet(GameObject colorPallet)
    {
        SelectedColor = colorPallet;
        Color color = colorPallet.GetComponent<Image>().color;
        PalletImage.color = color;
        Color.RGBToHSV(color, out float H, out float S, out float V);
        HueSlider.value = H * 360;
        SaturationSlider.value = S * 100;
        ValueSlider.value = V * 100;
        InputHue.text = HueSlider.value.ToString();
        InputSaturation.text = SaturationSlider.value.ToString();
        InputValue.text = ValueSlider.value.ToString();
    }
    public void SetHue(float hue)
    {
        HueSlider.value = hue;
        UpdateHSVColor();
    }
    public void AddColor(MyColor newColor)
    {
        GameObject colorPallet = Instantiate(ColorPalletPrefab, ColorPalletScollViewContent);
        Color color = new Color();
        color.r = newColor.r;
        color.g = newColor.g;
        color.b = newColor.b;
        color.a = newColor.a;
        colorPallet.GetComponent<Image>().color = color;
        colorPallet.GetComponent<Button>().onClick.AddListener(delegate { onClickColorPallet(colorPallet); });
    }

}
