using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using DG.Tweening;

public class ScaleTextAnimation
{
    public Tween ScaleAnimation(GameObject objectToAnimate, Vector3 endVector, float duration)
    {
        return objectToAnimate.transform.DOScale(endVector, duration);
    }
}
