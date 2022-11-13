using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

public class SliderVolumeScript : MonoBehaviour
{
    [SerializeField] private Slider musicSlider;
    [SerializeField] private Slider sFXSlider;
    [SerializeField] private AudioMixer mixer;
    
    private const string MixerMusic = "MusicVolume";
    private const string MixerSfx = "SFXVolume";

    private void Awake()
    {
        AddEventListenSliders();
    }

    private void Start()
    {
        DisplaySliderValue();
        SetValueSliderMixer();
    }

    private void AddEventListenSliders()
    {
        musicSlider.onValueChanged.AddListener(SetMusicVolume);
        sFXSlider.onValueChanged.AddListener(SetSFXVolume);
    }
    
    
    private void DisplaySliderValue()
    {
       
        if (PlayerPrefs.HasKey("MusicVolume"))
        {
            musicSlider.value = PlayerPrefs.GetFloat("MusicVolume");
            sFXSlider.value = PlayerPrefs.GetFloat("SFXVolume");
           
        }
        else
        {
          
            musicSlider.value = musicSlider.maxValue/2;
            sFXSlider.value = sFXSlider.maxValue/2;
            PlayerPrefs.SetFloat("MusicVolume", musicSlider.value);
            PlayerPrefs.SetFloat("SFXVolume", sFXSlider.value);
            PlayerPrefs.Save();
        }
    }
  
    private void SetValueSliderMixer()
    {
        SetMusicVolume(musicSlider.value);
        SetSFXVolume(sFXSlider.value);
    }
    
    private void SetMusicVolume(float value)
    {
        mixer.SetFloat(MixerMusic,Mathf.Log10(value)*20);
        if (musicSlider.value <= 0.005)
        {
            mixer.SetFloat(MixerMusic, -80);
        }
        PlayerPrefs.SetFloat("MusicVolume", value);
        PlayerPrefs.Save();
    }
    private void SetSFXVolume(float value)
    {
        mixer.SetFloat(MixerSfx, Mathf.Log10(value) * 20);
        if (sFXSlider.value <= 0.005)
        {
            mixer.SetFloat(MixerSfx, -80);
        }
        PlayerPrefs.SetFloat("SFXVolume", value);
        PlayerPrefs.Save();
    }
}
