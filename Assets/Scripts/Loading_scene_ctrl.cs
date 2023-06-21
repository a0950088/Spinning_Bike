using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.IO;
using System;
using TMPro;

public class Loading_scene_ctrl : MonoBehaviour
{
    public GameObject load;
    public static string buttonName;
    private TMP_Text str1;
    public void Start()
    {
        load.SetActive(true);
        str1=GameObject.Find("Loading_text").GetComponent<TMP_Text>();
    }
    public void Update()
    {
        if(TCP_Client.conn_state==1){
            //load.SetActive(false);
            Debug.Log("Connected");
            LoadScene("PlayScene");
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
        StartCoroutine(WaitAndContinue(sceneName));
    }
    IEnumerator WaitAndContinue(string sceneName)
    {
        str1.text="3";
        yield return new WaitForSecondsRealtime(1);
        str1.text="2";
        yield return new WaitForSecondsRealtime(1);
        str1.text="1";
        yield return new WaitForSecondsRealtime(1);
        Debug.Log("Just Check");
        SceneManager.LoadScene(sceneName);
    }
}
