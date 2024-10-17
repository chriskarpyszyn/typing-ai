using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using System.Net.Http.Headers;
using UnityEngine;

public class StarRotationAnimation : MonoBehaviour
{

    [SerializeField] private float zEnd = -359.99f;
    [SerializeField] private float duration = 1f;
    [SerializeField] private bool infiniteLoop = false;

    Sequence seq;

    // Start is called before the first frame update
    void Start()
    {
        seq = DOTween.Sequence();

        if (infiniteLoop)
        {
            seq.SetLoops(-1);
        } else
        {
            seq.SetLoops(0);
        }
        seq.Append(transform.DORotate(new Vector3(0, 0, zEnd), duration, RotateMode.LocalAxisAdd).SetEase(Ease.Linear));
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnDestroy()
    {
        seq.Kill();
    }
}
