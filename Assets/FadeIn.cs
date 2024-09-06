using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FadeIn : MonoBehaviour
{
    [SerializeField] private float fadeInDelay = 30f;
    [SerializeField] private float fadeInDuration = 1f;

    private Image image;
    private Tween fadeTween;


    // Start is called before the first frame update
    void Start()
    {
        image = GetComponent<Image>();
        fadeTween = image.DOFade(0f, fadeInDuration).SetDelay(fadeInDelay);
    }

    private void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            fadeTween.Kill();
            image.DOFade(0f, fadeInDuration);
        }
    }

}