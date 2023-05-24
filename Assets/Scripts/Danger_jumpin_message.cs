using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Video;

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

    void Start()
    {
        player = GameObject.FindObjectOfType<PlayerController>();
        obstacle = GameObject.FindObjectOfType<ObstacleController>();
        videoPlayer = GameObject.Find("Screen").GetComponent<VideoPlayer>();
        wins = GameObject.FindGameObjectsWithTag("danger_window");
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
    }
}

