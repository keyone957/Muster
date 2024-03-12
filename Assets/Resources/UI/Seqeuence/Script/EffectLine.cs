using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System;
using System.IO;
using Newtonsoft.Json;
public class EffectLine : MonoBehaviour
{
    public TMP_InputField Time;
    public TMP_Dropdown EffectMain;
    public TMP_Dropdown EffectSub;
    public TMP_Dropdown EffectTime;
    public Button PreviewButton;
    public Button DeleteButton;
    public GameObject SettingPrefab;
    public Image itemImagePrefab;
    private GameObject canvas;
    public List<MyColor> colorList = new List<MyColor>();
    private int nowMain = -1;
    private int nowSub = -1;
    private bool active = false;
    private bool creating = false;
    public int index = 0;
    private string path = "Assets/Resources/UI/Seqeuence/Lines/colors.json";
    public Light longLight;
    public List<Material> StageMaterials0 = null;
    public List<Material> StageMaterials1 = null;
    public List<GameObject> meshRenderers;
    // Start is called before the first frame update
    void Start()
    {
        meshRenderers.Add(GameObject.Find("우주정거장"));
        this.colorList = UISequencer.instance.colorList;
        canvas = GameObject.Find("Canvas");
        Time.onEndEdit.AddListener(delegate { onTimeEdited(); });
        EffectMain.onValueChanged.AddListener(delegate { onClickMainEffect(EffectMain.value); });
        EffectSub.onValueChanged.AddListener(delegate { onClickSubEffect(EffectSub.value); });
        EffectTime.onValueChanged.AddListener(delegate { onClickEffectTime(EffectTime.value); });
        PreviewButton.onClick.AddListener(delegate { onClickPreviewButton(); });
        DeleteButton.onClick.AddListener(delegate { onClickDeleteButton(); });
        EffectSub.gameObject.SetActive(true);
        EffectTime.gameObject.SetActive(true);
        PreviewButton.gameObject.SetActive(true);
    }

    public string getTime(string time)
    {
        Debug.Log(time);
        string inTime = time;
        string inputed = "";
        if (inTime.Contains("."))
        {
            string[] parts = inTime.Split('.');
            string frontPart = parts[0];
            string rearPart = parts[1];

            // 뒷부분의 길이가 2보다 작다면 뒷부분에 0을 추가하여 2자리로 만듭니다.
            if (rearPart.Length < 2)
            {
                rearPart = rearPart.PadRight(2, '0');
            }

            // 합친 문자열을 반환
            inTime = frontPart + rearPart;
        }
        Debug.Log(inTime);
        string newText = ConvertToTimeString(inTime);
        Debug.Log(newText);
        return newText;
    }
    public string ConvertToTimeString(string inTime)
    {
        int time = int.Parse(inTime);

        // 밀리초를 분, 초, 밀리초로 분리
        int milliseconds = time %100;
        int total = time/100;
        int seconds = total%60;
        int minutes = total/60;
        

        // 시간 형식으로 변환하여 반환
        return string.Format("{0:D2}:{1:D2}:{2:D2}", minutes, seconds, milliseconds);
    }
    public void onTimeEdited()
    {
        Time.text = getTime(Time.text);
    }
    public void updateSub(bool last = false)
    {
        // 색깔 숫자화
        EffectSub.ClearOptions();
        List<String> colorTextList = new List<string>();
        Color color = new Color();
        //colorList = JsonConvert.DeserializeObject<List<MyColor>>(System.IO.File.ReadAllText(path));
        colorList = JsonConvert.DeserializeObject<List<MyColor>>(ES3.Load("colorList", Define._ESFileName).ToString());
        if (colorList.Count > 0)
        {
            for (int i = 0; i < colorList.Count; i++)
            {
                color.r = colorList[i].r;
                color.g = colorList[i].g;
                color.b = colorList[i].b;
                color.a = colorList[i].a;
                string name = "";
                if (i == 0) name = "바다";
                else if (i == 1) name = "우주";
                else if (i == 2) name = "열정";
                colorTextList.Add("<color=#" + ColorUtility.ToHtmlStringRGB(color) + ">" + name + "</color>");
            }

        }
        //colorTextList.Add("직접입력");
        EffectSub.AddOptions(colorTextList);
        if (last)
        {
            EffectSub.value = colorList.Count - 1;
        }

    }
    public void onClickMainEffect(int option)
    {
        // 서브 초기화
        EffectSub.gameObject.SetActive(true);
        EffectTime.gameObject.SetActive(true);
        EffectSub.ClearOptions();

        PreviewButton.gameObject.SetActive(true);
        DeleteButton.gameObject.SetActive(true);

        nowMain = option;
        //colorList = UISequencer.instance.GetColorList();
        // 테마컬러
        if (option == 1)
        {
            UISequencer.instance.nowLine = this;
            updateSub();
        }
        // 조명 종류
        else if (option == 2)
        {
            List<String> nameList = new List<string>();
            nameList.Add("Main Light");
            nameList.Add("SpotLight");
            nameList.Add("Upper Light");
            nameList.Add("Lower Light");
            nameList.Add("Beam Light");
            nameList.Add("Long Light");
            EffectSub.AddOptions(nameList);
        }
        // 무대 특수효과
        else if (option == 3)
        {
            List<String> nameList = new List<string>();
            nameList.Add("Fire");
            nameList.Add("Fireworks");
            nameList.Add("Game");
            EffectSub.AddOptions(nameList);
        }
        // // 실내 특수 효과 
        // else if (option == 4)
        // {
        //     Debug.Log("main : " + option);
        //     Debug.Log(option);
        //     List<String> nameList = new List<string>();
        //     nameList.Add("게임");
        //     nameList.Add("폭죽");
        //     // nameList.Add("반딧불이");
        //     // nameList.Add("반짝이");
        //     EffectSub.AddOptions(nameList);
        // }
        //EffectSub.RefreshShownValue();
        EffectSub.value = -1;
        EffectTime.value = -1;
    }
    public void onClickSubEffect(int option)
    {
        EffectSub.gameObject.SetActive(true);
        EffectTime.gameObject.SetActive(true);
        nowSub = option;
        EffectTime.ClearOptions();
        List<String> optionList = new List<string>();
        optionList.Add("OFF");
        optionList.Add("ON");
        // 테마컬러
        if (nowMain == 1)
        {
            if (nowSub == colorList.Count && !creating)
            {
                Debug.Log("직접입력");
                //GameObject setting = Instantiate(SettingPrefab, UISequencer.instance.gameObject.transform.parent) as GameObject;

                // UIHSVPallet hsv = setting.GetComponent<UIHSVPallet>();
                // for(int i=0; i < colorList.Count;i++)
                //     hsv.AddColor(colorList[i]);

                //EffectSub.RefreshShownValue();
            }
        }
        // 조명 종류
        else if (nowMain == 2)
        {

        }
        // 무대 특수효과
        else if (nowMain == 3)
        {

        }
        // 실내 특수 효과
        else if (nowMain == 4)
        {

        }
        EffectTime.AddOptions(optionList);
        active = false;
        PreviewButton.gameObject.SetActive(true);
        DeleteButton.gameObject.SetActive(true);
    }
    public void onClickEffectTime(int option)
    {
        EffectTime.gameObject.SetActive(true);
        // 비활성
        if (option == 0)
        {
            active = false;
        }
        // 활성
        else if (option == 1)
        {
            active = true;
        }

        PreviewButton.gameObject.SetActive(true);
        DeleteButton.gameObject.SetActive(true);
    }
    public EffectTimeLine GetInfo()
    {
        float time = 0f;
        string[] timeText = Time.text.Split(':');
        int minute = int.Parse(timeText[0]) * 60;
        int second = int.Parse(timeText[1]);
        string times = (minute + second).ToString();
        times += "." + timeText[2];
        time = float.Parse(times);
        EffectTimeLine temp = new EffectTimeLine(time, nowMain, nowSub, active);
        return temp;
    }
    public void SetInfo(EffectTimeLine effectTimeLine)
    {
        creating = true;
        string times = (effectTimeLine.time * 100f).ToString();
        Time.text = getTime(times);

        //EffectMain.value = effectTimeLine.main;
        onClickMainEffect(effectTimeLine.main);
        EffectMain.value = effectTimeLine.main;

        //EffectSub.value = effectTimeLine.sub;
        onClickSubEffect(effectTimeLine.sub);
        EffectSub.value = effectTimeLine.sub;

        //EffectTime.value = effectTimeLine.active ? 1 : 0;
        onClickEffectTime(effectTimeLine.active ? 1 : 0);
        EffectTime.value = effectTimeLine.active ? 1 : 0;


        nowMain = effectTimeLine.main;
        nowSub = effectTimeLine.sub;
        active = effectTimeLine.active;
        EffectSub.gameObject.SetActive(true);
        EffectTime.gameObject.SetActive(true);
        PreviewButton.gameObject.SetActive(true);
        DeleteButton.gameObject.SetActive(true);
        creating = false;
    }
    // 마테리얼 변경
    public void changeMaterial(MeshRenderer meshRenderer, int Index, int group, int cnt)
    {
        Material[] materials = meshRenderer.materials;
        Material newMaterial = null;
        if (group == 0) newMaterial = StageMaterials0[cnt];
        else if (group == 1) newMaterial = StageMaterials1[cnt];

        materials[Index] = newMaterial;

        meshRenderer.materials = materials;
    }
    // preview
    public void onClickPreviewButton()
    {
        int main = nowMain;
        int sub = nowSub;
        bool act = active;
        // 테마컬러
        if (main == 1)
        {
            // 롱라이트 변경
            // Color color = new Color();
            // color.r = colorList[sub].r;
            // color.g = colorList[sub].g;
            // color.b = colorList[sub].b;
            // color.a = colorList[sub].a;
            // if (longLight != null)
            // {
            //     longLight.color = color;
            //     longLight.enabled = active;
            // }

            // // 마테리얼 변경
            // // 우주정거장
            // changeMaterial(meshRenderers[0].GetComponent<MeshRenderer>(), 1, sub, 0);
            // // 응원봉
            // //changeMaterial(meshRenderers[1].GetComponent<MeshRenderer>(), 1, sub);
            // if (sub == 0)
            // {
            //     Debug.Log("색 변경 0");

            // }
            // else if (sub == 1)
            // {
            //     Debug.Log("색 변경 1");
            // }
            // orgin 246,122,255,255
            // StageMaterial.EnableKeyword("_EMISSION");
            // Color color = new Color();
            // color.r = colorList[sub].r;
            // color.g = colorList[sub].g;
            // color.b = colorList[sub].b;
            // color.a = colorList[sub].a;
            // StageMaterial.SetColor("_EmissionColor", color);
        }
        // 조명 연출
        else if (main == 2)
        {
            if (sub == 0)
            {
                if (FindObjectOfType<LightManager>() != null) { GameObject.Find("LightManager").GetComponent<LightManager>().Server_ActiveMainLight(act); }
                Debug.Log(act ? "메인 조명 활성화" : "메인 조명 비활성화");
            }
            else if (sub == 1)
            {
                if (FindObjectOfType<LightManager>() != null) { GameObject.Find("LightManager").GetComponent<LightManager>().Server_ActiveSpotLight(act); }
                Debug.Log(act ? "스포트라이트 활성화" : "스포트라이트 비활성화");
            }
            else if (sub == 2)
            {
                if (FindObjectOfType<LightManager>() != null) { GameObject.Find("LightManager").GetComponent<LightManager>().Server_ActiveUpperLightAll(act); }
                Debug.Log(act ? "어퍼 라이트 활성화" : "어퍼 라이트 비활성화");
            }
            else if (sub == 3)
            {
                if (FindObjectOfType<LightManager>() != null) { GameObject.Find("LightManager").GetComponent<LightManager>().Server_ActiveLowerLightAll(act); }
                Debug.Log(act ? "로우 라이트 활성화" : "로우 라이트 비활성화");
            }
            else if (sub == 4)
            {
                if (FindObjectOfType<LightManager>() != null) { GameObject.Find("LightManager").GetComponent<LightManager>().Server_ActiveBeamLightAll(act); }
                Debug.Log(act ? "빔 라이트 활성화" : "빔 라이트 비활성화");
            }
            else if (sub == 5)
            {
                if (FindObjectOfType<LightManager>() != null) { GameObject.Find("LightManager").GetComponent<LightManager>().Server_ActiveLongLightAll(act); }
                Debug.Log(act ? "롱 라이트 활성화" : "롱 라이트 비활성화");
            }
        }
        //특수효과
        else if (main == 3)
        {
            if (sub == 0)
            {
                if (FindObjectOfType<FlameManager>() != null) { GameObject.Find("FlameManager").GetComponent<FlameManager>().Local_ActiveFlame(act); }
                Debug.Log(act ? "불기둥 활성화" : "불기둥 비활성화");
            }
            else if (sub == 1)
            {
                if (FindObjectOfType<FlameManager>() != null) { GameObject.Find("FlameManager").GetComponent<FlameManager>().Local_ActiveFireWork(act); }
                // NetworkFlameManager networkFlameManager = null;
                // if (networkFlameManager == null)
                //     networkFlameManager = FindObjectOfType<NetworkFlameManager>();
                // networkFlameManager.Rpc_ActivateAllFireWork(act);
                Debug.Log(act ? "폭죽 활성화" : "폭죽 비활성화");
            }
            else if (sub == 2)
            {
                Debug.Log(act ? "게임 활성화" : "게임 비활성화");
            }
            // else if (sub == 2)
            // {
            //     Debug.Log(act ? "안개 활성화" : "안개 비활성화");
            // }
            // else if (sub == 3)
            // {
            //     Debug.Log(act ? "꽃가루 활성화" : "꽃가루 비활성화");
            // }
        }
        // 실내 특수 효과
        else if (main == 4)
        {
            // if (sub == 0)
            // {
            //     Debug.Log(act ? "게임 활성화" : "게임 비활성화");
            // }
            // else if (sub == 1)
            // {
            //     NetworkFlameManager networkFlameManager = null;
            //     if (networkFlameManager == null)
            //         networkFlameManager = FindObjectOfType<NetworkFlameManager>();
            //     networkFlameManager.Rpc_ActivateAllFireWork(act);
            //     Debug.Log(act ? "폭죽 활성화" : "폭죽 비활성화");
            // }
            // if (sub == 0)
            // {
            //     Debug.Log(act ? "폭죽 활성화" : "폭죽 비활성화");
            // }
            // else if (sub == 1)
            // {
            //     Debug.Log(act ? "반딧불이 활성화" : "반딧불이 비활성화");
            // }
            // else if (sub == 2)
            // {
            //     Debug.Log(act ? "반짝이 활성화" : "반짝이 비활성화");
            // }
        }
    }
    public void onClickDeleteButton()
    {
        Destroy(this.gameObject);
        UISequencer.instance.DeleteEffectLine();
    }
    public void setIndex(int index)
    {
        this.index = index;
    }
    public bool isSetting() => nowSub != -1;

}
