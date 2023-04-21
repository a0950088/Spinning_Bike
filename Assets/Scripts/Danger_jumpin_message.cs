using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Video;

public class Danger_jumpin_message : MonoBehaviour
{
    private GameObject[] wins;
    private bool crushed = false;
    public bool win_on { get => crushed; set => crushed = value; }
    
    public PlayerController player;
    public ObstacleController obstacle;
    private VideoPlayer videoPlayer;

    void Start()
    {
        videoPlayer = GameObject.Find("Screen").GetComponent<VideoPlayer>();
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
            videoPlayer.playbackSpeed = 0;
            player.WheelSpeed = 0.0f;
            player.turnSpeed = 0.0f;
            player.ridingSpeed = 0.0f;
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
            // yield return new WaitForSecondsRealtime(1);
            yield return new WaitForSeconds(1f);
            Sec--;
        }
        /* close the window after 5 sec and back to the 20 frame before */
        if (win_on) {
            win_on = false;
            videoPlayer.frame = videoPlayer.frame - 40;
        }
        videoPlayer.playbackSpeed = 1;
        player.WheelSpeed = 40.0f;
        player.turnSpeed = 30.0f;
        player.ridingSpeed = 30.0f;
        obstacle.initSpeed = 0.1f;
        obstacle.initScale = 0.01f;
    }
}
