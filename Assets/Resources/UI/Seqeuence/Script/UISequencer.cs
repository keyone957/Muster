using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System;
using System.IO;
using Newtonsoft.Json;
using OVRSimpleJSON;
// 효과 저장용 클래스
public class EffectTimeLine
{
    public float time { get; set; }
    public int main { get; set; }
    public int sub { get; set; }
    public bool active { get; set; }
    public EffectTimeLine(float time, int main, int sub, bool active)
    {
        this.time = time;
        this.main = main;
        this.sub = sub;
        this.active = active;
    }
}
// 색깔 저장용 클래스
[System.Serializable]
public class MyColor
{
    // 직렬화에 포함되길 원하는 프로퍼티만 남겨두기
    public float r;
    public float g;
    public float b;
    public float a;
    public MyColor(Color color)
    {
        r = color.r;
        g = color.g;
        b = color.b;
        a = color.a;
    }
}
public class UISequencer : UIWindow
{
    public static UISequencer instance;
    public Transform EffectList;
    public GameObject AddButtonPrefab;
    public GameObject EffectLinePrefab;
    public MyVideoPlayer VideoController;
    public MyAudioPlayer AudioController;
    public Button ExitButton;
    public Button SaveButton;
    public Button CreateButton;
    // 색상 리스트
    public List<MyColor> colorList = null;
    public GameObject ColorColorSettingPrefab;
    public List<GameObject> lines = new List<GameObject>();
    public List<EffectTimeLine> effectLines = new List<EffectTimeLine>(); // 이팩트 리스트

    public TMP_Dropdown urls;
    public List<string> urlList = new List<string>();
    public GameObject loading;
    public EffectLine nowLine;
    public GameObject targetCamera = null;
    public RawImage targetImage;
    public TextMeshProUGUI errText;
    private RenderTexture renderTexture;

    private string path = "Assets/Resources/UI/Seqeuence/Lines/";
    private string colorFileName = "colors";
    private JsonSerializerSettings settings;
    private GameObject sprites = null;

    Button AddButton;
    // Start is called before the first frame update
    private void Awake()
    {
        instance = this;
    }
    void Start()
    {
        setColorList();
        GameObject addButton = Instantiate(AddButtonPrefab, EffectList);
        //gameObject.transform.position = new Vector3(gameObject.transform.position.x, gameObject.transform.position.y, 0);
        AddButton = addButton.GetComponent<Button>();
        AddButton.onClick.AddListener(OnAddButtonClicked);
        CreateButton.onClick.AddListener(loadFile);
        ExitButton.onClick.AddListener(OnExitButtonClicked);
        SaveButton.onClick.AddListener(saveFile);
        // urls.onValueChanged.AddListener(OnVideoSelected);
        this.settings = new JsonSerializerSettings
        {
            ReferenceLoopHandling = ReferenceLoopHandling.Ignore
        };
        // load colorList;
        //colorList = JsonConvert.DeserializeObject<List<MyColor>>(System.IO.File.ReadAllText(path + colorFileName + ".json"));

        if (ES3.FileExists(Define._ESFileName) && ES3.KeyExists("colorList"))
        {
            colorList = JsonConvert.DeserializeObject<List<MyColor>>(ES3.Load("colorList", Define._ESFileName).ToString());
        }
        else
        {
            colorList.Clear();
            // 초기 기본 컬러 추가
            // Color newColor = Color.red;
            // MyColor myColor = new MyColor(newColor);
            // colorList.Add(myColor);
            setColorList();
            string jsonString = JsonConvert.SerializeObject(colorList, Formatting.Indented, settings);
            ES3.Save("colorList", jsonString.ToString());
        }

        renderTexture = new RenderTexture(Screen.width, Screen.height, 24);
        if (targetCamera == null) { targetCamera = GameObject.Find("PreviewCamera"); }
        if (targetCamera != null)
        {
            targetCamera.GetComponent<Camera>().enabled = true;
            targetCamera.GetComponent<Camera>().targetTexture = renderTexture; // 카메라가 렌더링한 결과를 이 RenderTexture로 지정
            targetImage.texture = renderTexture;
        }
    }
    public void OnAddButtonClicked()
    {
        GameObject effectLine = Instantiate(EffectLinePrefab, EffectList);
        effectLine.GetComponent<EffectLine>().setIndex(lines.Count);
        lines.Add(effectLine);
        AddButton.transform.SetParent(this.transform);
        AddButton.transform.SetParent(EffectList);
    }
    public void OnExitButtonClicked()
    {
        //GameObject cam = GameObject.Find("PC UI Cam");
        //if(cam != null)
        //cam.SetActive(false);
        targetCamera.GetComponent<Camera>().enabled = false;
        //Destroy(this.gameObject);
        Vector3 newPos = this._RectTrans.transform.position;
        newPos.z += 10;
        this._RectTrans.transform.position = newPos;
        LineEventPlayer linePlayer = GameObject.FindAnyObjectByType<LineEventPlayer>();
        linePlayer.StopAniForTest();
    }
    public void OnVideoSelected(int option)
    {
        VideoController.setClip(option);
        loadFile();
    }
    public void AddColor(Color color)
    {
        var settings = new JsonSerializerSettings
        {
            ReferenceLoopHandling = ReferenceLoopHandling.Ignore
        };
        if (ES3.FileExists(Define._ESFileName))
        {
            // load color
            colorList = JsonConvert.DeserializeObject<List<MyColor>>(ES3.Load("colorList", Define._ESFileName).ToString());
            Debug.Log("find color file : " + colorList.ToString());
            colorList.Add(new MyColor(color));
            // save color
            string jsonString = JsonConvert.SerializeObject(colorList, Formatting.Indented, settings);
            ES3.Save("colorList", jsonString.ToString());
        }

        if (nowLine != null)
        {
            Debug.Log("update Line");
            nowLine.updateSub(true);
        }

    }
    public void DeleteEffectLine()
    {
        Debug.Log("DeleteEffectLine");
    }

    // 완성된 라인으로 조합 생성
    public void RefreshTimeLine()
    {
        effectLines.Clear();
        for (int i = 0; i < EffectList.childCount - 1; i++)
        {
            if ((EffectList.GetChild(i).GetComponent<EffectLine>() != null) && EffectList.GetChild(i).GetComponent<EffectLine>().isSetting())
            {
                EffectTimeLine temp = EffectList.GetChild(i).GetComponent<EffectLine>().GetInfo();
                lines[i] = EffectList.GetChild(i).gameObject;
                effectLines.Add(temp);
            }
        }
        string jsonString = JsonConvert.SerializeObject(effectLines, Formatting.Indented);
        ES3.Save("effectLines", jsonString.ToString());
        LineEventPlayer linePlayer = GameObject.FindAnyObjectByType<LineEventPlayer>();
        linePlayer.SetLine();
    }
    public List<MyColor> setColorList()
    {
        colorList.Clear();
        Color color = new Color(29 / 255f, 124 / 255f, 255 / 255f, 1); MyColor myColor = new MyColor(color); // 파랑 바다
        Color color2 = new Color(106 / 255f, 62 / 255f, 179 / 255f, 1); MyColor myColor2 = new MyColor(color2); // 보라 우주
        Color color3 = new Color(255 / 255f, 0, 0, 1); MyColor myColor3 = new MyColor(color3); // 빨강 열정
        colorList.Add(myColor);
        colorList.Add(myColor2);
        colorList.Add(myColor3);
        return colorList;
    }
    public void ShowEffectList()
    {
        for (int i = 0; i < effectLines.Count; i++)
        {
            Debug.Log("ShowEffectList[" + i + "] : " + effectLines[i].time + " : " + effectLines[i].main + " : " + effectLines[i].sub + " : " + effectLines[i].active);
        }
    }
    public void setClip(List<String> clips, bool reset = false)
    {
        urls.options.Clear();
        urlList = clips;
        for (int i = 0; i < clips.Count; i++)
        {
            urls.options.Add(new TMP_Dropdown.OptionData(clips[i]));
        }
        loadFile();
        // if (reset){ urls.value = -1;Debug.Log("reset");}
        // else {Debug.Log("NOT reset");}
        urls.RefreshShownValue();
    }
    public void setEffectLines()
    {
        Debug.Log(effectLines.Count + "Line Detected");
        for (int i = 0; i < EffectList.childCount - 1; i++)
        {
            Destroy(EffectList.GetChild(i).gameObject);
        }
        for (int i = 0; i < effectLines.Count; i++)
        {
            GameObject effectLine = Instantiate(EffectLinePrefab, EffectList);
            effectLine.GetComponent<EffectLine>().setIndex(i);
            effectLine.GetComponent<EffectLine>().SetInfo(effectLines[i]);

            lines.Add(effectLine);
            Debug.Log("ADDED");
        }
        if (AddButton != null)
        {
            AddButton.transform.SetParent(this.transform);
            AddButton.transform.SetParent(EffectList);
        }
        Debug.Log("effect set");
    }
    public void saveFile()
    {
        Debug.Log("save start");
        RefreshTimeLine();
        loading.SetActive(true);
        // if (VideoController.getClipName() != "")
        // {
        //save Effect Line
        string jsonString = JsonConvert.SerializeObject(effectLines, Formatting.Indented, settings);
        Debug.Log("jsonString" + jsonString);
        //System.IO.File.WriteAllText(path + name + ".json", jsonString);
        ES3.Save("effectLines", jsonString);

        // save colorList
        // if (File.Exists(path + colorFileName + ".json"))
        // {
        //     try
        //     {
        //         Debug.Log("find color file");
        //         string colorJson = JsonConvert.SerializeObject(colorList, Formatting.Indented, settings);
        //         System.IO.File.WriteAllText(path + colorFileName + ".json", colorJson);
        //     }
        //     catch (Exception e)
        //     {
        //         Debug.Log(e.ToString());
        //     }


        // }
        // else Debug.Log("no color file");
        // Debug.Log("save end");
        // loading.SetActive(false);
        string colorJson = JsonConvert.SerializeObject(colorList, Formatting.Indented, settings);
        Debug.Log("colorJson: " + colorJson);
        ES3.Save("colorList", colorJson);
        loading.SetActive(false);
        UIActiveMenu.instrance.onSet();
        // }
    }
    public void loadFile()
    {
        Debug.Log("load start");
        loading.SetActive(true);
        for (int i = 0; i < EffectList.childCount - 1; i++)
        {
            Destroy(EffectList.GetChild(i).gameObject);
        }
        // if (VideoController.getClipName() != "")
        // {
        //Debug.Log("trt load : " + VideoController.getClipName() + ".json");
        // 파일 있으면 불러오기

        // if (File.Exists(path + VideoController.getClipName() + ".json"))
        // {
        //     effectLines.Clear();
        //     effectLines = JsonConvert.DeserializeObject<List<EffectTimeLine>>(System.IO.File.ReadAllText(path + VideoController.getClipName() + ".json"));
        //     while (EffectList == null)
        //     {
        //         Debug.Log("EffectList is null");
        //     }
        //     Debug.Log("find file");
        //     setEffectLines();
        // }
        //if ()
        //{
        if (ES3.FileExists(Define._ESFileName))
        {
            if (ES3.KeyExists("effectLines"))
            {
                Debug.Log("have KEY");
                effectLines = JsonConvert.DeserializeObject<List<EffectTimeLine>>(ES3.Load("effectLines", Define._ESFileName).ToString());
                Debug.Log(effectLines.Count + "Line Detected");
                setEffectLines();
            }
            else
            {
                Debug.Log("NO KEY");
                effectLines.Clear();
                string jsonString = JsonConvert.SerializeObject(effectLines, Formatting.Indented, settings);
                ES3.Save("effectLines", jsonString.ToString());
            }
        }
        else
        {
            Debug.Log("NO FILE");
            effectLines.Clear();
            string jsonString = JsonConvert.SerializeObject(effectLines, Formatting.Indented, settings);
            ES3.Save("effectLines", jsonString.ToString());
        }
        //}

        // 파일 없으면 빈 파일 만들기
        // else
        // {
        //     Debug.Log("no file");
        //     // create Empty file
        //     var settings = new JsonSerializerSettings
        //     {
        //         ReferenceLoopHandling = ReferenceLoopHandling.Ignore
        //     };
        //     effectLines.Clear();
        //     List<EffectTimeLine> temp = new List<EffectTimeLine>();
        //     string jsonString = JsonConvert.SerializeObject(effectLines, Formatting.Indented, settings);
        //     System.IO.File.WriteAllText(path + VideoController.getClipName() + ".json", jsonString);
        // }
        loading.SetActive(false);
        // }
        //else errText.text = "Load ERROR";

        Debug.Log("load end");
    }
    public void activeSprite(bool act)
    {
        if (sprites == null)
            sprites = GameObject.Find("SpriteParent");
        sprites.SetActive(act);
    }
    void OnDestroy()
    {
        // 사용이 끝난 RenderTexture를 해제
        if (renderTexture != null)
            renderTexture.Release();
    }

}

