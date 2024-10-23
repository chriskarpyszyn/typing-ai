using DG.Tweening;
using UnityEngine;

public class AstroidMovement : MonoBehaviour
{

    [SerializeField] private float asteroidSpeed = 20f;

    // Start is called before the first frame update
    void Start()
    {
        RectTransform rectTransform = GetComponent<RectTransform>();
        rectTransform.DOAnchorPos(new Vector3(0, 0, -1), asteroidSpeed).SetEase(Ease.Linear);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
