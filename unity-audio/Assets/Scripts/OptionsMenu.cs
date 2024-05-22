using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class OptionsMenu : MonoBehaviour
{
    public SettingsSO settings;
    public Toggle RotationInverstion;
    public Slider VolumeSlider;
    public Slider SFXSlider;
    public AudioMixer mixer;



    private void Start()
    {

        VolumeSlider.value = PlayerPrefs.GetFloat("BG");
        SFXSlider.value = PlayerPrefs.GetFloat("SFX");
        RotationInverstion.isOn = PlayerPrefs.GetInt("Inversion") == 1 ? true : false;

        RotationInverstion.onValueChanged.AddListener(RotationDirection);
        VolumeSlider.onValueChanged.AddListener(UpdateVolumeBG);
        SFXSlider.onValueChanged.AddListener(UpdateVolumeMenu);

       
    }

    private void UpdateVolumeMenu(float value)
    {
        if (value > 0)
        {
            value = 20.0f * Mathf.Log10(value);
        }
        else
        {
            value = -80;
        }
        // MenuVolume , AmbienceVolume
        mixer.SetFloat("MenuVolume", value);
        mixer.SetFloat("AmbienceVolume", value);


        SFXSlider.value = Mathf.Pow(10.0f, value / 20.0f);
    }


    private void UpdateVolumeBG(float value)
    {
        if(value > 0)
        {
            value = 20.0f * Mathf.Log10(value);
        }
        else
        {
            value = -80;
        }
      // MenuVolume , AmbienceVolume
        mixer.SetFloat("BGMVolume", value);
       

        VolumeSlider.value = Mathf.Pow(10.0f, value / 20.0f);
    }

    private void OnDisable()
    {
        PlayerPrefs.SetFloat("SFX", SFXSlider.value);
        PlayerPrefs.SetFloat("BG", VolumeSlider.value);
        PlayerPrefs.SetInt("Inversion",  RotationInverstion.isOn ? 1 : 0);
        PlayerPrefs.Save();
    }

    private void RotationDirection(bool state)
    {

        settings.isInverted = state;
        RotationInverstion.isOn = state;
    }

    [System.Obsolete]
    public void Back()
    {
        SceneManager.LoadScene(settings.PreviousScene);
    }
}
