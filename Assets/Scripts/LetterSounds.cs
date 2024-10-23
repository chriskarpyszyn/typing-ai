using UnityEngine;

public class LetterSounds : MonoBehaviour
{
    //private List<AudioSource> audioSources;

    //[SerializeField] private AudioClip positiveLongSound;

    //[SerializeField] private float positiveLongSoungVol = 0.5f;

    ////TODO-ck this can be deleted.

    //void Start()
    //{
    //    audioSources = new List<AudioSource>(GetComponents<AudioSource>());
    //}


    //public void playPositiveLongSound()
    //{
    //    playSound(positiveLongSound, 1f, positiveLongSoungVol, true);
    //}

 
    //private void playSound(AudioClip audioClip, float pitch, float vol, bool shiftPitch)
    //{
    //    AudioSource audioSources = getAvailableAudioSource();
    //    if (audioSources != null)
    //    {
    //        audioSources.clip = audioClip;
    //        audioSources.volume = vol;
    //        if (shiftPitch)
    //        {
    //            audioSources.pitch = SlightPitchShift(pitch);
    //        }
    //        else
    //        {
    //            audioSources.pitch = pitch;
    //        }
            
    //        audioSources.Play();
    //    }
    //}

    //private float SlightPitchShift(float startingNumber)
    //{
    //    return Random.Range(startingNumber - 0.2f, startingNumber + 0.2f);
    //}
   
    //private AudioSource getAvailableAudioSource()
    //{
    //    foreach (AudioSource audioSource in audioSources)
    //    {
    //        if (audioSource != null && !audioSource.isPlaying)
    //        {
    //            return audioSource;
    //        }
    //    }
    //    return audioSources[0];
    //}
}
