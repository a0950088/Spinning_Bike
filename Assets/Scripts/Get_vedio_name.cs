using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Get_vedio_name : MonoBehaviour
{
    public void Start()
    {
        Loading_scene_ctrl.buttonName=gameObject.name;
        Debug.Log("Vedio name = "+Loading_scene_ctrl.buttonName);
        
    }

}
