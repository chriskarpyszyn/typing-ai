using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UIElements;

public class LetterCanvas : MonoBehaviour
{

    [SerializeField] private Color successColor = new Color(0.439f, 0.812f, 0.498f, 1f);

    public GameObject Create(Letter letter, GameObject wordCanvas)
    {
        Debug.Log("Enter Letter Create Canvas");
        Vector3 letterPosition = new Vector3(0, 0, 0);
        
        GameObject newLetterCanvas = Instantiate(this.gameObject, letterPosition, Quaternion.identity);
        TextMeshPro tmpLetter = newLetterCanvas.GetComponent<TextMeshPro>();

        tmpLetter.color = new Color(tmpLetter.color.r, tmpLetter.color.g, tmpLetter.color.b, 0);

        newLetterCanvas.transform.SetParent(wordCanvas.transform, false);

        tmpLetter.name = "letterCanvas:" + letter.letterChar;
        tmpLetter.text = letter.letterChar.ToString().ToUpper();

        return newLetterCanvas;
    }

    public void ChangeColor(GameObject letter)
    {
        letter.GetComponent<TextMeshPro>().color = successColor;
    }

    private void LetterScaleAnimation(float toSize, GameObject letter)
    {
        ScaleTextAnimation scaleTextAnimation = new ScaleTextAnimation();
        scaleTextAnimation.ScaleAnimation(
            letter,
            new Vector3(toSize, toSize, toSize),
            0.1f);
    }

    public void LetterFadeInAnimation(GameObject letter, float animationDuration, float delay)
    {
        TextMeshPro letterTMP = letter.GetComponent<TextMeshPro>();
        ScaleTextAnimation scaleTextAnimation = new ScaleTextAnimation();
        scaleTextAnimation.FadeTMPAnimation(letterTMP, 1, animationDuration).SetDelay(delay);
    }

    public void IncreaseLetterScale(GameObject letter)
    {
        LetterScaleAnimation(1.2f, letter);
    }

    public void DecreaseLetterScale(GameObject letter)
    {
        LetterScaleAnimation(1f, letter);
    }

 
    //private void CreateGameObjectWordList(char[] wordCharArray, GameObject letterParent)
    //{
    //    //float animationDuration = 0.4f;
    //    //float overlapDelay = 0.04f;
    //    //float cumulativeDelay = overlapDelay;

    //    foreach (char c in wordCharArray)
    //    {
    //        GameObject newLetter = Instantiate(letterPrefab, new Vector3(0, 0, 0), Quaternion.identity);
    //        TextMeshPro tmpLetter = newLetter.GetComponent<TextMeshPro>();
    //        tmpLetter.color = new Color(tmpLetter.color.r, tmpLetter.color.g, tmpLetter.color.b, 0);
    //        newLetter.transform.SetParent(letterParent.transform, false);
    //        newLetter.name = "offset" + firstLetterPositionX;
    //        tmpLetter.text = c.ToString().ToUpper();
    //        //AddNewLetter(newLetter);


    //        if (currentWordList.Count == 1)
    //        {
    //            scaleTextAnimation.FadeTMPAnimation(tmpLetter, 1, animationDuration);
    //        }
    //        else
    //        {
    //            scaleTextAnimation.FadeTMPAnimation(tmpLetter, 1, animationDuration).SetDelay(cumulativeDelay);
    //            cumulativeDelay = cumulativeDelay + overlapDelay;

    //        }
    //    }
    //}
}
