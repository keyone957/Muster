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
        // "_avatar" ������ "marimari" ������Ʈ �Ҵ�
        _avatar = GameObject.Find("marichan");
        if (_avatar == null)
        {
            Debug.LogError("marimari ������Ʈ�� ã�� �� �����ϴ�.");
        }
    }

    void Update()
    {
        // "_avatar" ������Ʈ�� �Ҵ�Ǿ����� Ȯ��
        if (_avatar == null)
        {
            return;
        }

        // ���콺 ���� ��ư�� �����ִ� ���ȿ��� ȸ���� ���
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
            // ���콺 �Է��� �޾� ȸ���� ���
            float mouseX = Input.GetAxis("Mouse X");

            // ������Ʈ�� ȸ���� ����Ͽ� ���� (Y�� ȸ���� ����)
            Quaternion newRotation = Quaternion.Euler(0f, mouseX * rotationSpeed, 0f);
            _avatar.transform.rotation *= newRotation;
        }

         float scroll = Input.GetAxis("Mouse ScrollWheel");
        if (scroll != 0f)
        {
            // �̹����� ���� �������� �����ɴϴ�.
            Vector3 currentScale = transform.localScale;

            // ��ũ�� ���⿡ ���� �������� �����մϴ�.
            float newScale = Mathf.Clamp(currentScale.x +scroll * zoomSpeed1, minScale, maxScale);

            // ���ο� �������� �̹����� �����մϴ�.
            transform.localScale = new Vector3(newScale, newScale, newScale);
        }
       
    }
}
