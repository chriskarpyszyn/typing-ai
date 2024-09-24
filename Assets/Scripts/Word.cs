using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices.WindowsRuntime;
using UnityEditor;
using UnityEngine;

public class Word 
{
    private List<Letter> letters = new List<Letter>();
    int currentLetterIndex;

    public Word(string word)
    {
        currentLetterIndex = 0;
        foreach (char c in word)
        {
            Letter letter = new Letter(c);
            this.letters.Add(letter);
        }
    }

    public void AnimateFading()
    {

    }

    public char GetCharAtIndex(int i)
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

        //call animation? etc. etc.

        return true;
    }

    public int Count()
    {
        return letters.Count;
    }
}
