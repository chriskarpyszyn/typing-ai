using UnityEngine;

public class TitleSound : MonoBehaviour
{
    [SerializeField] private FadeIn fadeIn;
    [SerializeField] private AudioClip titleSound;
    [SerializeField] private float titleSoundVol;
    [SerializeField] private float titleSoundPitch;

    void Awake()
    {
        fadeIn.EventOnFadeInComplete += HandleFadeInComplete;
    }

    private void OnDisable()
    {
        fadeIn.EventOnFadeInComplete -= HandleFadeInComplete;
    }

    private void HandleFadeInComplete()
    {
        new Sounds()
            .PlaySound(GetComponent<AudioSource>(), titleSound, titleSoundPitch, titleSoundVol);
    }
}
