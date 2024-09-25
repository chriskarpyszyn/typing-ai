using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using Unity.Profiling.Editor;
using UnityEngine;

public class Letter 
{
    private LetterCanvas letterCanvas;
    
    public char letterChar { get; set; }

    public Letter(char letter)
    {
        this.letterChar = letter;
    }

    public Letter(char letter, LetterCanvas letterCanvas)
    {
        this.letterChar = letter;
        this.letterCanvas = letterCanvas;

        //letterCanvas.Create();
    }

    public void Highlight()
    {
        //implement highlight logic 
    }

}
