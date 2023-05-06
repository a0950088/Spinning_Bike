using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Video;

public class ObjectGenerator : MonoBehaviour
{
	public GameObject bike, prefab;
	// public GameObject[] objectInstance;
	
	private VideoController videoController;
	private Danger_jumpin_message danger_win;

	private VideoPlayer videoPlayer;
	private float videoWidth;
    private float videoHeight;

	private LoadJsonData jsondata;
	private long frameIndex;
	private float[] objects;

	private Vector3[] ObjectPosition;
	private float initPositionZ = 900f;

	private Transform playerPos;

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
        danger_win = GameObject.FindObjectOfType<Danger_jumpin_message>();
        playerPos = bike.GetComponent<Transform>();
	}

	void FixedUpdate()
	{
		if (videoPlayer.isPrepared && videoController.nowframe > 0 && videoPlayer.frame > 0)
		{
			frameIndex = jsondata.dataWrapper.FrameData[videoController.nowframe].frame;
            objects = jsondata.dataWrapper.FrameData[videoController.nowframe].objects_position;

            // Debug.Log(objects);

            if (objects.Length != 0)
            {
            	CreateObjects();
            	Debug.Log("objects generated.");
            }
            // else
            // {
            // 	DeleteObjects();
            // }
		}
	}
	
	int normPlayer(float x)
	{
    	return (int) System.Math.Floor(260 * x) - 130;
    }

	void CreateObjects()
	{
		// if (objectInstance != null)
		// {
		// 	DeleteObjects();
		// }
		int NumOfObjects = objects.Length / 4;	// x, y, w, h
		for (int i = 0; i < NumOfObjects; i += 4)
		{
			Vector2 objectPointL = new Vector2(objects[i], objects[i+1]);
			float objectWidth = objects[i+2];
			float objectHeight = objects[i+3];
			Vector2 objectPointR = new Vector2(objectPointL.x + objectWidth, objectPointL.y + objectHeight);

			if (playerPos.position.x >= objectPointL.x && playerPos.position.x <= objectPointR.x && playerPos.position.y >= objectPointL.y && playerPos.position.y <= objectPointR.y)
			{
				danger_win.win_on = true;
			}

			// ObjectPosition[i/4] = new Vector3(normPlayer(objectPoint.x/videoWidth), objectPoint.y/videoHeight, initPositionZ);
			// objectInstance[i/4] = Instantiate(prefab, ObjectPosition[i/4], Quaternion.identity);
			// objectInstance[i/4].transform.localScale = new Vector3(objectWidth/videoWidth, objectHeight/videoHeight, 1);
		}
	}

	// void DeleteObjects()
	// {
	// 	foreach (GameObject instance in objectInstance)
	// 	{
	// 		Destroy(instance);	
	// 	}
	// }
}