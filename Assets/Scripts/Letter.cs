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

    public Letter WithLetterCanvas(LetterCanvas letterCanvas)
    {
        this.letterCanvas = letterCanvas;
        return this;
    }

    public void CreateCanvas(GameObject wordObject)
    {
        if (letterCanvas == null) { 
            Debug.LogError("Letter.CreateCanvas: Missing LetterCanvas reference");
            return;
        }
        if (wordObject == null) {
            Debug.LogError("Letter.CreateCanvas: Missing WordObject reference");
        }


        letterCanvas.Create(this, wordObject);
    }

    public void Highlight()
    {
        //implement highlight logic 
    }

}
