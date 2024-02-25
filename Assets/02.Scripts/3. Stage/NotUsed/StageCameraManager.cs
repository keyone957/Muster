// using Cysharp.Threading.Tasks.Triggers;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
//스테이지 씬에서 여러대의 카메라 매니저 관련
//최초 작성자: 홍원기
//수정자:
//최종 수정일: 2023-11-30
public class StageCameraManager : MonoBehaviour
{
    public static StageCameraManager instance = null;
    [SerializeField]
    public GameObject _mainScreenCamera;
    [SerializeField]
    public Camera _playerCamera;

    private void Awake()
    {
        #region Singleton
        if (instance == null)
            instance = this;
        else
            Destroy(instance);
        #endregion
    }
    private void Start()
    {
        //테스트용
        //메인 스크린 카메라 비활
        _mainScreenCamera.SetActive(false);
    }
    //입장시 2d ui보여줄때 뒷배경을 메인 스크린카메라로 설정
    public void SetMainCamera()
    {
        Camera mainScreenCamera = _mainScreenCamera.GetComponent<Camera>();
        mainScreenCamera = Camera.main;
        
    }
    //연결 다되면 플레이어 vr기기의 카메라에 메인카메라 연결, vr기기 사용가능하게
    public void SetPlayerCamera()
    {
        _mainScreenCamera.SetActive(false);
        UIStageStartMenu._Instance.xrRig.SetActive(true);
        _playerCamera = Camera.main;
    }
}
