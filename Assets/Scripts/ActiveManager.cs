using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActiveManager : MonoBehaviour
{
    public GameObject Obj;
    private GameObject startButon;
    private GameObject optionButton;
    private GameObject exitButton;
    private GameObject title;

    void Start()
    {
        startButon = GameObject.Find("StartButton");
        optionButton = GameObject.Find("OptionButton");
        exitButton = GameObject.Find("ExitButton");
        title = GameObject.Find("Title");
    }



    public void SetVisible()
    {
        if (Obj != null)
        {
            Obj.SetActive(true);
        }
    }

    public void SetInvisible()
    {
        if (Obj != null)
        {
            Obj.SetActive(false);
        }
    }


    public void StartVisible()
    {
        startButon.SetActive(true);
        optionButton.SetActive(true);
        exitButton.SetActive(true);
        title.SetActive(true);
    }

    public void StartInvisible()
    {
        startButon.SetActive(false);
        optionButton.SetActive(false);
        exitButton.SetActive(false);
        title.SetActive(false);
    }

    

}
