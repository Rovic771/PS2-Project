using JetBrains.Annotations;
using UnityEngine;
using UnityEngine.InputSystem;

public class AudioManager : MonoBehaviour
{
    [Header("Sounds")]
    [SerializeField] private AudioClip[] sfx;
    [SerializeField] private AudioClip mainMusic;


    [Header("Audio Sources")] 
    [SerializeField] private AudioSource musicSource;
    [SerializeField] private AudioSource sfxSource;
    
    public static AudioManager Instance;

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
    
    public void SoundExample(int _soundToplay, bool _loop = false)
    {
        sfxSource.clip = sfx[_soundToplay];
        sfxSource.loop = _loop;
        sfxSource.Play();
    }

    public void StopSound()
    {
        sfxSource.Stop();
    }
}


