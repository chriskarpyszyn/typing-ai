using UnityEngine;

public class Sounds
{
    public Sounds() { }

    public void PlaySound(AudioSource audioSource, float pitch, float vol)
    {
        audioSource.volume = vol;
        audioSource.pitch = pitch;
        audioSource.Play();
    }
}