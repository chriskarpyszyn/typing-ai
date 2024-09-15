using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using Mono.Cecil;

public class StarAnimation : MonoBehaviour
{

    [SerializeField] private float maxStarSize = 2f;
    private float duration = 0.5f;

    private static int numberOfStars = 0;

    private Sequence mySeq;

    [SerializeField] float randomSizeRange = 0.3f;
    

    // Start is called before the first frame update
    void Start()
    {
        float randoFloato = Random.Range(0, duration-0.35f);
        float randomSizeFloat = Random.Range(-randomSizeRange, randomSizeRange);

        Sequence mySeq = DOTween.Sequence();
        mySeq.PrependInterval(randoFloato);
        mySeq.Append(transform.DOScale(maxStarSize+randomSizeFloat, duration).SetEase(Ease.Linear));
        mySeq.Append(transform.DOScale(0f, duration).SetEase(Ease.Linear));
        mySeq.OnComplete(MyCallback);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void MyCallback()
    {
        if (numberOfStars < 200)
        {
            CreateClone();
            int randomMax = 6;
            int randomInt = Random.Range(1, randomMax);
            if (randomInt == randomMax/2)
            {
                Debug.Log("test " + randomInt);
                CreateClone();
            }


        }
        mySeq.Kill();
        Destroy(gameObject);
        numberOfStars--;
    }

    private GameObject CreateClone()
    {
        numberOfStars++;
        float canvasX = 950f*2;
        float canvasY = 540f*2;
        float randomX = Random.Range(0, canvasX);
        float randomY = Random.Range(0, canvasY);

        GameObject newStar = Instantiate(gameObject);
        newStar.transform.SetParent(transform.parent);
        newStar.transform.position = new Vector3(randomX, randomY, 0.007943317f);
        newStar.transform.rotation = Quaternion.Euler(0f, 0f, Random.Range(-180,180));
        return newStar;
    }
}
