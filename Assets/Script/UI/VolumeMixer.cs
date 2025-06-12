using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

public class VolumeMixer : MonoBehaviour
{

    [SerializeField] AudioMixer mixer;

    public void ChangeVolumeMusic(float v)
    {
        mixer.SetFloat("VolMusic", v);
    }

    public void ChangeVolumeFX(float v)
    {
        mixer.SetFloat("VolSFX", v);
    }

}
