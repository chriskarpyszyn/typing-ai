using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LetterSounds : MonoBehaviour
{
    private AudioSource audioSource;

    [SerializeField]
    private AudioClip errorSound;
    [SerializeField]
    private AudioClip positiveSound;
    [SerializeField]
    private AudioClip positiveLongSound;

    [SerializeField]
    private float positiveSoundVol = 0.5f;
    [SerializeField]
    private float positiveLongSoungVol = 0.5f;

    void Start()
    {
        audioSource = GetComponent<AudioSource>(); //GetComponents if we need more than one :)
    }


    public void playErrorSound()
    {
        playSound(errorSound, 1.2f, 0.3f, false);
    }

    public void playPositiveSound()
    {
        playSound(positiveSound, 1f, positiveSoundVol, true);
    }

    public void playPositiveLongSound()
    {
        playSound(positiveLongSound, 1f, positiveLongSoungVol, true);
    }

    private void playSound(AudioClip audioClip, float pitch, float vol, bool shiftPitch)
    {
        //AudioSource audioSource = getAvailableAudioSource()
        if (audioSource != null)
        {
            audioSource.clip = audioClip;
            audioSource.volume = vol;
            if (shiftPitch)
            {
                audioSource.pitch = SlightPitchShift(pitch);
            }
            else
            {
                audioSource.pitch = pitch;
            }
            
            audioSource.Play();
        }
    }

    private float SlightPitchShift(float startingNumber)
    {
        return Random.Range(startingNumber - 0.2f, startingNumber + 0.2f);
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
