using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

public class AudioManager : MonoBehaviour
{
    
    public static AudioManager Instance;
    [SerializeField] private AudioClip mainMusic;
    [SerializeField] private AudioSource musicSource;
    
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
    
    public enum TypeAudio
    {
        playerShotDo,
        playerShotRe,
        playerZoneDo,
        playerZoneRe,
        enemy,
        environment,
    }
    
    [Header("Sounds Player")]
    [SerializeField] private List<AudioClip> sfxPlayer = new List<AudioClip>();

    [Header("Audio Mixers Player")] 
    [SerializeField] private List<AudioMixerGroup> audioMixers = new List<AudioMixerGroup>();

    [Header("Audio Sources Player")] 
    [SerializeField] private AudioSource sfxSourcePlayer;
    
    
    
    public void SoundExample(int _soundToplay, int _audioMixer, TypeAudio _typeAudio,bool _loop = false)
    {
        switch (_typeAudio)
        {
            case TypeAudio.playerShotDo:
                sfxSourcePlayer.outputAudioMixerGroup = audioMixers[_audioMixer];
                break;
            case TypeAudio.playerShotRe:
                sfxSourcePlayer.outputAudioMixerGroup = audioMixers[_audioMixer];
                break;
            case TypeAudio.playerZoneDo:
                sfxSourcePlayer.outputAudioMixerGroup = audioMixers[_audioMixer];
                break;
            case TypeAudio.playerZoneRe:
                sfxSourcePlayer.outputAudioMixerGroup = audioMixers[_audioMixer];
                break;
        }
        sfxSourcePlayer.clip = sfxPlayer[_soundToplay];
        sfxSourcePlayer.loop = _loop;
        sfxSourcePlayer.Play();
    }

    public void StopSound()
    {
        sfxSourcePlayer.Stop();
    }
}

// Mettre plusieurs sfxSource genre player Environment


