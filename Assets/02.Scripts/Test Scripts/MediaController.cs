using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.UI;

// 메인스크린 동영상 페이드인 페이드아웃
// 최초 작성자 : 홍원기
// 수정자 : -
// 최종 수정일 : 2024-02-19
public class MediaController : MonoBehaviour
{
    public static MediaController instance { get; private set; }
    [SerializeField] public RawImage rawImage;
    [SerializeField] public GameObject mainScreen;
    private Color originalColor;


    private void Awake()
    {
        if (instance == null)
            instance = this;
        else
            Destroy(instance);

    }
    private void Start()
    {
        originalColor = rawImage.color;
    }
    public IEnumerator FadeOutCoroutine(float fadeOutTime)
    {
        float elapsed = 0f;
        Color originalColor = rawImage.color;

        while (elapsed < fadeOutTime)
        {
            elapsed += Time.deltaTime;
            float alpha = Mathf.Lerp(1, 0, elapsed / fadeOutTime);
            rawImage.color = new Color(originalColor.r, originalColor.g, originalColor.b, alpha);

            yield return null;
        }
        mainScreen.gameObject.SetActive(true);
    }
   public IEnumerator FadeInCoroutine(float fadeInTime)
    {
        float elapsed = 0f;
        Color originalColor = rawImage.color;
        mainScreen.gameObject.SetActive(false);
        while (elapsed < fadeInTime)
        {
            elapsed += Time.deltaTime;
            float alpha = Mathf.Lerp(0, 1, elapsed / fadeInTime);
            rawImage.color = new Color(originalColor.r, originalColor.g, originalColor.b, alpha);

            yield return null;
        }
    }
}
