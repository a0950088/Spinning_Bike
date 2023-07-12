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

    void Start(){
        if(PlayerPrefs.GetString("BGMValue")==null){
            BGMValue=BGM.value;
            SEValue=SE.value;
            PlayerPrefs.SetString("BGMValue", BGMValue.ToString());
            PlayerPrefs.SetString("SEValue", SEValue.ToString());
        }
        else{
            string BGMString=PlayerPrefs.GetString("BGMValue");
            string SEString=PlayerPrefs.GetString("SEValue");
            float BGMnum=float.Parse(BGMString, CultureInfo.InvariantCulture.NumberFormat);
            float SEnum=float.Parse(SEString, CultureInfo.InvariantCulture.NumberFormat);
            BGM.value=BGMnum;
            SE.value=SEnum;
        }
    }
    public void PassValue()
    {
        BGMValue=BGM.value;
        SEValue=SE.value;
        PlayerPrefs.SetString("BGMValue", BGMValue.ToString());
        PlayerPrefs.SetString("SEValue", SEValue.ToString());
    }
}
