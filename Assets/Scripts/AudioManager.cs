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
    
    public enum Sound
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
    
    [Header("Sounds Enemy")]
    [SerializeField] private List<AudioClip> sfxEnemy = new List<AudioClip>();

    [Header("Audio Mixers Enemy")] 
    [SerializeField] private List<AudioMixerGroup> audioEnemy = new List<AudioMixerGroup>();

    [Header("Audio Sources Enemy")] 
    [SerializeField] private AudioSource sfxSourceEnemy;
    
    
    
    public void PlaySound(int _soundToplay, int _audioMixer, Sound _sound,bool _loop = false)
    {
        switch (_sound)
        {
            case Sound.playerShotDo:
                sfxSourcePlayer.outputAudioMixerGroup = audioMixers[_audioMixer];
                break;
            case Sound.playerShotRe:
                sfxSourcePlayer.outputAudioMixerGroup = audioMixers[_audioMixer];
                break;
            case Sound.playerZoneDo:
                sfxSourcePlayer.outputAudioMixerGroup = audioMixers[_audioMixer];
                break;
            case Sound.playerZoneRe:
                sfxSourcePlayer.outputAudioMixerGroup = audioMixers[_audioMixer];
                break;
        }

        if (_loop)
        {
            sfxSourcePlayer.clip = sfxPlayer[_soundToplay];
            sfxSourcePlayer.loop = _loop;
            sfxSourcePlayer.Play();
            return;
        }
        sfxSourcePlayer.PlayOneShot(sfxPlayer[_soundToplay]);
    }

    public void StopSound()
    {
        sfxSourcePlayer.Stop();
    }
}

// Mettre plusieurs sfxSource genre player Environment


