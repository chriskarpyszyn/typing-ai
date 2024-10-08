using System.Collections;
using System.Collections.Generic;
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
            return false;
        }

        GetLetterAtCurrentIndex().HighlightSuccessLetter();

        currentLetterIndex++;
        //Increase Size of next letter

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
