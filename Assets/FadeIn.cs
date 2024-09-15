using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FadeIn : MonoBehaviour
{
    [SerializeField] private float timeBeforeFadeIn = 30f;
    [SerializeField] private float fadeInDuration = 1f;

    // Start is called before the first frame update
    void Start()
    {
        Sequence seq = DOTween.Sequence();
        Image image = GetComponent<Image>();

        seq.PrependInterval(timeBeforeFadeIn)
            .Append(image.DOFade(0f, fadeInDuration));
        ;
    }

}
