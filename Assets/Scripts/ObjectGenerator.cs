using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Video;

public class ObjectGenerator : MonoBehaviour
{
	public GameObject bike;
	private VideoController videoController;

	private VideoPlayer videoPlayer;
	private float videoWidth;
    private float videoHeight;

	private LoadJsonData jsondata;
	private long frameIndex;
	private float[] objects;

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
	}

	void FixedUpdate()
	{
		if (videoPlayer.isPrepared && videoController.nowframe > 0 && videoPlayer.frame > 0)
		{
			frameIndex = jsondata.dataWrapper.FrameData[videoController.nowframe].frame;
            objects = jsondata.dataWrapper.FrameData[videoController.nowframe].objects_position;
		}
	}
	
	int normPlayer(float x)
	{
    	return (int) System.Math.Floor(260 * x) - 130;
    }

	void rewind()
	{
		videoPlayer.frame = videoPlayer.frame - 40;
	}

	void CreateObjects()
	{
		
	}

	void updateObjects()
	{

	}
}