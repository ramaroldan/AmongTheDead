using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class VolumeSystem : MonoBehaviour
{
    [SerializeField] Slider slider;
    [SerializeField] float sliderValue;
    [SerializeField] Image imgMute;

    // Start is called before the first frame update
    void Start()
    {
        slider.value = PlayerPrefs.GetFloat("Volume",0.5f); //al inicio esta en 0.5
        AudioListener.volume = slider.value; 
        Mute();
    }

    public void ChangeSlider(float value) //Para cambiar el volumen desde el slider
    {
        sliderValue = value;
        PlayerPrefs.SetFloat("Volume", sliderValue);
        AudioListener.volume = slider.value;
        Mute();
    }

    public void Mute()
    {
        if (slider.value == 0)
        {
            imgMute.enabled = true;
        }
        else
        {
            imgMute.enabled = false;
        }
    }
}
