using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class PauseWindowController : MonoBehaviour
{
    public GameObject PauseWindow;
    public TMP_Text PauseMess; 
    TCP_Client tcpClient;
    void Start()
    {
        tcpClient = TCP_Client.Instance;
        PauseWindow.SetActive(false);
    }

    void Update()
    {
        if(tcpClient.conn_state==1){
            PauseWindow.SetActive(false);
        }
        else{
            PauseWindow.SetActive(true);
        }
        
    }
}
