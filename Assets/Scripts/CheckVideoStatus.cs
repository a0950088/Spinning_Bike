using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.IO;

public class CheckVideoStatus : MonoBehaviour
{
    public GameObject listPanel;

    public void CheckAllVideoStatus()
    {
        // 搜尋 List Panel 下的所有按鈕
        Button[] buttons = listPanel.GetComponentsInChildren<Button>();

        // 檢查按鈕是否已存在於列表中
        foreach (Button button in buttons)
        {
            ChangeStatus(button);
        }
    }


    public void ChangeStatus(Button videoButton)
    {
        string buttonName = videoButton.gameObject.name; // 取得按鈕的名字
        if (buttonName != "AddVideo")
        {
            if (!JsonExists(buttonName))
            { 
                videoButton.interactable = false;
                //改一下 TCP_Client 下面的 processPath
                string videoFileName = buttonName + ".mp4";
                string videoPath = Path.Combine(Application.dataPath, "Video", videoFileName);
                tcpClient = GameObject.Find("Server").GetComponent<TCP_Client>();
                tcpClient.processPath = videoPath;
            }
            else
            {
                videoButton.interactable = true;
            }
        }
    }

    private bool JsonExists(string buttonName)
    {
        string jsonFileName = buttonName + ".json";
        string jsonPath = Path.Combine(Application.dataPath, "JsonData", jsonFileName);
        return File.Exists(jsonPath);
    }

}
