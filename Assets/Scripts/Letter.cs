using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using Unity.Profiling.Editor;
using UnityEngine;

public class Letter 
{
    private LetterCanvas letterCanvas;
    private GameObject letterObject;

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

    public void CreateObject(GameObject wordObject)
    {
        if (letterCanvas == null) { 
            Debug.LogError("Letter.CreateObject: Missing LetterCanvas reference");
            return;
        }
        if (wordObject == null) {
            Debug.LogError("Letter.CreateObject: Missing WordObject reference");
        }

        letterObject = letterCanvas.Create(this, wordObject);
    }

    public void HighlightSuccessLetter()
    {
        letterCanvas.ChangeColor(letterObject);
    }

}
