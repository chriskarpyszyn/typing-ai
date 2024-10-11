using System.Collections;
using System.Collections.Generic;
using System.Runtime.ExceptionServices;
using System.Runtime.InteropServices.WindowsRuntime;
using UnityEditor;
using UnityEngine;

public class Word
{
    private WordCanvas wordCanvas;
    private List<Letter> letters;
    private GameObject wordGameObject;

    private float xPos;
    private float yPos;
    private float zPos;

    int currentLetterIndex;

    private float positiveIncremenet = 0.2f;
    private float positiveSoundPitch = 0.6f;

    public Word(string word)
    {
        letters = new List<Letter>();
        currentLetterIndex = 0;
        foreach (char c in word)
        {
            Letter letter = new Letter(c);
            this.letters.Add(letter);
        }
    }

    public Word WithWordCanvas(WordCanvas wordCanvas)
    {
        this.wordCanvas = wordCanvas;
        return this;
    }

    public Word WithPosition(float x, float y, float z)
    {
        this.xPos = x;
        this.yPos = y;
        this.zPos = z;
        return this;
    }

    public GameObject CreateCanvas()
    {
        wordGameObject = wordCanvas.Create(xPos,yPos,zPos);
        return wordGameObject;
    }

    public void CanvasShrinkFX()
    {
        ScaleTextAnimation sta = new ScaleTextAnimation();
        sta.ScaleAnimation(wordGameObject, new Vector3(0, 0, 0), 0.001f);
    }

    public void DestroyCanvas()
    {
        wordCanvas.DestroyCanvas(wordGameObject);
    }

    public void CreateLetterCanvas(GameObject wordObject, LetterCanvas letterCanvas)
    {
        foreach (Letter l in letters)
        {
            l.WithLetterCanvas(letterCanvas)
                .CreateObject(wordObject);
        }

        float animationDuration = 0.4f;
        float overlapDelay = 0.04f;
        float cumulativeDelay = overlapDelay;
        int letterCount = letters.Count;

        for (int i = 0; i<letterCount; i++)
        {
            if (i==0)
            {
                letters[i].FadeInLetterFX(animationDuration, 0);
            } else
            {
                letters[i].FadeInLetterFX(animationDuration, cumulativeDelay);
                cumulativeDelay = cumulativeDelay + overlapDelay;
            }
        }
    }

    public void AnimateFading()
    {

    }

    private char GetCharAtIndex(int i)
    {
        return letters[i].letterChar;
    }

    public char GetCharAtCurrentLetterIndex()
    {
        return letters[currentLetterIndex].letterChar;
    }

    public Letter GetLetterAtCurrentIndex()
    {
        return letters[currentLetterIndex];
    }

    public bool ValidateLetter(char c)
    {
        if (GetCharAtCurrentLetterIndex() != c)
        {
            Debug.Log("Validate False");
            return false;
        }
        Debug.Log("Validate True");

        Letter currentLetter = GetLetterAtCurrentIndex();
        currentLetter.HighlightSuccessLetterFX();
        currentLetter.PlaySuccessSound(positiveSoundPitch);
        positiveSoundPitch += positiveIncremenet;
        currentLetter.LetterScaleDecreaseFX();

        currentLetterIndex++;
        
        if (!IsWordCompleted()) {
            GetLetterAtCurrentIndex().LetterScaleIncreaseFX();
        }

        return true;
    }

    public bool IsWordCompleted()
    {
        return currentLetterIndex == Count();
    }

    public int Count()
    {
        return letters.Count;
    }
}
