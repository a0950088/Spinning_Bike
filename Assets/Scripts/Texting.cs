using System;
using System.Collections;
using TMPro;
using UnityEngine;

public class Texting : MonoBehaviour
{
    public float charsPerSecond = 0.05f; // typing speed. The less the value is, the faster word's typed
    private string words;
    private bool isActive = false;
    private float timer; //counter
    public TMP_Text myText;
    private int currentPos = 0;
    //private AudioSource TypingSound;    // typing sound

    void Start()
    {
        timer = 0;
        /* start effect */
        isActive = true;
        charsPerSecond = Mathf.Max(0.05f, charsPerSecond);
        myText = this.GetComponent<TMP_Text>();
        /* record full content */
        words = myText.text;
        /* record the content step by step */
        myText.text = "";
        //TypingSound = GetComponent<AudioSource>();
        //TypingSound.UnPause();
    }

    void Update()
    {
        OnStartWriter();
    }

    void OnStartWriter()
    {
        if (isActive)
        {
            timer += Time.deltaTime;
            if (timer >= charsPerSecond)
            {
                timer = 0;
                currentPos++;
                myText.text = words.Substring(0, currentPos);

                if (currentPos >= words.Length)
                {
                    OnFinish();
                }
            }

        }
    }

    void OnFinish()
    {
        isActive = false;
        timer = 0;
        currentPos = 0;
        myText.text = words;
        //TypingSound.Pause();
    }
}