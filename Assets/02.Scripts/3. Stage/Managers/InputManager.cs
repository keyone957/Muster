using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR;
using UniRx;
using Cysharp.Threading.Tasks;

//버튼 이벤트 관련 함수 수정
//최초 작성자: 홍원기
//수정자 : 김기홍
//최종 수정일: 2024-02-04

// 최종 수정 내용
// - Get Device Pos 함수의 접근 제한자 변경 (private -> public)
public class InputManager : MonoBehaviour
{
    public static InputManager instance { get; private set; }

    // 장치
    public InputDevice _rightController;
    public InputDevice _leftController;
    public InputDevice _HMD;

    // 장치 연결 확인
    private bool isHmdMounted;

    public delegate bool ControllerCondition(InputDevice _Controller);
    //TODO : 모코피 연결 상태에 따른 상태값 필요
    //싱글톤
    private void Awake()
    {
        if (instance == null)
            instance = this;
        else
            Destroy(instance);

    }
    private void Start()
    {
        OVRManager.HMDMounted += HandleHMDMounted;
        OVRManager.HMDUnmounted += HandleHMDUnmounted;
    }
    void HandleHMDMounted()
    {
        isHmdMounted = true;
    }

    void HandleHMDUnmounted()
    {
        isHmdMounted = false;
    }

    // 현재 유저가 HMD를 끼고 있는지 안끼고 있는지 확인
    public bool IsHMDTracking()
    {
        return isHmdMounted;
    }
    // 현재 기기가 연결되어있는지 확인 
    public bool IsDeviceConnceted()
    {
        //TODO : 기기연결 확인
        return true;
    }

    //각 기기가 없으면 기기 찾아서 초기화 해주기
    private void InitInputDevice()
    {

        if (!_rightController.isValid)
        {
            InitInputDevice(InputDeviceCharacteristics.Controller | InputDeviceCharacteristics.Right, ref _rightController);
        }
        if (!_leftController.isValid)
        {
            InitInputDevice(InputDeviceCharacteristics.Controller | InputDeviceCharacteristics.Left, ref _leftController);
        }
        if (!_HMD.isValid)
        {
            InitInputDevice(InputDeviceCharacteristics.HeadMounted, ref _HMD);
        }

    }

    //각 컨트롤러의 역할별로 디바이스 가져오기 (특정 역할을 하는 컨트롤러 가져오기)
    private void InitInputDevice(InputDeviceCharacteristics inputCharacteristics, ref InputDevice inputDevice)
    {
        List<InputDevice> devices = new List<InputDevice>();
        InputDevices.GetDevicesWithCharacteristics(inputCharacteristics, devices);
        if (devices.Count > 0)
        {
            inputDevice = devices[0];
        }
    }

    //각 InpuDevice별로 각각 pos값 return
    private Vector3 GetDevicePos(InputDevice device)
    {
        InitInputDevice();
        Vector3 pos;
        device.TryGetFeatureValue(CommonUsages.devicePosition, out pos);
        return pos;
    }

    //왼쪽컨트롤러 x버튼 누르면 true값 반환 ==>x버튼 누르면 게임시작
    //public bool GetDeviceBtn(InputDevice device)
    //{
    //    if (!_rightController.isValid || !_leftController.isValid || !_HMD.isValid)
    //    {
    //        InitInputDevice();
    //    }
    //    bool isBtnClicked;
    //    if (device.TryGetFeatureValue(CommonUsages.primaryButton, out isBtnClicked) && isBtnClicked)
    //    {
    //        return isBtnClicked;
    //    }
    //    return isBtnClicked;
    //}

    //어느쪽 컨트롤러와 어떤 버튼의 input을 체크할건지 확인하는 함수
    //CommonUsages.primaryButton=>각 컨트롤러의 아래버튼
    //CommonUsages.secondaryButton=>각 컨트롤러의 위버튼
    //CommonUsages.triggerButton=>트리거 버튼
    public bool GetDeviceBtn(InputDevice device, InputFeatureUsage<bool> button)
    {
        InitInputDevice();

        bool isBtnClicked;
        if (device.TryGetFeatureValue(button, out isBtnClicked) && isBtnClicked)
        {
            return isBtnClicked;
        }
        return isBtnClicked;
    }
    public bool GetDeviceBtnSec(InputDevice device)
    {
        if (!_rightController.isValid || !_leftController.isValid || !_HMD.isValid)
        {
            InitInputDevice();
        }
        bool isBtnClicked;
        if (device.TryGetFeatureValue(CommonUsages.secondaryButton, out isBtnClicked) && isBtnClicked)
        {
            return isBtnClicked;
        }
        return isBtnClicked;
    }

    //Get Controller Speed
    public IObservable<float> GetDeviceSpeed(InputDevice device, ControllerCondition condition)
    {
        return Observable.Create<float>(observer =>
        {
            var positionStream = Observable.EveryUpdate()
                .Select(_ => GetDevicePos(device))
                .Buffer(2)
                .Where(buffer => buffer.Count == 2)
                .Select(buffer => CalculateSpeed(buffer[0], buffer[1]))
                .Subscribe(speed =>
                    observer.OnNext(speed),
                    observer.OnError,
                    () => observer.OnCompleted())
                    .AddTo(this);

            return Disposable.Empty;
        });
    }
    public IObservable<float> GetDeviceSpeed(InputDevice device, float duration, ControllerCondition condition)
    {
        return Observable.Create<float>(observer =>
        {
            var startTime = DateTime.Now;

            var positionStream = Observable.EveryUpdate()
                .Select(_ => GetDevicePos(device))
                .TakeWhile(_ => (DateTime.Now - startTime).TotalSeconds <= duration)
                .Buffer(2, 1)
                .Where(buffer => buffer.Count == 2)
                .Select(buffer => CalculateSpeed(buffer[0], buffer[1]))
                .Subscribe(speed =>
                    {
                        observer.OnNext(speed);
                    },
                    observer.OnError,
                    () => observer.OnCompleted()
                    )
                    .AddTo(this);

            return Disposable.Empty;
        });
    }

    //Get Controller Velocity
    public IObservable<Vector3> GetDeviceVelocity(InputDevice device, float duration, ControllerCondition condition)
    {
        return Observable.Create<Vector3>(observer =>
        {
            var startTime = DateTime.Now;

            var positionStream = Observable.EveryUpdate()
                .Select(_ => GetDevicePos(device))
                .TakeWhile(_ => (DateTime.Now - startTime).TotalSeconds <= duration)
                .Buffer(2, 1)
                .Where(buffer => buffer.Count == 2)
                .Select(buffer => buffer[1] - buffer[0])
                    .Subscribe(velocity =>
                    {
                        observer.OnNext(velocity);
                    },
                    observer.OnError,
                    () => observer.OnCompleted()
                    )
                    .AddTo(this);

            return Disposable.Empty;
        });
    }

    private float CalculateSpeed(Vector3 pos1, Vector3 pos2)
    {
        return Vector3.Distance(pos1, pos2) / Time.deltaTime;
    }

    public bool IsHandOverHMD(InputDevice device)
    {
        var ret = GetDevicePos(device).y > GetDevicePos(_HMD).y;
        return ret;
    }
}