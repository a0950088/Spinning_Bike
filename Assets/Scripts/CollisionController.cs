using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CollisionController : MonoBehaviour
{
    public Danger_jumpin_message danger_win;

    void OnCollisionEnter(Collision other)
    {
        if (other.gameObject.CompareTag("obstacle")){
            danger_win.win_on = true;
        }
    }
}
