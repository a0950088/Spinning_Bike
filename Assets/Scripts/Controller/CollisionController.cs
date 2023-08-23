using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Globalization;

public class CollisionController : MonoBehaviour
{
    private Danger_jumpin_message danger_win;
    public AudioSource crashSE;
    void Start()
    {
        danger_win = GameObject.FindObjectOfType<Danger_jumpin_message>();
    }

    void OnCollisionEnter(Collision other)
    {
        if (other.gameObject.CompareTag("obstacle"))
        {
            print("!!!!!crushed obstacle.");
            string SEString = PlayerPrefs.GetString("SEValue");
            float SEnum = float.Parse(SEString, CultureInfo.InvariantCulture.NumberFormat);
            crashSE.volume = SEnum;
            crashSE.Play();
            danger_win.win_on = true;
            string crash_str = PlayerPrefs.GetString("crash_num");
            int crash_num = int.Parse(crash_str, CultureInfo.InvariantCulture.NumberFormat);
            crash_num += 1;
            PlayerPrefs.SetString("crash_num", crash_num.ToString());
        }
    }

}
