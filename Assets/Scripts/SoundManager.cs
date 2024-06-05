using System.Collections;
using System.Collections.Generic;
using UnityEditor.UIElements;
using UnityEngine;


public class SoundManager : MonoBehaviour
{
    public static SoundManager Instance;
    private AudioSource audioSource;

    private int pitchTick = 0;
    private int pitchTickRefreshAt = 10;

    private void Awake()
    {
        if (Instance == null)
        {
            audioSource = GetComponent<AudioSource>();
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
            float pitch = audioSource.pitch;

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

            audioSource.pitch = pitch;
            pitchTick = 0;
            Debug.Log(pitch);
        } else
        {
            pitchTick++;
        }

    }

    private int GetPosOrNeg() { 
    
        return (int)Mathf.Sign(Random.value - 0.5f);
    }
}
