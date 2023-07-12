using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Video;
using TMPro;

public class ObjectController : MonoBehaviour
{
	public GameObject bike;
	
	private VideoController videoController;
	private Danger_jumpin_message danger_win;

	private VideoPlayer videoPlayer;
	private float videoWidth;
    private float videoHeight;

	private LoadJsonData jsondata;
	private long frameIndex;
	private float[] objects;

	private Vector3[] ObjectPosition;

	private Transform playerPos;
	private int MAX_X = 130;
	private int MAX_Y;

	private TMP_Text hint;
	
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
        MAX_Y = calNormRatio(MAX_X);
		hint=GameObject.Find("hint").GetComponent<TMP_Text>();
	}

	void FixedUpdate()
	{
		if (videoPlayer.isPrepared && videoController.nowframe > 0 && videoPlayer.frame > 0)
		{
			frameIndex = jsondata.dataWrapper.FrameData[videoController.nowframe].frame;
            objects = jsondata.dataWrapper.FrameData[videoController.nowframe].objects_position;

            if (objects.Length != 0)
            {
				hint.text="Watch out for pedestrians/vehicles ahead";
				//Debug.Log("object find in frame "+frameIndex);
            	CreateObjects();
            }
			else{
				hint.text="Safe";
			}
		}
	}

	int calNormRatio(int x) 
	{
		return (int) System.Math.Floor(x * 2 * videoHeight / videoWidth);
	}
	
	int[] normPlayer(float x, float y)
	{
		List<int> tempList = new List<int>();
		tempList.Add((int) System.Math.Floor(-(MAX_X*2) * x/videoWidth) + MAX_X);
		tempList.Add((int) System.Math.Floor(-(MAX_Y*2) * y/videoHeight) + MAX_Y);
		int[] new_point = tempList.ToArray();
    	return new_point;
    }

	void CreateObjects()
	{
		int NumOfObjects = objects.Length / 4;

		for (int i = 0; i < NumOfObjects; i += 4)
		{
			Vector2 objectPointL = new Vector2(objects[i], objects[i+1]);
			Vector2 objectPointR = new Vector2(objects[i] + objects[i+2], objects[i+1] + objects[i+3]);

			int[] tmp_point = normPlayer(objectPointL.x, objectPointL.y);
			float Left_x = tmp_point[0];
			float Left_y = tmp_point[1];

			tmp_point = normPlayer(objectPointR.x, objectPointR.y);
			float Right_x = tmp_point[0];
			float Right_y = tmp_point[1];

			float player_posx = playerPos.position.x;
			float player_posy = playerPos.position.y;

			// Debug.Log(frameIndex);
			// Debug.Log("left x: " + Left_x);
			// Debug.Log("player x: " + player_posx);
			// Debug.Log("right x: " + Right_x);
			// Debug.Log("left y: " + Left_y);
			// Debug.Log("player y: " + player_posy);
			// Debug.Log("right y: " + Right_y);

			float min_x = System.Math.Min(Left_x, Right_x);
			float max_x = System.Math.Max(Left_x, Right_x);
			float min_y = System.Math.Min(Left_y, Right_y);
			float max_y = System.Math.Max(Left_y, Right_y);

			if (player_posx >= min_x && player_posx <= max_x && player_posy >= min_y && player_posy <= max_y)
			{
				danger_win.win_on = true;
				// Debug.Log("!!!!!!Crushed: " + frameIndex);
			}
		}
	}
}