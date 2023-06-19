using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CollisionController : MonoBehaviour
{
    private Danger_jumpin_message danger_win;
    private int crash_num=0;
    void Start()
    {
        danger_win = GameObject.FindObjectOfType<Danger_jumpin_message>();
    }

    void OnCollisionEnter(Collision other)
    {
        if (other.gameObject.CompareTag("obstacle")){
            print("!!!!!crushed obstacle.");
            danger_win.win_on = true;
            crash_num+=1;
            PlayerPrefs.SetString("crash_num", crash_num.ToString());
        }
    }

}
