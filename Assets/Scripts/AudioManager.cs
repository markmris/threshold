using System;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public AudioSource generatorHum;
    public AudioSource sfxSource;

    public void PlaySound(AudioClip sound)
    {
        sfxSource.resource = sound;
        sfxSource.Play();
    }

    public void PlaySound(AudioClip sound, bool powerOn)
    {
        sfxSource.resource = sound;
        sfxSource.Play();

        if (!powerOn) generatorHum.Stop();
        else generatorHum.Play();


    }
}
