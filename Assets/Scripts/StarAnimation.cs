using UnityEngine;
using DG.Tweening;

public class StarAnimation : MonoBehaviour
{

    [SerializeField] private float maxStarSize = 2f;
    private float duration = 0.3f;

    private static int numberOfStars = 0;

    private readonly float Z_POS = 0.007943317f;

    private Sequence mySeq;

    [SerializeField] float randomSizeRange = 0.3f;
    

    // Start is called before the first frame update
    void Start()
    {
        float randoFloato = Random.Range(0, duration-0.40f);
        float randomSizeFloat = Random.Range(-randomSizeRange, randomSizeRange);

        Sequence mySeq = DOTween.Sequence();
        mySeq.PrependInterval(randoFloato);
        mySeq.Append(transform.DOScale(maxStarSize+randomSizeFloat, duration).SetEase(Ease.Linear));
        mySeq.Append(transform.DOScale(0f, duration).SetEase(Ease.Linear));
        mySeq.SetLoops(-1);
        mySeq.OnStepComplete(MyCallback);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void MyCallback()
    {
        this.transform.position = new Vector3(GetRandomX(), GetRandomY(), Z_POS);
        if (numberOfStars < 5)
        {
            CreateClone();
            CreateClone();
            CreateClone();
            CreateClone();
            CreateClone();
            CreateClone();
            CreateClone();
        }

        if (numberOfStars < 600)
        {
            CreateClone();
            int randomMax = 4;
            int randomInt = Random.Range(1, randomMax);
            if (randomInt == randomMax/2)
            {
                CreateClone();
                CreateClone();
                CreateClone();
            }


        }
        //mySeq.Kill();
        //Destroy(gameObject);
        //numberOfStars--;
    }

    private GameObject CreateClone()
    {
        numberOfStars++;
        GameObject newStar = Instantiate(gameObject);
        newStar.transform.SetParent(transform.parent);
        newStar.transform.position = new Vector3(GetRandomX(), GetRandomY(), Z_POS);
        newStar.transform.rotation = Quaternion.Euler(0f, 0f, Random.Range(-180,180));
        return newStar;
    }

    private float GetRandomX()
    {
        float canvasX = 950f * 2;
        return Random.Range(0, canvasX);
    }

    private float GetRandomY()
    {
        float canvasY = 540f * 2;
        return Random.Range(0, canvasY);
    }
}
