using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Video;
using TMPro;

public class LaneLineController : MonoBehaviour
{
	public GameObject bike;
	public GameObject minus_point;
	private VideoController videoController;

	private VideoPlayer videoPlayer;
	private float videoWidth;
    private float videoHeight;

	private LoadJsonData jsondata;
	private long frameIndex;
	private float[] left_line;
	private float[] right_line;
	private string direction;

	private Transform playerPos;

	private int points = 100;
	private TMP_Text scores;

	private TMP_Text direction_hit;

	void Start()
	{
		videoPlayer = GameObject.Find("Screen").GetComponent<VideoPlayer>();
        videoPlayer.prepareCompleted += (source) =>
        {
            videoWidth = videoPlayer.width;
            videoHeight = videoPlayer.height;           
            bike = GameObject.FindGameObjectWithTag("Bike");
            videoController = GameObject.FindObjectOfType<VideoController>();
            jsondata = GameObject.FindObjectOfType<LoadJsonData>();
        };
        playerPos = bike.GetComponent<Transform>();
        minus_point = GameObject.Find("score_minus");
        scores = GameObject.Find("Score_text").GetComponent<TMP_Text>();
        direction_hit = GameObject.Find("hint").GetComponent<TMP_Text>();
	}

	void FixedUpdate()
	{
		if (videoPlayer.isPrepared && videoController.nowframe > 0 && videoPlayer.frame > 0)
		{
			frameIndex = jsondata.dataWrapper.FrameData[videoController.nowframe].frame;
            left_line = jsondata.dataWrapper.FrameData[videoController.nowframe].left_line_range;
            right_line = jsondata.dataWrapper.FrameData[videoController.nowframe].right_line_range;
            direction = jsondata.dataWrapper.FrameData[videoController.nowframe].direction;

            UpdateLaneLine();
		}
	}
	
	int normPlayer(float x)
	{
    	return (int) System.Math.Floor(260 * x) - 130;
    }

	void UpdateLaneLine()
	{
		Vector2 pointNearL = new Vector2(left_line[0], left_line[1]);
		Vector2 pointNearR = new Vector2(right_line[0], right_line[1]);

		float left_lane_line = normPlayer(pointNearL.x / videoWidth);
		float right_lane_line = normPlayer(pointNearR.x / videoWidth);
		
		if (playerPos.position.x > right_lane_line || playerPos.position.x < left_lane_line)
		{
			// Debug.Log("left x: " + left_lane_line);
			// Debug.Log("right x: " + right_lane_line);
			// Debug.Log("player: " + playerPos.position.x);
			minusPoints();
			minus_point.SetActive(true);
			scores.text = "SCORE: " + points.ToString();
		}
		else
		{
			minus_point.SetActive(false);
		}
		direction_hit.text = direction;
	}

	void minusPoints()
	{
		if (points > 0)
		{
			points -= 1;	
		}
		else
		{
			points = 0;
		}
	}
}