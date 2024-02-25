
using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;
using Newtonsoft.Json;
using System.IO;
public class LineEventPlayer : MonoBehaviour
{
    static public LineEventPlayer instance = null;
    public Animator animator;
    public Animation animationComponent;
    public AnimationClip animationClip;
    public Light longLight;
    public Light longLight2;
    public List<Material> StageMaterials0 = null;
    public List<Material> StageMaterials1 = null;
    public List<GameObject> meshRenderers;
    private List<MyColor> colorList;
    private string path = "Assets/Resources/UI/Seqeuence/Lines/";
    private string sPath = "UI/Seqeuence/Lines/";
    //private string VideoList = "";
    string Cname = "";
    private List<EffectTimeLine> effectLines = new List<EffectTimeLine>(); // 이팩트 리스트
    private string TestName = "Asteroids Gameplay";
    // Start is called before the first frame update
    public GameObject IdolSideL;
    public GameObject IdolSideR;
    public GameObject subScreen01;
    public GameObject subScreen02;
    void Start()
    {
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(this);
        }
        animator.SetTrigger("Stop");
        meshRenderers.Add(GameObject.Find("우주정거장"));
        //effectLines = JsonConvert.DeserializeObject<List<EffectTimeLine>>(System.IO.File.ReadAllText(path + Cname + ".json"));
        //animationComponent = GetComponent<Animation>();
        //animationClip = animationComponent.clip;

        // 애니메이션 클립의 모든 이벤트를 삭제합니다.
        //ClearAllAnimationEvents();

        // 애니메이션을 재생합니다.
        //animationComponent.Play();
    }

    void ClearAllAnimationEvents(string ClipName)
    {
        Cname = ClipName;
        animationClip = LoadAnimationClipByName(Cname);
        if (animationClip == null)
        {
            Debug.LogWarning("AnimationClip is null.");
            AnimationClip newAnimationClip = new AnimationClip();
            //newAnimationClip.legacy = true; // 레거시 X
            newAnimationClip.wrapMode = WrapMode.Once;
            // AssetDatabase.CreateAsset(newAnimationClip, path + Cname + ".anim");
            // AssetDatabase.SaveAssets();
            // AssetDatabase.Refresh();
            // animationClip = LoadAnimationClipByName(Cname);
            animationClip = newAnimationClip;
            animationComponent.AddClip(animationClip, Cname);
        }
        else
        {
            Debug.Log(animationClip.name + "가 있습니다.");
        }
        AnimationEvent[] animationEvents = animationClip.events;
        if (animationEvents.Length > 0)
        {
            var list = animationEvents.ToList();
            list.RemoveAt(0);
            //animationClip.events = list.ToArray();
            animationClip.events = new AnimationEvent[0];
            Debug.Log(animationClip.name + "의 모든 이벤트가 삭제되었습니다.");
        }
        //animationComponent.clip = animationClip;
    }
    void LogEmpty()
    {
        Debug.LogError("함수 이름이 설정 되지 않았습니다.");
    }
    // 애니메이션 재생
    public void startAni(string VideoName, float normalizedTime)
    {

        string VName = VideoName;
        if (VName == "") VName = TestName;
        //animationComponent.clip = animationClip;
        //animator.SetInteger("PlayClip",0);
        //animator.Play(animationClip.name,0,frame);
        Debug.Log("normal: " + normalizedTime + " - " + VName);
        animator.Play(VName, 0, normalizedTime);

        // 노래 재생 - Temp
        MediaManager.Instance.Server_PlayMusic(normalizedTime);

        // Control Dummies
        AudienceDummyController.instance.Server_DoCheering();
        AudienceDummyController.instance.Server_LookTarget(true);

        Debug.Log("Tdldldqkf");

        //animationComponent.Play();
        //animationComponent.Sample();

        //카메라 스크린
        MediaManager.Instance.Server_PlayerVideo();

    }
    public void StartAniForTest(string VideoName, float normalizedTime)
    {
        string VName = VideoName;
        if (VName == "") VName = TestName;

        Debug.Log("normal: " + normalizedTime + " - " + VName);
        animator.Play(VName, 0, normalizedTime);
    }
    public void stopAni()
    {
        Debug.Log("STOP");
        //animationComponent.Stop();
        animator.SetInteger("PlayClip", -1);
        animator.SetTrigger("Stop");

        // Particle 종료
        FlameManager._instance.Server_StopAll();
        LightManager.Instance.Server_SetInit();

        // Control Dummies
        AudienceDummyController.instance.Server_DoIdle();
        AudienceDummyController.instance.Server_LookTarget(false);

        // 노래 종료
        MediaManager.Instance.Server_StopMusic();
        MediaManager.Instance.Server_StopVideo();


    }
    public void StopAniForTest()
    {
        Debug.Log("STOP In Sequencer");
        //animationComponent.Stop();
        animator.SetInteger("PlayClip", -1);
        animator.SetTrigger("Stop");

        // Particle 종료
        FlameManager._instance.Local_StopAll();
        LightManager.Instance.Local_SetInit();

        // Control Dummies
        AudienceDummyController.instance.Local_DoIdle();
        AudienceDummyController.instance.LookTarget(false);

        // 노래 종료
        MediaManager.Instance.Local_StopMusic();
        FindObjectOfType<MyAudioPlayer>().StopAudio();




    }
    // 파일 로드, 애니메이션 초기화 후 이벤트 등록
    public void SetLine()
    {
        Debug.Log("SetLine");
        //string name = animationClip.name;
        //colorList = JsonConvert.DeserializeObject<List<MyColor>>(System.IO.File.ReadAllText(path + "colors.json"));
        if (ES3.FileExists(Define._ESFileName))
        {
            if (ES3.KeyExists("effectLines"))
            {
                Debug.Log("have KEY");
                effectLines = JsonConvert.DeserializeObject<List<EffectTimeLine>>(ES3.Load("effectLines", Define._ESFileName).ToString());
                Debug.Log(effectLines.Count + "Line Detected");
            }
            else
            {
                Debug.Log("NO KEY");
                effectLines.Clear();
                string jsonString = JsonConvert.SerializeObject(effectLines, Formatting.Indented);
                ES3.Save("effectLines", jsonString.ToString());
            }

            if (ES3.KeyExists("colorList"))
            {
                // load color
                colorList = JsonConvert.DeserializeObject<List<MyColor>>(ES3.Load("colorList", Define._ESFileName).ToString());
                Debug.Log("find color file : " + colorList.ToString());
            }
        }
        else
        {
            Debug.Log("NO FILE");
            effectLines.Clear();
            string jsonString = JsonConvert.SerializeObject(effectLines, Formatting.Indented);
            ES3.Save("effectLines", jsonString.ToString());
            colorList.Clear();
            string colorJson = JsonConvert.SerializeObject(colorList, Formatting.Indented);
            Debug.Log("colorJson: " + colorJson);
            ES3.Save("colorList", colorJson);
        }/*
        if (UISequencer.instance)
            Cname = UISequencer.instance.VideoController.videoPlayer.clip.name;*/
        if (Cname == null || Cname == "") Cname = TestName;

        //Json받기
        //effectLines = JsonConvert.DeserializeObject<List<EffectTimeLine>>(System.IO.File.ReadAllText(path + Cname + ".json"));
        ClearAllAnimationEvents(Cname);
        for (int i = 0; i < effectLines.Count; i++)
        {
            AddEvent(effectLines[i].time, effectLines[i].main, effectLines[i].sub, effectLines[i].active);
        }
        // 맨 마지막에 추가
        // AddEvent((float)(UISequencer.instance.VideoController.videoPlayer.clip.frameCount / UISequencer.instance.VideoController.videoPlayer.clip.frameRate), 0, 1, false);
    }

    // 이벤트 등록 (시간 00.00, 대분류, 소분류, 활성화여부)
    public void AddEvent(float time, int main, int sub, bool act)
    {
        // 애니메이션 클립에 이벤트를 추가
        AnimationEvent animationEvent = new AnimationEvent();
        animationEvent.time = time;
        // 대기 상태로 전환
        if (main == 0) { animationEvent.functionName = "stopAni"; animationClip.AddEvent(animationEvent); return; }
        animationEvent.functionName = "LogEmpty"; // 함수 이름을 변경 할 것!
        animationEvent.intParameter = sub;
        // 테마컬러
        if (main == 1)
        {
            if (act)
                animationEvent.functionName = "onMaterial";
            else
                animationEvent.functionName = "offMaterial";
        }
        // 조명 종류
        else if (main == 2)
        {
            if (act)
                animationEvent.functionName = "onLight";
            else
                animationEvent.functionName = "offLight";
        }
        // 무대 특수효과
        else if (main == 3)
        {
            if (act)
                animationEvent.functionName = "onStageEffect";
            else
                animationEvent.functionName = "offStageEffect";
        }
        // 실내 특수 효과
        else if (main == 4)
        {
            if (act)
                animationEvent.functionName = "onEnvEffect";
            else
                animationEvent.functionName = "offEnvEffect";

        }
        animationClip.AddEvent(animationEvent);
        Debug.Log("ADD");
    }
    // 이벤트 호출: 마테리얼 색상, 발광여부 변경
    void onMaterial(int index)
    {
        // colorList = JsonConvert.DeserializeObject<List<MyColor>>(System.IO.File.ReadAllText(path + "colors.json"));
        // // 롱라이트 변경
        // Color color = new Color();
        // color.r = colorList[index].r;
        // color.g = colorList[index].g;
        // color.b = colorList[index].b;
        // color.a = colorList[index].a;
        // if (longLight != null)
        // {
        //     longLight.color = color;
        //     longLight.enabled = true;
        // }

        // // 마테리얼 변경
        // // 우주정거장
        // changeMaterial(meshRenderers[0].GetComponent<MeshRenderer>(), 1, index, 0);
        // // 응원봉
        // //changeMaterial(meshRenderers[1].GetComponent<MeshRenderer>(), 1, sub);
        // if (index == 0)
        // {
        //     Debug.Log("색 변경 0");

        // }
        // else if (index == 1)
        // {
        //     Debug.Log("색 변경 1");
        // }
    }
    // 이벤트 호출: 마테리얼 색상, 발광여부 변경
    void offMaterial(int index)
    {
        // colorList = JsonConvert.DeserializeObject<List<MyColor>>(System.IO.File.ReadAllText(path + "colors.json"));
        // // 롱라이트 변경
        // Color color = new Color();
        // color.r = colorList[index].r;
        // color.g = colorList[index].g;
        // color.b = colorList[index].b;
        // color.a = colorList[index].a;
        // if (longLight != null)
        // {
        //     longLight.color = color;
        //     longLight.enabled = false;
        // }

        // // 마테리얼 변경
        // // 우주정거장
        // changeMaterial(meshRenderers[0].GetComponent<MeshRenderer>(), 1, index, 0);
        // // 응원봉
        // //changeMaterial(meshRenderers[1].GetComponent<MeshRenderer>(), 1, sub);
        // if (index == 0)
        // {
        //     Debug.Log("색 변경 0");

        // }
        // else if (index == 1)
        // {
        //     Debug.Log("색 변경 1");
        // }

        // 이전버전
        // if (StageMaterial != null)
        // {
        //     Debug.Log("material is not null : " + colorList[index].r + " " + colorList[index].g + " " + colorList[index].b + " ");
        //     Color color = new Color();
        //     color.r = colorList[index].r;
        //     color.g = colorList[index].g;
        //     color.b = colorList[index].b;
        //     color.a = colorList[index].a;
        //     StageMaterial.EnableKeyword("_EMISSION");

        //     StageMaterial.SetColor("_EmissionColor", color);
        //     StageMaterial.DisableKeyword("_EMISSION");


        // }
        // else
        // {
        //     Debug.Log("material is ----  null");
        // }
    }

    // 이벤트 호출: 조명 활성화, 종류
    void onLight(int sub)
    {
        if (LightManager.Instance == null) return;

        bool act = true;
        if (sub == 0)
        {
            LightManager.Instance.Server_ActiveMainLight(act);
            Debug.Log(act ? "메인 조명 활성화" : "메인 조명 비활성화");
        }
        else if (sub == 1)
        {
            LightManager.Instance.Server_ActiveSpotLight(act);
            Debug.Log(act ? "스포트라이트 활성화" : "스포트라이트 비활성화");
        }
        else if (sub == 2)
        {
            LightManager.Instance.Server_ActiveUpperLightAll(act);
            Debug.Log(act ? "어퍼 라이트 활성화" : "어퍼 라이트 비활성화");
        }
        else if (sub == 3)
        {
            LightManager.Instance.Server_ActiveLowerLightAll(act);
            Debug.Log(act ? "로우 라이트 활성화" : "로우 라이트 비활성화");
        }
        else if (sub == 4)
        {
            LightManager.Instance.Server_ActiveBeamLightAll(act);
            Debug.Log(act ? "빔 라이트 활성화" : "빔 라이트 비활성화");
        }
        else if (sub == 5)
        {
            LightManager.Instance.Server_ActiveLongLightAll(act);
            Debug.Log(act ? "롱 라이트 활성화" : "롱 라이트 비활성화");
        }
    }
    // 이벤트 호출: 조명 활성화, 종류
    void offLight(int sub)
    {
        if (LightManager.Instance == null) return;

        bool act = false;
        if (sub == 0)
        {
            LightManager.Instance.Server_ActiveMainLight(act);
            Debug.Log(act ? "메인 조명 활성화" : "메인 조명 비활성화");
        }
        else if (sub == 1)
        {
            LightManager.Instance.Server_ActiveSpotLight(act);
            Debug.Log(act ? "스포트라이트 활성화" : "스포트라이트 비활성화");
        }
        else if (sub == 2)
        {
            LightManager.Instance.Server_ActiveUpperLightAll(act);
            Debug.Log(act ? "어퍼 라이트 활성화" : "어퍼 라이트 비활성화");
        }
        else if (sub == 3)
        {
            LightManager.Instance.Server_ActiveLowerLightAll(act);
            Debug.Log(act ? "로우 라이트 활성화" : "로우 라이트 비활성화");
        }
        else if (sub == 4)
        {
            LightManager.Instance.Server_ActiveBeamLightAll(act);
            Debug.Log(act ? "빔 라이트 활성화" : "빔 라이트 비활성화");
        }
        else if (sub == 5)
        {
            LightManager.Instance.Server_ActiveLongLightAll(act);
            Debug.Log(act ? "롱 라이트 활성화" : "롱 라이트 비활성화");
        }
    }

    // 이벤트 호출 : 무대 특수 효과 설정
    void onStageEffect(int sub)
    {
        bool act = true;
        if (sub == 0)
        {
            if (FlameManager._instance != null) FlameManager._instance.Server_ActiveFlame(act);
            Debug.Log(act ? "불기둥 활성화" : "불기둥 비활성화");
        }
        else if (sub == 1)
        {
            if (FlameManager._instance) FlameManager._instance.Server_ActiveFirework(act);
            // NetworkFlameManager networkFlameManager = null;
            // if (networkFlameManager == null)
            //     networkFlameManager = FindObjectOfType<NetworkFlameManager>();
            // networkFlameManager.Rpc_ActivateAllFireWork(act);
            Debug.Log(act ? "폭죽 활성화" : "폭죽 비활성화");
        }
        else if (sub == 2)
        {
            if (GamificationManager.instance) GamificationManager.instance.StartGamification<Gamification_Crazy>();
            Debug.Log(act ? "게임 활성화" : "게임 비활성화");
        }
    }
    // 이벤트 호출 : 무대 특수 효과 설정
    void offStageEffect(int sub)
    {
        bool act = false;
        if (sub == 0)
        {
            if (FlameManager._instance) FlameManager._instance.Server_ActiveFlame(act);
            Debug.Log(act ? "불기둥 활성화" : "불기둥 비활성화");
        }
        else if (sub == 1)
        {
            if (FlameManager._instance) FlameManager._instance.Server_ActiveFirework(act);
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
    }

    // 이벤트 호출 : 실내 효과 설정
    void onEnvEffect(int sub)
    {
        // bool act = true;
        // if (sub == 0)
        // {
        //     GamificationManager.instance.StartGamification<Gamification_Crazy>();
        //     Debug.Log("게임 활성화");
        // }
        // else if (sub == 1)
        // {
        //     if (FindObjectOfType<FlameManager>() != null) { GameObject.Find("FlameManager").GetComponent<FlameManager>().Server_ActiveFirework(act); }
        //     // NetworkFlameManager networkFlameManager = null;
        //     // if (networkFlameManager == null)
        //     //     networkFlameManager = FindObjectOfType<NetworkFlameManager>();
        //     // networkFlameManager.Rpc_ActivateAllFireWork(act);
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
    // 이벤트 호출 : 실내 효과 설정
    void offEnvEffect(int sub)
    {
        // bool act = false;
        // if (sub == 0)
        // {
        //     GamificationManager.instance.StartGamification<Gamification_Crazy>();
        //     Debug.Log("게임 활성화");
        // }
        // else if (sub == 1)
        // {
        //     if (FindObjectOfType<FlameManager>() != null) { GameObject.Find("FlameManager").GetComponent<FlameManager>().Server_ActiveFirework(act); }
        //     // NetworkFlameManager networkFlameManager = null;
        //     // if (networkFlameManager == null)
        //     //     if (networkFlameManager == null)
        //     //         networkFlameManager = FindObjectOfType<NetworkFlameManager>();
        //     // networkFlameManager.Rpc_ActivateAllFireWork(act);
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
    private AnimationClip LoadAnimationClipByName(string clipName)
    {
        AnimationClip video = Resources.Load<AnimationClip>(sPath + clipName);
        return video;
        // // 폴더 내의 모든 에셋 가져오기
        // string[] assets = AssetDatabase.FindAssets("t:AnimationClip", new string[] { folderPath });

        // foreach (var assetGUID in assets)
        // {
        //     // GUID를 통해 실제 에셋의 경로 가져오기
        //     string assetPath = AssetDatabase.GUIDToAssetPath(assetGUID);

        //     // 경로로부터 애니메이션 클립 가져오기
        //     AnimationClip NewAnimationClip = AssetDatabase.LoadAssetAtPath<AnimationClip>(assetPath);

        //     // 애니메이션 클립의 이름이 목표와 일치하는지 확인
        //     if (NewAnimationClip != null && NewAnimationClip.name == clipName)
        //     {
        //         return NewAnimationClip;
        //     }
        // }
        // //AnimationClip animationClip = AssetDatabase.LoadAssetAtPath<AnimationClip>(path + clipName + ".json");
        // return null; // 해당 이름의 애니메이션 클립이 없을 경우 null 반환
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
}


