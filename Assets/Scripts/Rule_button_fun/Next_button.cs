using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Next_button : MonoBehaviour
{
    public GameObject rule1_photo;
    public GameObject rule2_photo;
    public GameObject rule3_photo;
    public GameObject rule4_photo;

    public GameObject rule1_text;
    public GameObject rule2_text;
    public GameObject rule3_text;
    public GameObject rule4_text;

    public GameObject next_arrow;
    public GameObject last_arrow;
    public static int Which_rule=1;

    public void On_Click_next(){
        Which_rule++;
        if(Which_rule==2){
            rule1_photo.SetActive(false);
            rule2_photo.SetActive(true);
            rule1_text.SetActive(false);
            rule2_text.SetActive(true);
            last_arrow.SetActive(true);
        }
        else if(Which_rule==3){
            rule2_photo.SetActive(false);
            rule3_photo.SetActive(true);
            rule2_text.SetActive(false);
            rule3_text.SetActive(true);
        }
        else if(Which_rule==4){
            rule3_photo.SetActive(false);
            rule4_photo.SetActive(true);
            rule3_text.SetActive(false);
            rule4_text.SetActive(true);
            next_arrow.SetActive(false);
        }
    }
}
