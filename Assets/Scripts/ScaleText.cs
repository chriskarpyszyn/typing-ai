using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using DG.Tweening;

public class ScaleTextAnimation
{
    Tween tween;
    //public IEnumerator Scale(GameObject objectToScale, Vector3 startScale, Vector3 endScale, float duration)
    //{
    //    float elapsedTime = 0f;
    //    while (elapsedTime < duration)
    //    {
    //        objectToScale.transform.localScale = Vector3.Lerp(startScale, endScale, elapsedTime / duration);
    //        elapsedTime += Time.deltaTime;
    //        yield return null;
    //    }
    //    objectToScale.transform.localScale = endScale;
    //}

    public Tween ScaleAnimation(GameObject objectToAnimate, Vector3 endVector, float duration)
    {
        return objectToAnimate.transform.DOScale(endVector, duration);
    }
}
