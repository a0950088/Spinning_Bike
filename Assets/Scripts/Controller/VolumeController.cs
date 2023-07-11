using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Globalization;

public class VolumeController : MonoBehaviour
{
    public AudioSource Sound;

    void Update(){
        string BGMValue=PlayerPrefs.GetString("BGMValue");
        float Volume=float.Parse(BGMValue, CultureInfo.InvariantCulture.NumberFormat);
        Sound.volume=Volume;
    }
}
