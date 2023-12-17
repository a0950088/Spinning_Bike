using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Globalization;

public class SetVolume : MonoBehaviour
{
    public Slider BGM;
    public Slider SE;
    float BGMValue;
    float SEValue;

    void Start()
    {
        if (PlayerPrefs.GetString("BGMValue") == null)
        {
            BGMValue = 1;
            PlayerPrefs.SetString("BGMValue", BGMValue.ToString());
        }
        else
        {
            string BGMString = PlayerPrefs.GetString("BGMValue");
            float BGMnum = float.Parse(BGMString);
            BGM.value = BGMnum;
        }
        if (PlayerPrefs.GetString("SEValue") == null)
        {
            SEValue = 1;
            PlayerPrefs.SetString("SEValue", SEValue.ToString());
        }
        else
        {
            string SEString = PlayerPrefs.GetString("SEValue");
            float SEnum = float.Parse(SEString);
            SE.value = SEnum;
        }
    }
    public void PassValue()
    {
        BGMValue = BGM.value;
        SEValue = SE.value;
        PlayerPrefs.SetString("BGMValue", BGMValue.ToString());
        PlayerPrefs.SetString("SEValue", SEValue.ToString());
    }
}
