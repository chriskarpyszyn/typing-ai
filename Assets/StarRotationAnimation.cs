using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StarRotationAnimation : MonoBehaviour
{

    [SerializeField] private float zEnd = -359.99f;
    [SerializeField] private float duration = 1f;

    // Start is called before the first frame update
    void Start()
    {
        Sequence seq = DOTween.Sequence();
        seq.SetLoops(-1);
        seq.Append(transform.DORotate(new Vector3(0, 0, zEnd), duration, RotateMode.LocalAxisAdd).SetEase(Ease.Linear));
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
