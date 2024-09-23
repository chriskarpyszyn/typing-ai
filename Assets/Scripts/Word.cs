using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Word 
{
    private List<Letter> letters = new List<Letter>();

    public Word(string word)
    {
        foreach (char c in word)
        {
            Letter letter = new Letter(c);
            this.letters.Add(letter);
        }
    }

    public void AnimateFading()
    {

    }
}
