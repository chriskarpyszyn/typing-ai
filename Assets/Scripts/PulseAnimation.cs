using DG.Tweening;
using UnityEngine;

public class PulseAnimation : MonoBehaviour
{
    [SerializeField] private float scaleMin;
    [SerializeField] private float scaleMax;
    [SerializeField] private float duration;

    // Start is called before the first frame update
    void Start()
    {
        Sequence seq = DOTween.Sequence().SetLoops(-1);
        seq.Append(transform.DOScale(scaleMax, duration).SetEase(Ease.Linear))
            .Append(transform.DOScale(scaleMin, duration).SetEase(Ease.Linear));
    }
}
