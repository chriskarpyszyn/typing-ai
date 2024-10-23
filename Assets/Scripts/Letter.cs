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

    public void LetterScaleIncreaseFX()
    {
        letterCanvas.IncreaseLetterScale(letterObject);
    }

    public void LetterScaleDecreaseFX()
    {
        letterCanvas.DecreaseLetterScale(letterObject);
    }

    public void HighlightSuccessLetterFX()
    {
        letterCanvas.ChangeColor(letterObject);
    }

    public void FadeInLetterFX(float animationDuration, float delay)
    {
        letterCanvas.LetterFadeInAnimation(letterObject, animationDuration, delay);
    }

    public void PlaySuccessSound(float pitch)
    {
        AudioSource audioSource = letterObject?.GetComponent<AudioSource>();
        float positiveSoundVol = 0.5f;
        audioSource.volume = positiveSoundVol;
        audioSource.pitch = pitch;
        audioSource.Play();
    }

}
