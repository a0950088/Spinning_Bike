using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Video;
using System.Globalization;

public class Danger_jumpin_message : MonoBehaviour
{
    private PlayerController player;
    private ObstacleController obstacle;
    private VideoPlayer videoPlayer;

    private GameObject[] wins;
    private bool crushed = false;
    public bool win_on { get => crushed; set => crushed = value; }

    private int init_counter = 500;
    private int counter;

    private bool crash_flag = false;
    public AudioSource crashSE;

    void Start()
    {
        player = GameObject.FindObjectOfType<PlayerController>();
        obstacle = GameObject.FindObjectOfType<ObstacleController>();
        videoPlayer = GameObject.Find("Screen").GetComponent<VideoPlayer>();
        wins = GameObject.FindGameObjectsWithTag("danger_window");
        crashSE = GameObject.Find("CrashSE").GetComponent<AudioSource>();
        foreach (GameObject win in wins)
        {
            win.SetActive(false);
        }
        counter = init_counter;
    }

    void Update()
    {
        if (win_on)
        {
            if (counter > 0)
            {
                Stop();
                if (counter == init_counter)
                {
                    string SEString = PlayerPrefs.GetString("SEValue");
                    float SEnum = float.Parse(SEString, CultureInfo.InvariantCulture.NumberFormat);
                    crashSE.volume = SEnum;
                    crashSE.Play();
                    string crash_str = PlayerPrefs.GetString("crash_num");
                    int crash_num = int.Parse(crash_str, CultureInfo.InvariantCulture.NumberFormat);
                    crash_num += 1;
                    PlayerPrefs.SetString("crash_num", crash_num.ToString());
                }
                counter--;
            }
            else
            {
                Resume();
                counter = init_counter;
            }
        }
        else
        {
            foreach (GameObject win in wins)
            {
                win.SetActive(false);
            }
        }
    }

    void Stop()
    {
        foreach (GameObject win in wins)
        {
            win.SetActive(true);
        }
        videoPlayer.playbackSpeed = 0;
        player.init_speed = 0.0f;
        obstacle.initSpeed = 0.0f;
        obstacle.initScale = 0.0f;
    }




    void Resume()
    {
        win_on = false;
        videoPlayer.frame = videoPlayer.frame - 40;
        videoPlayer.playbackSpeed = 1;
        player.init_speed = 0.5f;
        obstacle.initSpeed = 0.1f;
        obstacle.initScale = 0.01f;
        Destroy(obstacle.obstacleInstance);
    }
}

