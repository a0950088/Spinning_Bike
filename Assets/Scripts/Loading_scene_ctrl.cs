using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Loading_scene_ctrl : MonoBehaviour
{
    public GameObject load;
    void Start()
    {
        load=GameObject.FindGameObjectWithTag("loading");
        if(TCP_Client.conn_state==1){
            load.SetActive(false);
            Debug.Log("Connected");
        }
        else{
            Debug.Log("Nothing");
        }

        
    }
    void Update()
    {
        if(TCP_Client.conn_state==1){
            load.SetActive(false);
            Debug.Log("Connected");
        }
        else{
            Debug.Log("Nothing");
        }
    }
    
}
