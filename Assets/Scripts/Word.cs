using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices.WindowsRuntime;
using UnityEditor;
using UnityEngine;

public class Word
{
    private WordCanvas wordCanvas;
    private LetterCanvas letterCanvas;
    private List<Letter> letters = new List<Letter>();
    int currentLetterIndex;

    public Word(string word, WordCanvas canvasWord)
    {
        currentLetterIndex = 0;
        foreach (char c in word)
        {
            Letter letter = new Letter(c);
            this.letters.Add(letter);
        }
        
        
        this.wordCanvas = canvasWord;
        //canvasWord.Create();

    }

    public void SetCanvas()
    {
        //for example.
    }

    public void CreateCanvas()
    {
        //for example
    }

    public void AnimateFading()
    {

    }

    private char GetCharAtIndex(int i)
    {
        return letters[i].LetterChar;
    }

    public char GetCharAtCurrentLetterIndex()
    {
        return letters[currentLetterIndex].LetterChar;
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
