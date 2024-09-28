using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using Unity.Profiling.Editor;
using UnityEngine;

public class Letter 
{
    private LetterCanvas letterCanvas;
    private GameObject wordObject;

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

    public Letter WithWordObject(GameObject wordObject)
    {
        this.wordObject = wordObject;
        return this;
    }

    public void CreateCanvas()
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
