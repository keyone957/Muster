using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using DG.Tweening;

public class FadeOverlay : MonoBehaviour
{
    [SerializeField] private Image _sprite = null;
    private Sequence _fadeSequence;
    private void Awake()
    {
        DontDestroyOnLoad(this);
        _fadeSequence = DOTween.Sequence();
    }

    /// <summary>
    /// 어두워지기
    /// </summary>
    public void DoFadeOut(float duration)
    {
        StopFade();
        gameObject.SetActive(true);

        if (duration != 0.0f)
        {
            _fadeSequence.Append(_sprite.DOFade(1, duration));
        }
        else
        {
            Color color = _sprite.color;
            color.a = 1.0f;
            _sprite.color = color;
        }
    }

    /// <summary>
    /// 밝아지기
    /// </summary>
    public void DoFadeIn(float duration)
    {
        StopFade();

        if (duration != 0.0f)
        {
            gameObject.SetActive(true);
            _fadeSequence.Append(_sprite.DOFade(0, duration));
        }
        else
        {
            gameObject.SetActive(false);
        }
    }

    private void StopFade()
    {
        if (_fadeSequence != null)
        {
            _fadeSequence.Kill();
            _fadeSequence = null;
        }
    }

    public bool IsFading()
    {
        return (_fadeSequence != null);
    }
}