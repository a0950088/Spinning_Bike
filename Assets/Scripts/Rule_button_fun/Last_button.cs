using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Last_button : MonoBehaviour
{
    public GameObject rule1;
    public GameObject rule2;
    public GameObject rule3;
    public GameObject rule4;
    public GameObject last_arrow;
    public GameObject next_arrow;
    public void On_Click_last(){
        Next_button.Which_rule--;
        if(Next_button.Which_rule==1){
            rule2.SetActive(false);
            rule1.SetActive(true);
            last_arrow.SetActive(false);
        }
        else if(Next_button.Which_rule==2){
            rule3.SetActive(false);
            rule2.SetActive(true);
        }
        else if(Next_button.Which_rule==3){
            rule4.SetActive(false);
            rule3.SetActive(true);
            next_arrow.SetActive(true);
        }
    }
}
