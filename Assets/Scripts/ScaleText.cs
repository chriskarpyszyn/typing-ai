using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using DG.Tweening;
using Unity.VisualScripting;
using System.Linq;

public class ScaleTextAnimation
{
    public Tween ScaleAnimation(GameObject objectToAnimate, Vector3 endVector, float duration)
    {
        return objectToAnimate.transform.DOScale(endVector, duration);
    }

    public Tween FadeChildrenAnimation(GameObject parentObjectToAnimate, float opacity, float duration)
    {
        List<Tween> tweens = new List<Tween>();
        TextMeshProUGUI[] children = parentObjectToAnimate.GetComponentsInChildren<TextMeshProUGUI>();
        Debug.Log("Number of children " + children.Length);
        foreach (TextMeshProUGUI child in children)
        {
           tweens.Add(child.DOFade(opacity, duration));
        }

        
        return tweens.Last();
    }

    public Tween FadeTMPAnimation(TextMeshPro tmpObject, float opacity, float duration)

    {
        return tmpObject.DOFade(opacity, duration);
    }
}
