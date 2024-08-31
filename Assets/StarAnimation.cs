using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class StarAnimation : MonoBehaviour
{

    [SerializeField] private float maxStarSize = 4f;

    // Start is called before the first frame update
    void Start()
    {
        Sequence mySeq = DOTween.Sequence();
        mySeq.SetLoops(-1);
        mySeq.Append(transform.DOScale(maxStarSize, 0.5f).SetEase(Ease.Linear));
        mySeq.Append(transform.DOScale(0f, 0.5f).SetEase(Ease.Linear));
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
