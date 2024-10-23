using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

public class StarSparkleAnimation : MonoBehaviour
{
    public bool isPos = true;
    private Sequence seq;
    // Start is called before the first frame update
    void Start()
    {

        float fullOpacityBoundary = 0.3f;
        float fullOpacityRange = Random.Range(-fullOpacityBoundary, fullOpacityBoundary);

        Image image = GetComponent<Image>();
        Color currCol = image.color;
        Color noOpacity = new Color(currCol.r, currCol.g, currCol.b, 0);
        Color fullOpacity = new Color(currCol.r, currCol.g, currCol.b, 0.5f+fullOpacityRange);

        float time = 0.11f;

        seq = DOTween.Sequence();
        seq.SetLoops(0);

        if (isPos)
        {
            seq.Append(image.DOColor(noOpacity, time))
                .Append(image.DOColor(currCol, time))
                .Append(image.DOColor(fullOpacity, time))
                .Append(image.DOColor(currCol, time));
        } else
        {
            seq.Append(image.DOColor(fullOpacity, time))
                .Append(image.DOColor(currCol, time))
                .Append(image.DOColor(noOpacity, time))
                .Append(image.DOColor(currCol, time));
        }

    }

    private void OnDestroy()
    {
        seq.Kill();
    }
}
