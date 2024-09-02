using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class StarSparkleAnimation : MonoBehaviour
{
    public bool isPos = true;
    // Start is called before the first frame update
    void Start()
    {
        Image image = GetComponent<Image>();
        Color currCol = image.color;
        Color noOpacity = new Color(currCol.r, currCol.g, currCol.b, 0);
        Color fullOpacity = new Color(currCol.r, currCol.g, currCol.b, 0.5f);

        float time = 0.125f;

        Sequence seq = DOTween.Sequence();
        seq.SetLoops(-1);

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

    // Update is called once per frame
    void Update()
    {
        
    }
}
