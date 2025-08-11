using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

public class AudioManager : Singleton<AudioManager>
{
    [Header("----- Audio Source -----")]
    [SerializeField] private AudioSource musicSource;
    [SerializeField] private AudioSource sfxSource;
    [SerializeField] private AudioMixer masterMixer;

    [Header("----- Audio Clip -----")]
    public AudioClip backgroundClip;
    public AudioClip click;
    public AudioClip loose;
    public AudioClip coin;
    public AudioClip hurt;
    public AudioClip dead;
    public AudioClip horn;
    public AudioClip drift;

    private bool isMuted;

    public bool IsMuted => isMuted;

    private void Start()
    {
        /*musicSource.clip = backgroundClip;
        musicSource.Play();
        TurnOn();*/
    }

    public void PlaySFX(AudioClip clip)
    {
        sfxSource.PlayOneShot(clip);
    }

    public void TurnOn()
    {
        isMuted = false;

        masterMixer.SetFloat("Music", -13.18f);
        masterMixer.SetFloat("SFX", 0f);
    }


    public void TurnOff()
    {
        isMuted = true;

        masterMixer.SetFloat("Music", -80f);
        masterMixer.SetFloat("SFX", -80f); // -80 dB là mức yên lặng hoàn toàn
    }
}