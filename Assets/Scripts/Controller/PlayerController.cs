using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;
using UnityEngine.Video;
using TMPro;

public class PlayerController : MonoBehaviour
{
    public GameObject Bike;
    public GameObject FrontWheel;
    public GameObject BackWheel;
    public GameObject BikeHandleBar;
    public GameObject[] panels;

    public float camToPlayerDist = 100f;
    public float playerPosy = -50f;

    // socket version
    private float MAX_LEFT_ROTATE_ANGLE = -45.0f;
    private float MAX_RIGHT_ROTATE_ANGLE = 45.0f;
    private float MAX_X = 130.0f;
    private float MIN_X = -130.0f;
    public float init_speed = 0.5f;
    private float sensor_speed = 0.0f;
    private float sensor_cadence = 0.0f;
    private float sensor_angle = 0.0f;
    // private float TIME_CONSTANT = 0.01f;

    private VideoPlayer videoPlayer;
    private Transform playerPos;
    private Transform camPos;

    private TMP_Text display_speed;
    private TMP_Text display_cadence;
    private TMP_Text display_angle;

    private void Awake()
    {
        Debug.Log("PlayerController Awake");
        videoPlayer = GameObject.Find("Screen").GetComponent<VideoPlayer>();
    }
    void Start()
    {
        Debug.Log("PlayerController Start");
        /* init bike */
        Bike = GameObject.FindGameObjectWithTag("Bike");
        FrontWheel = GameObject.FindGameObjectWithTag("frontwheel");
        BackWheel = GameObject.FindGameObjectWithTag("backwheel");
        BikeHandleBar = GameObject.FindGameObjectWithTag("bikeHandlebar");
        panels = GameObject.FindGameObjectsWithTag("panel");
        display_speed = GameObject.Find("speed_text").GetComponent<TMP_Text>();
        display_cadence = GameObject.Find("cadence_text").GetComponent<TMP_Text>();
        display_angle = GameObject.Find("angle_text").GetComponent<TMP_Text>();

        camPos = GameObject.Find("Main Camera").GetComponent<Transform>();
        playerPos = GetComponent<Transform>();

        videoPlayer.prepareCompleted += (source) =>
        {
            Debug.Log("Video preparation completed!");
            playerPos.position = new Vector3(0, playerPosy, camPos.position.z- camToPlayerDist);
        };
    }
    
    void FixedUpdate()
    {
        setPlayerAnimation(sensor_speed, sensor_angle);
        setUiText();
    }

    public void getSensorData(float speed, float cadence, float angle){
        // send sensor data to process player animation
        sensor_speed = speed;
        sensor_cadence = cadence;
        sensor_angle = angle;
    }

    void setPlayerAnimation(float speed, float angle){
        if (init_speed > 0) {  // while not stopped
            videoPlayer.playbackSpeed = (speed/10.0f) + init_speed;

            ChangePlayerDirection(angle);

            if (angle > 0) {    //left
                RideBike(speed);
            }
            else if (angle < 0) {   //right
                RideBike(-speed);
            }
        }
    }

    void setUiText() {
        if (init_speed == 0) {
            display_speed.text = System.Math.Floor(0.0).ToString();
        }
        else{
            display_speed.text = System.Math.Floor(sensor_speed).ToString();    
        }
        display_cadence.text = System.Math.Floor(sensor_cadence).ToString();
        display_angle.text = System.Math.Floor(sensor_angle).ToString();
    }

    void ChangePlayerDirection(float angle) {
        var bikeHandleBar_Y = UnityEditor.TransformUtils.GetInspectorRotation(BikeHandleBar.transform).y;
        
        // Rotate turn left: minus right: plus
        Debug.Log("bikeHandleBar_Y: " + bikeHandleBar_Y);

        // Position turn left: plus right: minus
        if (bikeHandleBar_Y <= MAX_RIGHT_ROTATE_ANGLE && bikeHandleBar_Y >= MAX_LEFT_ROTATE_ANGLE) {
            Debug.Log("in1: ");
            ChangeBikeDirection(-angle);
        }
        else if (bikeHandleBar_Y > MAX_RIGHT_ROTATE_ANGLE && (-angle) <= 0) {
            //右轉到底
            Debug.Log("in2: ");
            ChangeBikeDirection(-angle);
        }
        else if (bikeHandleBar_Y < MAX_LEFT_ROTATE_ANGLE && (-angle) >= 0) {
            //左轉到底
            Debug.Log("in3: ");
            ChangeBikeDirection(-angle);
        }
        
        // Vector3 bikeHandleBar_vec = BikeHandleBar.transform.rotation.eulerAngles;
        // Vector3 bike_vec = Bike.transform.rotation.eulerAngles;
        // Vector3 frontWheel_vec = FrontWheel.transform.rotation.eulerAngles;
        // bikeHandleBar_vec.y = -angle;
        // bike_vec.y = -angle;
        // frontWheel_vec.y = -angle;
        // Debug.Log("bikeHandleBar_vec: " + bikeHandleBar_vec.y);
        // Debug.Log("bike_vec: " + bike_vec.y);

        // BikeHandleBar.transform.rotation = Quaternion.Euler(bikeHandleBar_vec);
        // Bike.transform.rotation = Quaternion.Euler(bike_vec);
        // FrontWheel.transform.rotation = Quaternion.Euler(frontWheel_vec);
    }

    void RideBike(float speed) {
        if (Bike.transform.position.x <= MAX_X && Bike.transform.position.x >= MIN_X) {
            Bike.transform.position += (new Vector3(speed *Time.deltaTime, 0f, 0f));
        }
        else if (Bike.transform.position.x > MAX_X && speed <= 0) {
            Bike.transform.position += (new Vector3(speed * Time.deltaTime, 0f, 0f));

        }
        else if (Bike.transform.position.x < MIN_X && speed >= 0) {
            Bike.transform.position += (new Vector3(speed *Time.deltaTime, 0f, 0f));
        }

        // Bike.transform.Translate(speed * Time.deltaTime, 0, 0);
    }

    void ChangeBikeDirection(float frequency) {
        BikeHandleBar.transform.Rotate(0, frequency * Time.deltaTime, 0);
        Bike.transform.Rotate(0, frequency * Time.deltaTime, 0);
        FrontWheel.transform.Rotate(new Vector3(0, frequency * Time.deltaTime, 0), Space.World);
    }

    // public void DecideDirection() {
    //     var y = UnityEditor.TransformUtils.GetInspectorRotation(BikeHandleBar.transform).y;
    //     if (Input.GetKey("left")) {
    //         if (y > firstrotate - TurnAngle) {
    //             ChangeBikeDirection(-turnSpeed);
    //         }
    //         RideBike(ridingSpeed);
    //     }
    //     if (Input.GetKey("right")) {
    //         if (y < firstrotate + TurnAngle) {
    //             ChangeBikeDirection(turnSpeed);
    //         }
    //         RideBike(-ridingSpeed);
    //     }
    // }

    // public void WheelRoll() {
    //     FrontWheel.transform.Rotate(new Vector3(WheelSpeed, 0, 0) * Time.deltaTime);
    //     BackWheel.transform.Rotate(new Vector3(WheelSpeed, 0, 0) * Time.deltaTime);
    // }

    // public void PanelRoll() {
    //     foreach (GameObject panel in panels) {
    //         panel.transform.Rotate(new Vector3(WheelSpeed, 0, 0) * Time.deltaTime);
    //     }
    // }

    // public void BikeSpeed() {
    //     verticalInput = Input.GetAxis("Vertical");
    //     if (WheelSpeed > MaxSpeed) {
    //         WheelSpeed = MaxSpeed;
    //     }
    //     else if (WheelSpeed < MinSpeed) {
    //         WheelSpeed = MinSpeed;
    //     }
    //     else {
    //         WheelSpeed += acceleration * verticalInput * 0.1f;
    //     }
    //     display_speed.text = System.Math.Floor(WheelSpeed).ToString();
    // }
}
