using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.IO;

public class ChangeScene : MonoBehaviour
{   

    public void LoadScene(string sceneName)
    {
        string buttonName = gameObject.name;
        string videoFolder = Path.Combine(Application.dataPath, "Video");
        string videoPath = Path.Combine(videoFolder, buttonName + ".mp4");
        string jsonFolder = Path.Combine(Application.dataPath, "JsonData");
        string jsonPath = Path.Combine(jsonFolder, buttonName + ".json");

        PlayerPrefs.SetString("VideoPath", videoPath);
        PlayerPrefs.SetString("JsonPath", jsonPath);

        // 獲取按鈕的影片路徑
        // string videoPath = PlayerPrefs.GetString("VideoPath");
        string getPath = PlayerPrefs.GetString("VideoPath");
        Debug.Log("VideoPath: " + getPath);

        SceneManager.LoadScene(sceneName);
    }


}
