using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    
    public static AudioManager Instance;
    [SerializeField] private AudioClip mainMusic;
    [SerializeField] private AudioSource musicSource;
    
    void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        transform.SetParent(null);
        DontDestroyOnLoad(gameObject);
    }
    
    void Start()
    {
        if (mainMusic == null)
            return;
        musicSource.clip = mainMusic;
        musicSource.Play();
    }
    [SerializeField] private AudioSource sfxAudioManager;
    
    
    public enum EnemySound
    {
        AttackDo,
        AttackRé,
        AttackFlute,
        HitFlute,
        HitViolon,
    }

    public enum PlayerSound
    {
        AttackDo,
        AttackRé,
        ZoneDo,
        ZoneRé,
        PlayerHit
    }

    public enum EnvironmentSound
    {
        Checkpoint,
    }

    [SerializeField] public List<AudioClip> EnemyClips = new List<AudioClip>();
    [SerializeField] public List<AudioClip> PlayerClips = new List<AudioClip>();
    [SerializeField] public List<AudioClip> EnvironmentClips = new List<AudioClip>();

    public void PlaySoundEnemy(EnemySound son, float volume)
    {
        sfxAudioManager.PlayOneShot(EnemyClips[(int)son], volume);
    }

    public void PlaySoundPlayer(PlayerSound son, float volume)
    {
        sfxAudioManager.PlayOneShot(PlayerClips[(int)son], volume);
    }

    public void PlaySoundEnvironment(EnvironmentSound son, float volume)
    {
        sfxAudioManager.PlayOneShot(EnvironmentClips[(int)son], volume);
    }
}

// Mettre plusieurs sfxSource genre player Environment


