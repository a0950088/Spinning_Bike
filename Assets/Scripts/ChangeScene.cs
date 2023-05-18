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
        
        PlayerPrefs.SetString("VideoPath", videoPath);

        // 獲取按鈕的影片路徑
        // string videoPath = PlayerPrefs.GetString("VideoPath");
        string getPath = PlayerPrefs.GetString("VideoPath");
        Debug.Log("VideoPath: " + getPath);

        SceneManager.LoadScene(sceneName);
    }


}
