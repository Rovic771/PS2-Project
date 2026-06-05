using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

public class AudioManager : MonoBehaviour
{
    [Header("Sounds")]
    [SerializeField] private List<AudioClip> sfxPlayer = new List<AudioClip>();
    [SerializeField] private AudioClip mainMusic;

    [Header("Audio Mixers")] 
    [SerializeField] private List<AudioMixerGroup> audioMixers = new List<AudioMixerGroup>();

    [Header("Audio Sources")] 
    [SerializeField] private AudioSource musicSource;
    [SerializeField] private AudioSource sfxSource;
    
    public static AudioManager Instance;

    public enum TypeAudio
    {
        player,
        enemy,
        environment,
    }

   void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(this);
        }

        Instance = this;
    }
   
    void Start()
    {
        if (mainMusic == null)
            return;
        musicSource.clip = mainMusic;
        musicSource.Play();
    }
    
    public void SoundExample(int _soundToplay, int _audioMixer, TypeAudio _typeAudio,bool _loop = false)
    {
        /*
        switch (_typeAudio)
        {
            case 
        }*/
        sfxSource.clip = sfxPlayer[_soundToplay];
        sfxSource.loop = _loop;
        sfxSource.Play();
    }

    public void StopSound()
    {
        sfxSource.Stop();
    }
}

// Mettre plusieurs sfxSource genre player Environment


