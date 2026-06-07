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
        enemyShotDo,
        enemyShotRe,
        enemyViolonHit,
        enemyRunAttack,
        enemyFluteHit,
        checkpoint,
    }
    
    public enum TypeSfxSource{player, enemy}
    
    [Header("Audio Mixers")] 
    [SerializeField] private List<AudioMixerGroup> audioMixers = new List<AudioMixerGroup>();
    
    [Header("Sounds Player")]
    [SerializeField] private List<AudioClip> sfxPlayer = new List<AudioClip>();

    [Header("Audio Sources Player")] 
    [SerializeField] private AudioSource sfxSourcePlayer;
    
    [Header("Sounds Enemy")]
    [SerializeField] private List<AudioClip> sfxEnemy = new List<AudioClip>();
    
    [Header("Audio Sources Enemy")] 
    [SerializeField] public AudioSource sfxSourceEnemy;
    
    [Header("Sounds Environment")]
    [SerializeField] private List<AudioClip> sfxEnvironment = new List<AudioClip>();
    
    [Header("Audio Sources Environment")] 
    [SerializeField] public AudioSource sfxSourceEnvironment;


    private enum TypeAudio
    {
        player,
        enemy,
        environment
    }
    
    public void PlaySound(int _soundToplay, int _audioMixer, Sound _sound, GameObject _gameObject = null,bool _loop = false)
    {
        TypeAudio currentTypeAudio = TypeAudio.player;
        switch (_sound)
        {
            case Sound.playerShotDo:
                sfxSourcePlayer.outputAudioMixerGroup = audioMixers[_audioMixer];
                currentTypeAudio = TypeAudio.player;
                break;
            case Sound.playerShotRe:
                sfxSourcePlayer.outputAudioMixerGroup = audioMixers[_audioMixer];
                currentTypeAudio = TypeAudio.player;
                break;
            case Sound.playerZoneDo:
                sfxSourcePlayer.outputAudioMixerGroup = audioMixers[_audioMixer];
                currentTypeAudio = TypeAudio.player;
                break;
            case Sound.playerZoneRe:
                sfxSourcePlayer.outputAudioMixerGroup = audioMixers[_audioMixer];
                currentTypeAudio = TypeAudio.player;
                break;
            case Sound.enemyShotDo:
                if(_gameObject != null) sfxSourceEnemy = _gameObject.GetComponentInParent<AudioSource>();
                sfxSourceEnemy.outputAudioMixerGroup = audioMixers[_audioMixer];
                currentTypeAudio = TypeAudio.enemy;
                break;
            case Sound.enemyShotRe:
                if(_gameObject != null) sfxSourceEnemy = _gameObject.GetComponentInParent<AudioSource>();
                sfxSourceEnemy.outputAudioMixerGroup = audioMixers[_audioMixer];
                currentTypeAudio = TypeAudio.enemy;
                break;
            case Sound.enemyViolonHit:
                if(_gameObject != null) sfxSourceEnemy = _gameObject.GetComponentInParent<AudioSource>();
                sfxSourcePlayer.outputAudioMixerGroup = audioMixers[_audioMixer];
                currentTypeAudio = TypeAudio.enemy;
                break;
            case Sound.enemyFluteHit:
                if(_gameObject != null) sfxSourceEnemy = _gameObject.GetComponentInParent<AudioSource>();
                sfxSourceEnemy.outputAudioMixerGroup = audioMixers[_audioMixer];
                currentTypeAudio = TypeAudio.enemy;
                break;
            case Sound.enemyRunAttack:
                if(_gameObject != null) sfxSourceEnemy = _gameObject.GetComponentInParent<AudioSource>();
                sfxSourceEnemy.outputAudioMixerGroup = audioMixers[_audioMixer];
                currentTypeAudio = TypeAudio.enemy;
                break;
            case Sound.checkpoint:
                if(_gameObject is not null) sfxSourceEnvironment = _gameObject.GetComponentInParent<AudioSource>();
                sfxSourceEnvironment.outputAudioMixerGroup = audioMixers[_audioMixer];
                currentTypeAudio = TypeAudio.environment;
                break;
        }

        if (!_loop)
        {
            switch (currentTypeAudio)
            {
                case TypeAudio.player:
                    sfxSourcePlayer.PlayOneShot(sfxPlayer[_soundToplay]);
                    break;
                case TypeAudio.enemy:
                    sfxSourceEnemy.PlayOneShot(sfxEnemy[_soundToplay]);
                    break;
                case TypeAudio.environment:
                    sfxSourceEnvironment.PlayOneShot(sfxEnvironment[_soundToplay]);
                    break;
            }
        }
        else
        {
            switch (currentTypeAudio)
            {
                case TypeAudio.player:
                    sfxSourcePlayer.clip = sfxPlayer[_soundToplay];
                    sfxSourcePlayer.loop = _loop;
                    sfxSourcePlayer.Play();
                    break;
                case TypeAudio.enemy:
                    sfxSourceEnemy.clip = sfxEnemy[_soundToplay];
                    sfxSourceEnemy.loop = _loop;
                    sfxSourceEnemy.Play();
                    break;
                case TypeAudio.environment:
                    sfxSourceEnvironment.clip = sfxEnvironment[_soundToplay];
                    sfxSourceEnvironment.loop = _loop;
                    sfxSourceEnvironment.Play();
                    break;
            }
        }
    }
    
    
    public void StopSound(TypeSfxSource _typeSfx)
    {
        switch (_typeSfx)
        {
            case TypeSfxSource.player:
                sfxSourcePlayer.Stop();
                break;
            case TypeSfxSource.enemy:
                if (sfxSourceEnemy != null)
                    sfxSourceEnemy.Stop();
                break;
        }
    }
}

// Mettre plusieurs sfxSource genre player Environment


