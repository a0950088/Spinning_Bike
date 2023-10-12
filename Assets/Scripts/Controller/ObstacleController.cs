using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Video;

public class ObstacleController : MonoBehaviour
{
    public GameObject obstacleInstance, bike;
    public GameObject prefab_0, prefab_1;

    Vector3 initialPosition;
    private float initialPositionY = -100f;
    private float initialPositionZ = 15f;

    private float verticalInput;
    public float initSpeed = 0.05f;
    public float initScale = 0.01f;
    private float yspeed = 0.1f;
    private float zspeed;
    private float yspeed_total = 0.1f;
    private float scale = 0.01f;
    private float rand = 0.3f;

    private float deltaY;
    private float deltaZ;
    private float proportion;

    private VideoPlayer videoPlayer;
    private float videoWidth;
    private float videoHeight;

    private VideoController videoController;

    private LoadJsonData data;
    private long frameIndex;
    private string direction;

    TCP_Client tcpClient = TCP_Client.Instance;
    private int obstacle_shape_index = 1;

    // 1. if go staight, then possibly create object;
    // 2. when creating object, i need to initiate x: within lines, y: on the top point
    // 3. increase y value, and zoom in object size with speed and time

    //Material list

    //Mesh list


    void Start()
    {
        Debug.Log("Obstacle start");
        videoPlayer = GameObject.Find("Screen").GetComponent<VideoPlayer>();
        videoPlayer.prepareCompleted += (source) =>
        {
            Debug.Log("Video preparation completed!!!!!");

            videoWidth = videoPlayer.width;
            videoHeight = videoPlayer.height;
            bike = GameObject.FindGameObjectWithTag("Bike");
            videoController = GameObject.FindObjectOfType<VideoController>();
            data = GameObject.FindObjectOfType<LoadJsonData>();
        };

    }

    void FixedUpdate()
    {
        if (videoPlayer.isPrepared && videoController.nowframe > 0 && videoPlayer.frame > 0) // sync video frame
        {
            frameIndex = data.dataWrapper.FrameData[videoController.nowframe].frame;
            direction = data.dataWrapper.FrameData[videoController.nowframe].direction;

            float r = Random.Range(0.0f, 1.0f);

            if (direction == "Straight" && rand >= r && obstacleInstance == null)
            {
                createObstacle();
            }

            if (obstacleInstance != null)
            {
                updateObstacle();
            }

        }
    }

    int normPlayer(float x)
    {
        // corresponding player world coordinate x
        int world_x = (int)System.Math.Floor(260 * x) - 130;
        return world_x;
    }

    void createObstacle()
    {
        Vector2 pointNearL = new Vector2(data.dataWrapper.FrameData[frameIndex].left_line_range[0], data.dataWrapper.FrameData[frameIndex].left_line_range[1]);
        Vector2 pointFarL = new Vector2(data.dataWrapper.FrameData[frameIndex].left_line_range[2], data.dataWrapper.FrameData[frameIndex].left_line_range[3]);

        Vector2 pointNearR = new Vector2(data.dataWrapper.FrameData[frameIndex].right_line_range[0], data.dataWrapper.FrameData[frameIndex].right_line_range[1]);
        Vector2 pointFarR = new Vector2(data.dataWrapper.FrameData[frameIndex].right_line_range[2], data.dataWrapper.FrameData[frameIndex].right_line_range[3]);

        Vector2 pointTop = new Vector2(data.dataWrapper.FrameData[frameIndex].top_point[0], data.dataWrapper.FrameData[frameIndex].top_point[1]);
        Vector2 pointTopL;
        Vector2 pointTopR;

        float coefL = (pointTop.y - pointFarL.y) / (pointFarL.y - pointNearL.y);
        pointTopL = new Vector2((pointFarL.x + coefL * (pointFarL.x - pointNearL.x)), pointTop.y);

        float coefR = (pointTop.y - pointFarR.y) / (pointFarR.y - pointNearR.y);
        pointTopR = new Vector2((pointFarR.x + coefR * (pointFarR.x - pointNearR.x)), pointTop.y);

        // Debug.Log("frameI: " + frameIndex);
        // Debug.Log("pnr: " + pointNearR);
        // Debug.Log("pfr: " + pointFarR);
        // Debug.Log("pnl: " + pointNearL);
        // Debug.Log("pfl: " + pointFarL);
        // Debug.Log("top l: " + pointTopL);
        // Debug.Log("top r: " + pointTopR);
        // Debug.Log("top r map: " + pointTopR.x/videoWidth);
        // Debug.Log("top l map: " + pointTopL.x/videoWidth);


        float x = Random.Range(pointTopL.x / videoWidth, pointTopR.x / videoWidth);

        initialPosition = new Vector3(normPlayer(x), initialPositionY, initialPositionZ);

        // instantiate object

        if (obstacle_shape_index == 0)
        {
            obstacleInstance = Instantiate(prefab_0, initialPosition, Quaternion.identity);
            obstacle_shape_index = 1;
        }
        else if (obstacle_shape_index == 1)
        {
            obstacleInstance = Instantiate(prefab_1, initialPosition, Quaternion.identity);
            obstacle_shape_index = 0;
        }



    }
    void updateObstacle()
    {
        // get initial value
        deltaY = System.Math.Abs(obstacleInstance.transform.position.y - bike.transform.position.y);
        deltaZ = System.Math.Abs(obstacleInstance.transform.position.z - bike.transform.position.z);
        proportion = deltaZ / deltaY;

        verticalInput = Input.GetAxis("Vertical");

        if (tcpClient.conn_state == 1)
        {
            // increase y value
            yspeed_total = initSpeed + verticalInput * yspeed;

            // increase z value
            zspeed = yspeed_total * proportion;

            obstacleInstance.transform.position += new Vector3(0f, yspeed_total, zspeed);

            // zoom in
            obstacleInstance.transform.localScale += new Vector3(initScale + verticalInput * scale, initScale + verticalInput * scale, initScale + verticalInput * scale);
        }


        // Destroy
        if (obstacleInstance.transform.position.z > 1000)
        {
            // Debug.Log("Obstacle out of sight.");
            Destroy(obstacleInstance);
        }
    }

}