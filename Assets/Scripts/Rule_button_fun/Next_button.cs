using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Next_button : MonoBehaviour
{
    public GameObject rule1;
    public GameObject rule2;
    public GameObject rule3;
    public GameObject rule4;
    public GameObject next_arrow;
    public GameObject last_arrow;
    public static int Which_rule=1;
    public void On_Click_next(){
        Which_rule++;
        if(Which_rule==2){
            rule1.SetActive(false);
            rule2.SetActive(true);
            last_arrow.SetActive(true);
        }
        else if(Which_rule==3){
            rule2.SetActive(false);
            rule3.SetActive(true);
        }
        else if(Which_rule==4){
            rule3.SetActive(false);
            rule4.SetActive(true);
            next_arrow.SetActive(false);
        }
    }
}
