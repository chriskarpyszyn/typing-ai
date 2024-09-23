using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Letter 
{  

    public char LetterChar { get; set; }
    [SerializeField] private GameObject letterPrefab;

    public Letter(char letter)
    {
        LetterChar = letter;
    }

    public void Highlight()
    {
        //implement highlight logic 
    }

}
