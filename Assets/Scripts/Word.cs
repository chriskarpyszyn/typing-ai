using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices.WindowsRuntime;
using UnityEditor;
using UnityEngine;

public class Word
{
    private WordCanvas wordCanvas;
    private List<Letter> letters;

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
        return wordCanvas.Create(xPos,yPos,zPos);
    }

    public void CreateLetterCanvas(GameObject wordObject, LetterCanvas letterCanvas)
    {
        foreach (Letter l in letters)
        {
            l.WithLetterCanvas(letterCanvas)
                .CreateCanvas(wordObject);
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

    public bool ValidateLetter(char c)
    {
        if (GetCharAtCurrentLetterIndex() != c)
        {
            return false;
        }

        currentLetterIndex++;

        //Set Color of Letter on Success

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
