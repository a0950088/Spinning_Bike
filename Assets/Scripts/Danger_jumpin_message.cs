using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Danger_jumpin_message : MonoBehaviour
{
    public GameObject[] wins;
    private bool crushed = false;
    public bool win_on { get => crushed; set => crushed = value; }
    public VideoController video;
    public PlayerController player;
    public ObstacleController obstacle;

    void Start()
    {
        wins = GameObject.FindGameObjectsWithTag("danger_window");
        foreach (GameObject win in wins)
        {
            win.SetActive(false);
        }
    }

    void Update()
    {
        if (win_on)
        {
            foreach (GameObject win in wins)
            {
                win.SetActive(true);
            }
            video.videoPy.playbackSpeed = 0;
            player.WheelSpeed = 0.0f;
            obstacle.initSpeed = 0.0f;
            obstacle.initScale = 0.0f;
            StartCoroutine("Countdown");
        }
        else
        {
            foreach (GameObject win in wins)
            {
                win.SetActive(false);
            }
        }
    }

    IEnumerator Countdown()
    {
        int Sec = 5;
        while (Sec > 0)
        {
            yield return new WaitForSecondsRealtime(1);
            Sec--;
        }

        if (win_on) {
            win_on = false;
            video.videoPy.frame = video.nowframe - 20;
        }

        video.videoPy.playbackSpeed = 1;
        player.WheelSpeed = 40.0f;
        obstacle.initSpeed = 0.1f;
        obstacle.initScale = 0.01f;
    }
}
