using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LetterSounds : MonoBehaviour
{
    private List<AudioSource> audioSources;

    [SerializeField] private AudioClip errorSound;
    [SerializeField] private AudioClip positiveSound;
    [SerializeField] private AudioClip positiveLongSound;

    [SerializeField] private float positiveSoundVol = 0.5f;
    [SerializeField] private float positiveLongSoungVol = 0.5f;

    private float positiveIncremenet = 0.2f;
    private float positiveSoundPitch = 0.6f;
    private const float POSITIVE_SOUND_PITCH_START_VALUE = 0.6f;

    void Start()
    {
        audioSources = new List<AudioSource>(GetComponents<AudioSource>());
    }


    public void playErrorSound()
    {
        playSound(errorSound, 1.2f, 0.3f, false);
    }

    public void playPositiveSound()
    {
        playSound(positiveSound, IncreasePositivePitchShift(), positiveSoundVol, false);
    }

    public void playPositiveLongSound()
    {
        playSound(positiveLongSound, 1f, positiveLongSoungVol, true);
    }

    public void ResetPositiveSoundPitch()
    {
        positiveSoundPitch = POSITIVE_SOUND_PITCH_START_VALUE;
    }

    private float IncreasePositivePitchShift()
    {
        positiveSoundPitch += positiveIncremenet;
        return positiveSoundPitch;
    }

    private void playSound(AudioClip audioClip, float pitch, float vol, bool shiftPitch)
    {
        AudioSource audioSources = getAvailableAudioSource();
        if (audioSources != null)
        {
            audioSources.clip = audioClip;
            audioSources.volume = vol;
            if (shiftPitch)
            {
                audioSources.pitch = SlightPitchShift(pitch);
            }
            else
            {
                audioSources.pitch = pitch;
            }
            
            audioSources.Play();
        }
    }

    private float SlightPitchShift(float startingNumber)
    {
        return Random.Range(startingNumber - 0.2f, startingNumber + 0.2f);
    }
   
    private AudioSource getAvailableAudioSource()
    {
        foreach (AudioSource audioSource in audioSources)
        {
            if (audioSource != null && !audioSource.isPlaying)
            {
                return audioSource;
            }
        }
        return audioSources[0];
    }
}
