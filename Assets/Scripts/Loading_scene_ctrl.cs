using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.IO;

public class Loading_scene_ctrl : MonoBehaviour
{
    public GameObject load;
    public static string buttonName;
    public void Start()
    {
        if(TCP_Client.conn_state==1){
            load.SetActive(false);
            LoadScene("PlayScene");
            Debug.Log("Connected");
        }
        else{
            load.SetActive(true);
            Debug.Log("Nothing");
        }
    }
    public void Update()
    {
        if(TCP_Client.conn_state==1){
            load.SetActive(false);
            LoadScene("PlayScene");
            Debug.Log("Connected");
        }
        else{
            Debug.Log("Nothing");
        }
        
    }
    public void LoadScene(string sceneName)
    {
        //string buttonName = gameObject.name;
        string videoFolder = Path.Combine(Application.dataPath, "Video");
        string videoPath = Path.Combine(videoFolder, buttonName + ".mp4");
        string jsonFolder = Path.Combine(Application.dataPath, "JsonData");
        string jsonPath = Path.Combine(jsonFolder, buttonName + ".json");

        PlayerPrefs.SetString("VideoPath", videoPath);
        PlayerPrefs.SetString("JsonPath", jsonPath);

        // string videoPath = PlayerPrefs.GetString("VideoPath");
        string getPath = PlayerPrefs.GetString("VideoPath");
        Debug.Log("VideoPath: " + getPath);

        SceneManager.LoadScene(sceneName);
    }
    
}
