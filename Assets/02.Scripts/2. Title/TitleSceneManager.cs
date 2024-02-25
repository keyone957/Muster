using Cysharp.Threading.Tasks;
using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class TitleSceneManager : MonoBehaviour
{
    [SerializeField] Image[] vrLogos;
    [SerializeField] Image[] pcLogos;
    private async void Awake()
    {
        // Set Logo
        foreach (var vrLogo in vrLogos)
        {
            vrLogo.color = new Color(1, 1, 1, 0);
        }
        foreach (var pcLogo in pcLogos)
        {
            pcLogo.color = new Color(1, 1, 1, 0);
        }

        // Show Logo 
        await ShowLogo();

        // Load Start Scene
        await SceneLoader._instance.LoadScene(SceneName.Start);
    }
    async UniTask ShowLogo()
    {
        bool pcShow = true;
        bool vrShow = true;
        Sequence pcShowLogoSequence = DOTween.Sequence();
        foreach (var pcLogo in pcLogos)
        {
            pcShowLogoSequence
                .Append(pcLogo.DOFade(1, 1))
                .Append(pcLogo.DOFade(0, 1));
        }

        Sequence vrShowLogoSequence = DOTween.Sequence();
        foreach (var vrLogo in vrLogos)
        {
            vrShowLogoSequence
                .Append(vrLogo.DOFade(1, 1))
                .Append(vrLogo.DOFade(0, 1));
        }

        pcShowLogoSequence.OnComplete(() =>
        {
            foreach (var pcLogo in pcLogos)
            {
                pcLogo.gameObject.SetActive(false);
                pcShow = false;
            }
        });

        vrShowLogoSequence.OnComplete(() =>
        {
            foreach (var vrLogo in vrLogos)
            {
                vrLogo.gameObject.SetActive(false);
                vrShow = false;
            }
        });

        await UniTask.WaitUntil(() => !(pcShow | vrShow));
    }
}
