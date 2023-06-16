using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;

public class Get_Score_info : MonoBehaviour
{
    private TMP_Text score;
    // Start is called before the first frame update
    void Start()
    {
        string Score_num = PlayerPrefs.GetString("Final_point_str");
        score = GameObject.Find("Score").GetComponent<TMP_Text>();
        score.text = "Score:" + Score_num;
    }
    public void LoadScene(string sceneName)
    {
        SceneManager.LoadScene(sceneName);
    }

    
}
