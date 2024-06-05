using System.Collections;
using System.Collections.Generic;
using UnityEditor.UIElements;
using UnityEngine;


public class SoundManager : MonoBehaviour
{
    public static SoundManager Instance;
    private AudioSource[] audioSources;

    private int pitchTick = 0;
    private int pitchTickRefreshAt = 10;

    private void Awake()
    {
        if (Instance == null)
        {
            audioSources = GetComponents<AudioSource>();
            Instance = this;
            DontDestroyOnLoad(gameObject);
        } else
        {
            if (Instance != null)
            {
                Destroy(gameObject);
            }
        }
    }

    private void Update()
    {
        PitchShift();
    }

    public void PitchShift()
    {
        if (pitchTick >= pitchTickRefreshAt)
        {
            int getPosOrNeg = GetPosOrNeg();
            float pitch = audioSources[0].pitch;

            float pitchChangeAmount = 0.005f;
            float pitchCeiling = 1.0f;
            float pitchFloor = 0.75f;
            if (getPosOrNeg < 0)
            {
                if (pitch > pitchFloor)
                    pitch = pitch - pitchChangeAmount;
            }
            else
            {
                if (pitch < pitchCeiling)
                    pitch = pitch + pitchChangeAmount;
            }

            audioSources[0].pitch = pitch;
            pitchTick = 0;
        } else
        {
            pitchTick++;
        }
    }

    public void PlayBackgroundNoise()
    {
        //todo-ck can't assume the list will be initialized in order...
        audioSources[0].volume = 0.02f;
        audioSources[1].Play();
        audioSources[2].Play();
    }

    private int GetPosOrNeg() { 
    
        return (int)Mathf.Sign(Random.value - 0.5f);
    }
}
