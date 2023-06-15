using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Get_play_resulr : MonoBehaviour
{
    public int final_score;
    private TMP_Text scores;
    // Start is called before the first frame update
    void Start()
    {
        scores = GameObject.Find("Score_text").GetComponent<TMP_Text>();
        Debug.Log("scores: "+scores);
        
    }

   
}
