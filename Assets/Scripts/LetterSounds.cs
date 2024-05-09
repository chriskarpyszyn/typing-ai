using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LetterSounds : MonoBehaviour
{
    private AudioSource audioSource;

    public AudioClip errorSound;

    void Start()
    {
        audioSource = GetComponent<AudioSource>(); //GetComponents if we need more than one :)
    }


    public void playErrorSound()
    {
        playSound(errorSound, 1.2f, 0.3f, false);
    }

    private void playSound(AudioClip audioClip, float pitch, float vol, bool shiftPitch)
    {
        //AudioSource audioSource = getAvailableAudioSource()
        if (audioSource != null)
        {
            audioSource.clip = audioClip;
            audioSource.volume = vol;
            audioSource.pitch = pitch;
            audioSource.Play();
        }
    }

    /**
     *    private void playSound(AudioClip clip, float pitch, float vol, bool shiftPitch)
    {
        AudioSource audioSource1 = getAvailableAudioSource();
        if (audioSource1 != null)
        {
            audioSource1.clip = clip;
            if (shiftPitch)
                audioSource1.pitch = slightPitchShift(pitch);
            else
                audioSource1.pitch = pitch;
            audioSource1.volume = vol;
            audioSource1.Play();
        }


    }**/

    /** 
     *     private AudioSource getAvailableAudioSource()
    {
        foreach (AudioSource audioSource in audioSources)
        {
            if (audioSource != null && !audioSource.isPlaying)
            {
                return audioSource;
            }
        }
        return audioSources[0];
    } **/
}
