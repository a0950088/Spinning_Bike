using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;
using UnityEngine.Video;

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
    public float turnSpeed = 30.0f;
    public float WheelSpeed = 40.0f;
    private float acceleration = 10.0f;
    private float MaxSpeed = 100.0f;
    private float MinSpeed = 0.0f;
    public float ridingSpeed = 30.0f;

    private VideoPlayer videoPlayer;
    private Transform playerPos;
    private Transform camPos;

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

    void DecideDirection() {
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
    }

    void ChangeBikeDirection(float frequency) {
        BikeHandleBar.transform.Rotate(0, frequency * Time.deltaTime, 0);
        Bike.transform.Rotate(0, frequency * Time.deltaTime, 0);
        FrontWheel.transform.Rotate(new Vector3(0, frequency * Time.deltaTime, 0), Space.World);
    }

    void RideBike(float speed) {
        // Bike.transform.Translate(speed * Time.deltaTime, 0, 0);
        Bike.transform.position += (new Vector3(speed * Time.deltaTime, 0f, 0f));

    }

    void WheelRoll() {
        FrontWheel.transform.Rotate(new Vector3(WheelSpeed, 0, 0) * Time.deltaTime);
        BackWheel.transform.Rotate(new Vector3(WheelSpeed, 0, 0) * Time.deltaTime);
    }

    void PanelRoll() {
        foreach (GameObject panel in panels) {
            panel.transform.Rotate(new Vector3(WheelSpeed, 0, 0) * Time.deltaTime);
        }
    }

    void BikeSpeed() {
        verticalInput = Input.GetAxis("Vertical");
        if (WheelSpeed > MaxSpeed) {
            WheelSpeed = MaxSpeed;
        }
        else if (WheelSpeed < MinSpeed) {
            WheelSpeed = MinSpeed;
        }
        else {
            WheelSpeed += acceleration * verticalInput * 0.01f;
        }
    }
    
    /*
    void StoprRiding(){

    }

    void ResumeRiding(){
    
    }
    */
}
