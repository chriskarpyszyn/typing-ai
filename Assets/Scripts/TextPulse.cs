using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;


public class TextPulse : MonoBehaviour
{
    // Start is called before the first frame update

    [field: SerializeField]
    public float duration { get; private set; } = 1f;

    [SerializeField]
    private float scaleMax = 1.3f;

    private Vector3 initialScale;
    ScaleTextAnimation scaleAnimation;



    private void Start()
    {
        scaleAnimation = new ScaleTextAnimation();
        initialScale = transform.localScale;
        PulseText();
    }

    private void PulseText()
    {
        scaleAnimation.ScaleAnimation(this.gameObject, initialScale * scaleMax, duration);
        scaleAnimation.ScaleAnimation(this.gameObject, initialScale, duration);
    }
}
