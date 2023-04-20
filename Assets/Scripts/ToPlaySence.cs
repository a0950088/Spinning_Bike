using UnityEngine;
using System.Collections;
using UnityEngine.UI; //Button
using UnityEngine.SceneManagement; //SceneManager

public class ToPlaySence : MonoBehaviour
{
    public int sceneIndex = 1; //switch to which sence

    void Start()
    {
        //button click trigger ClickEvent
        GetComponent<Button>().onClick.AddListener(() => {
            ClickEvent();
        });
    }

    void ClickEvent()
    {
        //switch Scene
        SceneManager.LoadScene(sceneIndex);
    }
}
