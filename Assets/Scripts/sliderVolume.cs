using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

public class sliderVolume : MonoBehaviour
{
    [SerializeField] private AudioMixer mixer;

    private void Start()
    {
        mixer.SetFloat("Volume", Mathf.Log10(PlayerPrefs.GetFloat("Volume",1)) * 20);
        GetComponent<Slider>().value = PlayerPrefs.GetFloat("Volume");
    }

    public void OnChangeSlider(float value)
    {
        mixer.SetFloat("Volume", Mathf.Log10(value) * 20);
        if (value == 0)
        {
            mixer.SetFloat("Volume", -80);
        }
        PlayerPrefs.SetFloat("Volume", value);
        PlayerPrefs.Save();
    }
}
