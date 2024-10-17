using System.Collections.Generic;
using UnityEngine;

public class Sounds
{
    public Sounds() { }

    public void PlaySound(List<AudioSource> audioSources, AudioClip audioClip, float pitch, float vol)
    {
        PlaySound(GetFreeAudioSource(audioSources), audioClip, pitch, vol);
    }

    public void PlaySound(AudioSource audioSource, AudioClip audioClip, float pitch, float vol)
    {
        audioSource.clip = audioClip;
        audioSource.volume = vol;
        audioSource.pitch = pitch;
        audioSource.Play();
    }

    public AudioSource GetFreeAudioSource(List<AudioSource> audioSources)
    {
        foreach (AudioSource audioSource in audioSources)
        {
            if (!audioSource.isPlaying)
            {
                return audioSource;
            }
        }
        return audioSources[0];
    }
}