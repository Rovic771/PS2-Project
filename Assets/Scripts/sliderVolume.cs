using UnityEngine;
using UnityEngine.Audio;

public class sliderVolume : MonoBehaviour
{
    [SerializeField] private AudioMixer mixer;

    public void OnChangeSlider(float value)
    {
        mixer.SetFloat("Volume", Mathf.Log10(value) * 20);
    }
}
