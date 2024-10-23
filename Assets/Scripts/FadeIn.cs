using DG.Tweening;
using System;
using UnityEngine;
using UnityEngine.UI;

public class FadeIn : MonoBehaviour
{
    [SerializeField] private float fadeInDelay = 30f;
    [SerializeField] private float fadeInDuration = 1f;

    private Image image;
    private Tween fadeTween;

    public event Action EventOnFadeInComplete;


    // Start is called before the first frame update
    void Start()
    {
        image = GetComponent<Image>();
        image.color = new Color(image.color.r, image.color.g, image.color.b, 1);
        fadeTween = image.DOFade(0f, fadeInDuration).SetDelay(fadeInDelay).OnComplete(OnFadeInComplete);
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            Debug.Log("Mouse Click Kill");
            fadeTween.Kill();
            image.DOFade(0f, fadeInDuration).OnComplete(OnFadeInComplete);
        }
    }

    private void OnFadeInComplete()
    {
        EventOnFadeInComplete?.Invoke();
    }
}