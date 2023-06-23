using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;

public class Get_Score_info : MonoBehaviour
{
    private TMP_Text score;
    private TMP_Text miss;
    private TMP_Text time;
    // Start is called before the first frame update
    void Start()
    {
        string Score_num = PlayerPrefs.GetString("Final_point_str");
        score = GameObject.Find("Score").GetComponent<TMP_Text>();
        score.text = "Score " + Score_num;
        miss = GameObject.Find("Miss").GetComponent<TMP_Text>();

        string crash_num = PlayerPrefs.GetString("crash_num");
        if(crash_num==""){
            miss.text="Miss 0";
        }
        else{
            miss.text="Miss "+crash_num;
        }
        
        
        time = GameObject.Find("Play_Time").GetComponent<TMP_Text>();
        string time_total=PlayerPrefs.GetString("Time_total");
        int second=int.Parse(time_total);
        int min=second/60;
        second=second%60;
        int hour=min/60;
        min=min%60;
        string time_h_m_s="";
        if(hour<10){
            time_h_m_s+="0";
            time_h_m_s+=hour.ToString();
        }
        else{
            time_h_m_s+=hour.ToString();
        }
        time_h_m_s+=":";
        if(min<10){
            time_h_m_s+="0";
            time_h_m_s+=min.ToString();
        }
        else{
            time_h_m_s+=min.ToString();
        }
        time_h_m_s+=":";
        if(second<10){
            time_h_m_s+="0";
            time_h_m_s+=second.ToString();
        }
        else{
            time_h_m_s+=second.ToString();
        }

        time.text="Play Time "+time_h_m_s;

    }
    public void LoadScene(string sceneName)
    {
        SceneManager.LoadScene(sceneName);
    }

    
}
