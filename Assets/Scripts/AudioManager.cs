using UnityEngine;
using UnityEngine.InputSystem;

public class AudioManager : MonoBehaviour
{
    private AudioSource audioSource;
    [SerializeField] AudioClip GuitardTireRe, GuitardTireDo, GuitardZoneRe, GuitardZoneDo;
   

    [Range(0f, 1f)]
    public float volume;

    [Range(0.1f, 2.5f)]
    public float pitch;

    private AudioSource source;

   void Awake()
    {
        gameObject.AddComponent<AudioSource>();
        source = GetComponent <AudioSource>();

        volume = 0.5f;
        pitch = 1f;
    }





    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        source.clip = GuitardTireRe;
        source.volume = volume;
        source.pitch = pitch;
        audioSource = GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {
        if (Gamepad.current.leftTrigger.wasPressedThisFrame)
        {
            audioSource.PlayOneShot(GuitardTireDo);
        }

        if (Gamepad.current.rightTrigger.wasPressedThisFrame)
        {
            audioSource.PlayOneShot(GuitardTireRe);
        }

        if (Gamepad.current.leftShoulder.wasPressedThisFrame)
        {
            audioSource.PlayOneShot(GuitardZoneDo);
        }

        if (Gamepad.current.rightShoulder.wasPressedThisFrame)
        {
            audioSource.PlayOneShot(GuitardZoneRe);
        }
    }
}
