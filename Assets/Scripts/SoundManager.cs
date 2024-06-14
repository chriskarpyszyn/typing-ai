using System.Collections;
using System.Collections.Generic;
using UnityEditor.UIElements;
using UnityEngine;


public class SoundManager : MonoBehaviour
{
    public static SoundManager Instance;
    private AudioSource[] audioSources;

    [SerializeField]
    private AudioClip titleMusic;
    [SerializeField]
    private AudioClip backgroundMusic;
    [SerializeField]
    private AudioClip spaceshipHum;
    [SerializeField]
    private AudioClip radioSound;

    [SerializeField]
    private float titleMusicVol = 0.7f;
    [SerializeField]
    private float spaceshipHumVol = 0.25f;
    [SerializeField]
    private float radioSoundVol = 0.05f;

    private int pitchTick = 0;
    private int pitchTickRefreshAt = 10;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);

            Initialize();
        } else
        {
            if (Instance != null)
            {
                Destroy(gameObject);
            }
        }
    }

    private void Initialize()
    {
        audioSources = GetComponents<AudioSource>();
        audioSources[0].clip = titleMusic;
        audioSources[0].volume = titleMusicVol;
        audioSources[0].loop = true;
        audioSources[0].Play();
    }

    private void Update()
    {
        //PitchShift();
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
        //todo-ck (we probably want to initialize all AudioSource component settings in code?
        //todo-ck extract this into a method to setup looping music
        audioSources[0].volume = 0.1f;
        audioSources[1].clip = spaceshipHum;
        audioSources[1].volume = spaceshipHumVol;
        audioSources[1].loop = true;
        audioSources[2].clip = radioSound;
        audioSources[2].volume = radioSoundVol;
        audioSources[2].loop = true;
        audioSources[1].Play();
        audioSources[2].Play();
    }

    private int GetPosOrNeg() { 
        return (int)Mathf.Sign(Random.value - 0.5f);
    }
}
