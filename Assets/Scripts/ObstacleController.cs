using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Video;

public class ObstacleController : MonoBehaviour
{
    public GameObject prefab, obstacleInstance, bike;
    Vector3 initialPosition;

    private float verticalInput;
    private float initSpeed = 0.1f;
    private float initScale = 0.1f;
    private float yspeed = 0.1f;
    private float zspeed, yspeed_total;
    private float scale = 0.1f;

    private float deltaY;
    private float deltaZ;
    private float proportion;

    private VideoPlayer videoPlayer;
    private float videoWidth;

    private LoadJsonData data;

    private VideoController videoController;
    private long frameIndex;

    // public LoadJsonData jsonData;
    // public String stringData;
    // public FrameDataWrapper dataWrapper;

    // 1. if go staight, then possibly create object;
    // 2. when creating object, i need to initiate x: within lines, y: on the top point
    // 3. increase y value, and zoom in object size with speed and time

    void Start()
    {
        Debug.Log("Obstacle start");
        // loadData();
        
        // get video info
        videoPlayer = GameObject.Find("Screen").GetComponent<VideoPlayer>();
        videoPlayer.prepareCompleted += (source) =>
        {
            Debug.Log("Video preparation completed!!!!!");
            videoWidth = videoPlayer.width;
           
            bike = GameObject.FindGameObjectWithTag("Bike");

            // get now frames
            videoController = GameObject.FindObjectOfType<VideoController>();
            Debug.Log("frame index:" + videoController.nowframe);


            //////////////////////////////////////////
            // random object x
            float x = Random.Range(0.0f, 1.0f);

            initialPosition = new Vector3(normPlayer(x), -100, 15); // -100. 15

            // instantiate object
            obstacleInstance = Instantiate(prefab, initialPosition, Quaternion.identity);

            // get initial value
            deltaY = System.Math.Abs(obstacleInstance.transform.position.y - bike.transform.position.y);
            deltaZ = System.Math.Abs(obstacleInstance.transform.position.z - bike.transform.position.z);
            proportion = deltaZ/deltaY;

        };

    }

    void Update()
    {
        // Debug.Log("Obstacle update");

        if(videoPlayer.isPrepared)
        {
            Debug.Log("Obstacle update");

            // now frames
            frameIndex = videoController.nowframe;
            // Debug.Log("frame index:" + videoController.nowframe);

            ////////////////////////////////////
            verticalInput = Input.GetAxis("Vertical");
            // Debug.Log("v input:" + verticalInput);
            
            // increase y value
            yspeed_total = initSpeed+verticalInput*yspeed;

            // increase z value
            zspeed = yspeed_total*proportion;

            obstacleInstance.transform.position += new Vector3(0f, -yspeed_total, zspeed);
            Debug.Log("y:" + obstacleInstance.transform.position.y);
            Debug.Log("z:" + obstacleInstance.transform.position.z);
            // Debug.Log("z speed:" + zspeed);

            // zoom in
            obstacleInstance.transform.localScale += new Vector3(initScale+verticalInput*scale, initScale+verticalInput*scale, initScale+verticalInput*scale);
            // Debug.Log("scale:" + obstacleInstance.transform.localScale.x);
            
        }
            
        
    }

    float normCanvas(float x)
    {
        // normalize canvas x to [0, 1]
        float norm_x = x/videoWidth; // catch video width
        // float y = 400;
        // float norm_y = y/1440;
        // Debug.Log("x" + norm_x);
        // Debug.Log("y" + norm_y);

        return norm_x;
    }

    int normPlayer(float x)
    {
        // corresponding player world coordinate x
        int world_x = (int) System.Math.Floor(260*x) - 130;
        return world_x;
        // Debug.Log("y: " + y);
    }

    void loadData()
    {
        data = GameObject.FindObjectOfType<LoadJsonData>();
        Debug.Log(data.dataWrapper);
        foreach (FrameData frameData in (data.dataWrapper).FrameData)
        {
            Debug.Log("Frame: " + frameData.frame);
            Debug.Log("Direction: " + frameData.direction);
            Debug.Log("Angle: " + frameData.angle);
            Debug.Log("Left line range: " + frameData.left_line_range[0]);
            Debug.Log("Right line range: " + string.Join(", ", frameData.right_line_range));
            Debug.Log("Objects Appear Frame count: " + string.Join(", ", frameData.objects_appear_frame_count));
            Debug.Log("Objects Final PositionX: " + string.Join(", ", frameData.objects_final_positionX));
        }
    }

    void createObstacle()
    {

        // random object x
        float x = Random.Range(0.0f, 1.0f);

        initialPosition = new Vector3(normPlayer(x), -100, 15); // -100. 15

        // instantiate object
        obstacleInstance = Instantiate(prefab, initialPosition, Quaternion.identity);

        // get initial value
        deltaY = System.Math.Abs(obstacleInstance.transform.position.y - bike.transform.position.y);
        deltaZ = System.Math.Abs(obstacleInstance.transform.position.z - bike.transform.position.z);
        proportion = deltaZ/deltaY;

        ////////////////////////////////////////////////

        verticalInput = Input.GetAxis("Vertical");
        // Debug.Log("v input:" + verticalInput);
        
        // increase y value
        yspeed_total = initSpeed+verticalInput*yspeed;

        // increase z value
        zspeed = yspeed_total*proportion;

        obstacleInstance.transform.position += new Vector3(0f, -yspeed_total, zspeed);
        Debug.Log("y:" + obstacleInstance.transform.position.y);
        Debug.Log("z:" + obstacleInstance.transform.position.z);
        // Debug.Log("z speed:" + zspeed);

        // zoom in
        obstacleInstance.transform.localScale += new Vector3(initScale+verticalInput*scale, initScale+verticalInput*scale, initScale+verticalInput*scale);
        // Debug.Log("scale:" + obstacleInstance.transform.localScale.x);

    }

}