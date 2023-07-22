using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Last_button : MonoBehaviour
{
    public GameObject rule1_photo;
    public GameObject rule2_photo;
    public GameObject rule3_photo;
    public GameObject rule4_photo;

    public GameObject rule1_text;
    public GameObject rule2_text;
    public GameObject rule3_text;
    public GameObject rule4_text;

    public GameObject last_arrow;
    public GameObject next_arrow;

    public void On_Click_last(){
        Next_button.Which_rule--;
        if(Next_button.Which_rule==1){
            rule2_photo.SetActive(false);
            rule1_photo.SetActive(true);
            rule2_text.SetActive(false);
            rule1_text.SetActive(true);
            last_arrow.SetActive(false);
        }
        else if(Next_button.Which_rule==2){
            rule3_photo.SetActive(false);
            rule2_photo.SetActive(true);
            rule3_text.SetActive(false);
            rule2_text.SetActive(true);
        }
        else if(Next_button.Which_rule==3){
            rule4_photo.SetActive(false);
            rule3_photo.SetActive(true);
            rule4_text.SetActive(false);
            rule3_text.SetActive(true);
            next_arrow.SetActive(true);
        }
    }
}
