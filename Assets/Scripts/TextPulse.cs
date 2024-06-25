using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

[RequireComponent(typeof(TextMeshProUGUI))]
public class TextPulse : MonoBehaviour
{
    // Start is called before the first frame update

    [field: SerializeField]
    public float duration { get; private set; } = 1f;

    [SerializeField]
    private float scaleMax = 1.3f;

    private Vector3 initialScale;

    private TextMeshProUGUI textComponent;


    private void Start()
    {
        initialScale = transform.localScale;
        textComponent = GetComponent<TextMeshProUGUI>();
        StartCoroutine(PulseText());

    }

    IEnumerator PulseText()
    {
        while (true)
        {
            yield return ScaleText(initialScale, initialScale * scaleMax);
            yield return ScaleText(initialScale * scaleMax, initialScale);
        }
    }

    IEnumerator ScaleText(Vector3 startScale, Vector3 endScale)
    {
        float elapsedTime = 0f;
        while (elapsedTime < duration)
        {
            transform.localScale = Vector3.Lerp(startScale, endScale, elapsedTime / duration);
            elapsedTime += Time.deltaTime;
            yield return null;
        }
        transform.localScale = endScale;
    }
}
