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
    
    /* human */

    public float camToPlayerDist = 100f;
    public float playerPosy = -50f;
    public float firstrotate;
    private float verticalInput;
    private float TurnAngle = 30.0f;
    public float turnSpeed = 60.0f;
    public float WheelSpeed = 40.0f;
    private float acceleration = 10.0f;
    private float MaxSpeed = 100.0f;
    private float MinSpeed = 0.0f;
    public float ridingSpeed = 30.0f;

    // socket version
    public bool testflag = true;
    private float MAX_RIGHT_ANGLE = 45.0f;
    private float MAX_LEFT_ANGLE = -45.0f;


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

        camPos = GameObject.Find("Main Camera").GetComponent<Transform>();
        playerPos = GetComponent<Transform>();

        firstrotate = UnityEditor.TransformUtils.GetInspectorRotation(BikeHandleBar.transform).y;

        videoPlayer.prepareCompleted += (source) =>
        {
            Debug.Log("Video preparation completed!");
            playerPos.position = new Vector3(0, playerPosy, camPos.position.z- camToPlayerDist);
        };
    }
    
    void Update()
    {
        DecideDirection();
        BikeSpeed();
        WheelRoll();
        PanelRoll();
    }

    public void DecideDirection() {
        var y = UnityEditor.TransformUtils.GetInspectorRotation(BikeHandleBar.transform).y;
        if (Input.GetKey("left")) {
            if (y > firstrotate - TurnAngle) {
                ChangeBikeDirection(-turnSpeed);
            }
            RideBike(ridingSpeed);
        }
        if (Input.GetKey("right")) {
            if (y < firstrotate + TurnAngle) {
                ChangeBikeDirection(turnSpeed);
            }
            RideBike(-ridingSpeed);
        }

        // Debug.Log(testflag);
        // if(testflag){
        //     if (y > firstrotate - TurnAngle) {
        //         ChangeBikeDirection(-turnSpeed);
        //     }
        //     RideBike(ridingSpeed);
        //     testflag = false;
        // }
        // else{
        //     if (y < firstrotate + TurnAngle) {
        //         ChangeBikeDirection(turnSpeed);
        //     }
        //     RideBike(-ridingSpeed);
        //     testflag = true;
        // }
    }

    public void getSensorData(float speed, float cadence, float angle){
        // send sensor data to process player animation
        Debug.Log("rec speed: " + speed);// 0 ~ 20
        Debug.Log("rec cadence: " + cadence);
        Debug.Log("rec angle: " + angle); // -45 ~ 45
        setPlayerAnimation(speed, angle);
        setUiText(speed, cadence, angle);
    }

    void setPlayerAnimation(float speed, float angle){
        videoPlayer.playbackSpeed = (speed/10)+0.5f;
        ChangePlayerDirection(angle);
        if(angle>=0){
            RideBike(-speed);
        }
        else{
            RideBike(speed);
        }
    }

    void setUiText(float speed, float cadence, float angle){
        display_speed.text = System.Math.Floor(speed).ToString();
        display_cadence.text = System.Math.Floor(cadence).ToString();
        display_angle.text = System.Math.Floor(angle).ToString();
    }

    void ChangePlayerDirection(float angle){
        var bikeHandleBar_Y = UnityEditor.TransformUtils.GetInspectorRotation(BikeHandleBar.transform).y;
        if(bikeHandleBar_Y <= MAX_RIGHT_ANGLE && bikeHandleBar_Y >= MAX_LEFT_ANGLE){
            BikeHandleBar.transform.Rotate(0, angle * Time.deltaTime, 0);
            Bike.transform.Rotate(0, angle * Time.deltaTime, 0);
            FrontWheel.transform.Rotate(new Vector3(0, angle * Time.deltaTime, 0), Space.World);
        }
        else if(bikeHandleBar_Y > MAX_RIGHT_ANGLE && angle <= 0){
            BikeHandleBar.transform.Rotate(0, angle * Time.deltaTime, 0);
            Bike.transform.Rotate(0, angle * Time.deltaTime, 0);
            FrontWheel.transform.Rotate(new Vector3(0, angle * Time.deltaTime, 0), Space.World);
        }
        else if(bikeHandleBar_Y < MAX_LEFT_ANGLE && angle >= 0){
            BikeHandleBar.transform.Rotate(0, angle * Time.deltaTime, 0);
            Bike.transform.Rotate(0, angle * Time.deltaTime, 0);
            FrontWheel.transform.Rotate(new Vector3(0, angle * Time.deltaTime, 0), Space.World);
        }
    }

    void ChangeBikeDirection(float frequency) {
        BikeHandleBar.transform.Rotate(0, frequency * Time.deltaTime, 0);
        Bike.transform.Rotate(0, frequency * Time.deltaTime, 0);
        FrontWheel.transform.Rotate(new Vector3(0, frequency * Time.deltaTime, 0), Space.World);
    }

    void RideBike(float speed) {
        // Bike.transform.Translate(speed * Time.deltaTime, 0, 0);
        Debug.Log("change position");
        Bike.transform.position += (new Vector3(speed * Time.deltaTime, 0f, 0f));

    }

    public void WheelRoll() {
        FrontWheel.transform.Rotate(new Vector3(WheelSpeed, 0, 0) * Time.deltaTime);
        BackWheel.transform.Rotate(new Vector3(WheelSpeed, 0, 0) * Time.deltaTime);
    }

    public void PanelRoll() {
        foreach (GameObject panel in panels) {
            panel.transform.Rotate(new Vector3(WheelSpeed, 0, 0) * Time.deltaTime);
        }
    }

    public void BikeSpeed() {
        verticalInput = Input.GetAxis("Vertical");
        if (WheelSpeed > MaxSpeed) {
            WheelSpeed = MaxSpeed;
        }
        else if (WheelSpeed < MinSpeed) {
            WheelSpeed = MinSpeed;
        }
        else {
            WheelSpeed += acceleration * verticalInput * 0.1f;
        }
        display_speed.text = System.Math.Floor(WheelSpeed).ToString();
    }
}
