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
        // comment instance pourrait == this ? si tu rentres dans l'awake, il y repassera pas
        if (Instance != null && Instance != this)
        {
            Destroy(this);
            // ptet un petit return ici pour éviter que instance = this = null ( vu que tu destroy )
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
        environment,
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
    
    
    
    // pas giga claires tes params, je comprends pas trop la diff entre soundtoplay et sound
    public void PlaySound(int _soundToplay, int _audioMixer, Sound _sound, GameObject _enemy = null,bool _loop = false)
    {
        bool isEnemySound = false;
        if(_enemy is not null) sfxSourceEnemy = _enemy.GetComponentInParent<AudioSource>();
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
            case Sound.enemyShotDo:
                sfxSourceEnemy.outputAudioMixerGroup = audioMixers[_audioMixer];
                isEnemySound = true;
                break;
            case Sound.enemyShotRe:
                sfxSourceEnemy.outputAudioMixerGroup = audioMixers[_audioMixer];
                isEnemySound = true;
                break;
            case Sound.enemyViolonHit:
                sfxSourcePlayer.outputAudioMixerGroup = audioMixers[_audioMixer];
                isEnemySound = true;
                break;
            case Sound.enemyFluteHit:
                sfxSourceEnemy.outputAudioMixerGroup = audioMixers[_audioMixer];
                isEnemySound = true;
                break;
            case Sound.enemyRunAttack:
                sfxSourceEnemy.outputAudioMixerGroup = audioMixers[_audioMixer];
                isEnemySound = true;
                break;
        }

        if (_loop && !isEnemySound)
        {
            sfxSourcePlayer.clip = sfxPlayer[_soundToplay];
            sfxSourcePlayer.loop = _loop;
            sfxSourcePlayer.Play();
        }
        else if(!_loop && !isEnemySound) sfxSourcePlayer.PlayOneShot(sfxPlayer[_soundToplay]);
        else if (_loop && isEnemySound)
        {
            sfxSourceEnemy.clip = sfxEnemy[_soundToplay];
            sfxSourceEnemy.loop = _loop;
            sfxSourceEnemy.Play();
        }
        else if(!_loop && isEnemySound) 
            sfxSourceEnemy.PlayOneShot(sfxEnemy[_soundToplay]);
        
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


