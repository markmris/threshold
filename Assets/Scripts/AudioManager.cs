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

    public void GeneratorPowerSound(bool powerOn)
    {
        if (!powerOn) generatorHum.Stop();
        else generatorHum.Play();
    }

    public void MuteAudio()
    {
        foreach (Transform child in transform)
        {
            child.GetComponent<AudioSource>().volume = 0;
        }
    }
}
