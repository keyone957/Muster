using UnityEngine;
using UnityEngine.EventSystems;

public class RotationAvatar : MonoBehaviour
{
    public GameObject _avatar;
    public float rotationSpeed = 5.0f;
    public float zoomSpeed = 2.0f;
    private bool isMouseButtonDown = false;



    public float zoomSpeed1 = 0.1f;
    public float minScale = 0.5f;
    public float maxScale = 2f;




    void Start()
    {
        // "_avatar" 변수에 "marimari" 오브젝트 할당
        _avatar = GameObject.Find("marichan");
        if (_avatar == null)
        {
            Debug.LogError("marimari 오브젝트를 찾을 수 없습니다.");
        }
    }

    void Update()
    {
        // "_avatar" 오브젝트가 할당되었는지 확인
        if (_avatar == null)
        {
            return;
        }

        // 마우스 왼쪽 버튼이 눌려있는 동안에만 회전을 계산
        if (Input.GetMouseButtonDown(0))
        {
            isMouseButtonDown = true;
        }
        else if (Input.GetMouseButtonUp(0))
        {
            isMouseButtonDown = false;
        }

        if (isMouseButtonDown)
        {
            // 마우스 입력을 받아 회전을 계산
            float mouseX = Input.GetAxis("Mouse X");

            // 오브젝트의 회전을 계산하여 적용 (Y축 회전만 적용)
            Quaternion newRotation = Quaternion.Euler(0f, mouseX * rotationSpeed, 0f);
            _avatar.transform.rotation *= newRotation;
        }

         float scroll = Input.GetAxis("Mouse ScrollWheel");
        if (scroll != 0f)
        {
            // 이미지의 현재 스케일을 가져옵니다.
            Vector3 currentScale = transform.localScale;

            // 스크롤 방향에 따라 스케일을 조절합니다.
            float newScale = Mathf.Clamp(currentScale.x +scroll * zoomSpeed1, minScale, maxScale);

            // 새로운 스케일을 이미지에 적용합니다.
            transform.localScale = new Vector3(newScale, newScale, newScale);
        }
       
    }
}
