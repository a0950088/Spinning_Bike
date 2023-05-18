using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class MessageDisplay : MonoBehaviour
{
    public TMP_Text messageText;

    public void NoMessage()
    {
        if (messageText != null)
        {
            messageText.text = "";
        }
    }

    public void VideoNotReady()
    {
        if (messageText != null)
        {
            messageText.text = "Video Not Ready"; 
        }
    }

}
