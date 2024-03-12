using Cysharp.Threading.Tasks;
using DG.Tweening;
using System;
using UniRx;
using UniRx.Triggers;
using UnityEngine;
using VLB;

// 공연장의 Light를 관리하는 스크립트
// 최초 작성자 : 김기홍
// 수정자 : -
// 최종 수정일 : 20204-02-23
public class LightManager : MonoBehaviour
{
    public static LightManager Instance { get; private set; }
    private NetworkLightManager networkedManager;

    [SerializeField] MeshRenderer _mainScreen; // Screen의 Material을 조작하기 위해
    [SerializeField] MeshRenderer _subScreen1; // Screen의 Material을 조작하기 위해
    [SerializeField] MeshRenderer _subScreen2; // Screen의 Material을 조작하기 위해

    [SerializeField] Light _mainLight;
    [SerializeField] VolumetricLightBeamSD[] _spotLights;
    [SerializeField] VolumetricLightBeamSD[] _upperLight;
    [SerializeField] VolumetricLightBeamSD[] _lowerLight;
    [SerializeField] VolumetricLightBeamSD[] _beamLight;
    [SerializeField] VolumetricLightBeamSD[] _longLight;

    IDisposable _spotLightDisposable;

    private void Awake()
    {
        if (Instance == null)
            Instance = this;
        else
            Destroy(this.gameObject);
    }
    private void Start()
    {
        networkedManager = FindObjectOfType<NetworkLightManager>();
    }

    //////////////////////////////////////////
    // Server에서 호출하는 함수
    #region SERVER FUNCTION
    /// <summary> Server(=Idol)만 호출가능한 함수 </summary>
    public void Server_ActiveMainLight(bool active, float duration = 0.2f)
    {
        if (networkedManager == null)
            networkedManager = FindObjectOfType<NetworkLightManager>();
        networkedManager.Rpc_ActiveMainLight(active, duration);
    }
    /// <summary> Server(=Idol)만 호출가능한 함수 </summary>
    public void Server_ActiveSpotLight(bool active, float duration = 0.2f)
    {
        if (networkedManager == null)
            networkedManager = FindObjectOfType<NetworkLightManager>();
        networkedManager.Rpc_ActiveSpotLight(active, duration);
    }
    /// <summary> Server(=Idol)만 호출가능한 함수 </summary>
    public void Server_ActiveUpperLight(int index, bool active, float duration = 0.2f)
    {
        if (networkedManager == null)
            networkedManager = FindObjectOfType<NetworkLightManager>();
        networkedManager.Rpc_ActiveUpperLight(index, active, duration);
    }
    /// <summary> Server(=Idol)만 호출가능한 함수 </summary>
    public void Server_ActiveUpperLightAll(bool active, float duration = 0.2f)
    {
        if (networkedManager == null)
            networkedManager = FindObjectOfType<NetworkLightManager>();
        networkedManager.Rpc_ActiveUpperLightAll(active, duration);
    }
    /// <summary> Server(=Idol)만 호출가능한 함수 </summary>
    public void Server_ActiveLowerLight(int index, bool active, float duration = 0.2f)
    {
        if (networkedManager == null)
            networkedManager = FindObjectOfType<NetworkLightManager>();
        networkedManager.Rpc_ActiveLowerLight(index, active, duration);
    }
    /// <summary> Server(=Idol)만 호출가능한 함수 </summary>
    public void Server_ActiveLowerLightAll(bool active, float duration = 0.2f)
    {
        if (networkedManager == null)
            networkedManager = FindObjectOfType<NetworkLightManager>();
        networkedManager.Rpc_ActiveLowerLightAll(active, duration);
    }
    /// <summary> Server(=Idol)만 호출가능한 함수 </summary>
    public void Server_ActiveBeamLightAll(bool active, float duration = 0.2f)
    {
        if (networkedManager == null)
            networkedManager = FindObjectOfType<NetworkLightManager>();
        networkedManager.Rpc_ActiveBeamLight(active, duration);
    }
    /// <summary> Server(=Idol)만 호출가능한 함수 </summary>
    public void Server_ActiveLongLightAll(bool active, float duration = 0.2f)
    {
        if (networkedManager == null)
            networkedManager = FindObjectOfType<NetworkLightManager>();
        networkedManager.Rpc_ActiveLongLightAll(active, duration);
    }
    public void Server_SetInit()
    {
        if (networkedManager == null)
            networkedManager = FindObjectOfType<NetworkLightManager>();
        networkedManager.Rpc_SetInit();
    }
    #endregion

    //////////////////////////////////////////
    // Localr에서 호출하는 함수
    #region LOCAL FUNCTION
    public void Local_ActiveMainLight(bool active, float duration = 0.1f)
    {
        _mainLight.intensity = active ? 0.1f : 1.5f; // Init Intensity
        var targetIntensity = active ? 1.5f : 0.1f;
        _mainLight.DOIntensity(targetIntensity, duration);

    }
    public async void Local_ActiveScreenMaterial(bool active, float duration)
    {
        // Screen
        _mainScreen.gameObject.SetActive(true);
        _subScreen1.gameObject.SetActive(true);
        _subScreen2.gameObject.SetActive(true);

        _mainScreen.material.SetFloat("_FinalPower", active ? 0 : 1); // Init Value
        _subScreen1.materials[1].SetFloat("_FinalPower", active ? 0 : 1); // Init Value
        _subScreen2.materials[1].SetFloat("_FinalPower", active ? 0 : 1); // Init Value

        var targetValue = active ? 1 : 0;

        _mainScreen.material.DOFloat(targetValue, "_FinalPower", duration);
        _subScreen1.materials[1].DOFloat(targetValue, "_FinalPower", duration);
        var tween = _subScreen2.materials[1].DOFloat(targetValue, "_FinalPower", duration);

        await UniTask.WaitUntil(() => !tween.IsActive());
        _mainScreen.gameObject.SetActive(active);
        _subScreen1.gameObject.SetActive(active);
        _subScreen2.gameObject.SetActive(active);
    }
    /// <summary>
    /// 
    /// </summary>
    /// <param name="active"></param>
    /// <param name="duration"></param> 빛이 켜지는 시간
    public async void Local_ActiveSpotLight(bool active, float duration = 0.1f)
    {
        DG.Tweening.Tween tween = null;
        // Active Light
        foreach (var spot in _spotLights)
        {
            spot.enabled = true;
            var targetIntensity = active ? 1 : 0f;

            //light.DOIntensity(targetIntensity, duration);
            if (active) FadeIn(spot, targetIntensity, duration);
            else FadeOut(spot, targetIntensity, duration);
        }
        await UniTask.WaitUntil(() => !tween.IsActive());

        // Follow Target
        if (active)
        {
            var target = NetworkDataManager.GetNetworkObject(NetworkDataManager.IdolRef).transform;
            _spotLightDisposable = this.UpdateAsObservable().Subscribe(_ =>
            {
                foreach (var spot in _spotLights)
                {
                    if (target.transform.GetChild(0).transform.position.y < 90f)
                    {
                        spot.transform.LookAt(target);
                    }
                }
            });
        }
        else
        {
            _spotLightDisposable?.Dispose();
            foreach (var spot in _spotLights)
            {
                //spot.enabled = active;
            }
        }
    }
    public async void Local_ActiveUpperLight(int index, bool active, float duration = 0.1f)
    {
        _upperLight[index].enabled = true;

        //_upperLight[index].intensity = active ? 0f : 1000f; // Init Intensity
        var targetIntensity = active ? 1000f : 0f;

        //var tween = _upperLight[index].DOIntensity(targetIntensity, duration);
        //await UniTask.WaitUntil(() => !tween.IsActive());
        _upperLight[index].enabled = active;
    }
    public async void Local_ActiveUpperLightAll(bool active, float duration = 0.1f)
    {
        DG.Tweening.Tween tween = null;
        foreach (var light in _upperLight)
        {
            light.enabled = true;

            var targetIntensity = active ? 1 : 0f;

            //light.DOIntensity(targetIntensity, duration);
            if (active) FadeIn(light, targetIntensity, duration);
            else FadeOut(light, targetIntensity, duration);
        }
        await UniTask.WaitUntil(() => !tween.IsActive());
        foreach (var light in _upperLight)
        {
            //light.enabled = active;
        }
    }
    public async void Local_ActiveLowerLight(int index, bool active, float duration = 0.1f)
    {
        _lowerLight[index].enabled = true;

        //_lowerLight[index].intensity = active ? 0f : 1000f; // Init Intensity
        var targetIntensity = active ? 1000f : 0f;

        //var tween = _lowerLight[index].DOIntensity(targetIntensity, duration);
        //await UniTask.WaitUntil(() => !tween.IsActive());
        _lowerLight[index].enabled = active;
    }
    public async void Local_ActiveLowerLightAll(bool active, float duration = 0.1f)
    {
        Debug.Log("Local_ActiveLowerLightAll");
        DG.Tweening.Tween tween = null;
        foreach (var light in _lowerLight)
        {
            light.enabled = true;

            light.intensityGlobal = active ? 0f : 1; // Init Intensity
            var targetIntensity = active ? 1 : 0f;

            //light.DOIntensity(targetIntensity, duration);
            if (active) FadeIn(light, targetIntensity, duration);
            else FadeOut(light, targetIntensity, duration);
        }
        await UniTask.WaitUntil(() => !tween.IsActive());
        foreach (var light in _lowerLight)
        {
            //light.enabled = active;
        }
    }
    public async void Local_ActiveBeamLightAll(bool active, float duration = 0.1f)
    {
        DG.Tweening.Tween tween = null;
        foreach (var light in _beamLight)
        {
            light.enabled = true;
            var targetIntensity = active ? 1 : 0f;

            //light.DOIntensity(targetIntensity, duration);
            if (active) FadeIn(light, targetIntensity, 1);
            else FadeOut(light, targetIntensity, 1);
        }
        await UniTask.WaitUntil(() => !tween.IsActive());
        foreach (var light in _beamLight)
        {
            light.enabled = active;
        }
    }
    public async void Local_ActiveLongLightAll(bool active, float duration = 0.1f)
    {
        DG.Tweening.Tween tween = null;
        foreach (var light in _longLight)
        {
            light.enabled = true;

            //light.intensityGlobal = active ? 0f : 2f; // Init Intensity
            var targetIntensity = active ? 1f : 0f;
            DG.Tweening.Sequence intensitySequence;
            //light.DOIntensity(targetIntensity, duration);
            if (active) intensitySequence = FadeIn(light, targetIntensity, duration);
            else intensitySequence = FadeOut(light, targetIntensity, duration);
            intensitySequence.Play();
        }
        await UniTask.WaitUntil(() => !tween.IsActive());
        foreach (var light in _longLight)
        {
            //light.enabled = active;
        }
    }
    public void Local_SetInit()
    {
        Local_ActiveMainLight(true, 0);
        Local_ActiveSpotLight(false, 0);
        Local_ActiveUpperLightAll(false, 0);
        Local_ActiveLowerLightAll(false, 0);
        Local_ActiveBeamLightAll(false, 0);
        Local_ActiveLongLightAll(false, 0);
    }
    public static DG.Tweening.Sequence FadeIn(VolumetricLightBeamSD beam, float target, float duration = 0.1f)
    {
        Debug.Log("IN");
        Light light = new Light();
        // Tween 생성
        DG.Tweening.Sequence sequence = DOTween.Sequence();

        // TweenerCore를 시퀀스에 추가
        sequence.Append(DOTween.To(
            () => beam.intensityGlobal, // 시작값
            x => beam.intensityGlobal = x, // 값을 설정하는 람다 함수
            target, // 목표값
            duration // 지속 시간
        ));
        return sequence;
    }
    public static DG.Tweening.Sequence FadeOut(VolumetricLightBeamSD beam, float target, float duration = 0.1f)
    {
        Debug.Log("Out");
        // 새로운 시퀀스 생성
        DG.Tweening.Sequence sequence = DOTween.Sequence();

        // TweenerCore를 시퀀스에 추가
        sequence.Append(DOTween.To(
            () => beam.intensityGlobal, // 시작값
            x => beam.intensityGlobal = x, // 값을 설정하는 람다 함수
            target, // 목표값
            duration // 지속 시간
        ));

        return sequence;
    }
    #endregion
}
