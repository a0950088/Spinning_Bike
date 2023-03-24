using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public GameObject Bike;
    public GameObject FrontWheel;
    public GameObject BackWheel;
    public GameObject BikeHandleBar;
    public GameObject[] panels;
    
    /* human */

    public float firstrotate;
    private float verticalInput;
    private float TurnAngle = 30.0f;
    private float turnSpeed = 30.0f;
    private float WheelSpeed = 40.0f;
    private float acceleration = 10.0f;
    private float MaxSpeed = 100.0f;
    private float MinSpeed = 0.0f;
    private float ridingSpeed = 10.0f;

    void Start()
    {
        /* init bike */
        Bike = GameObject.FindGameObjectWithTag("Bike");
        FrontWheel = GameObject.FindGameObjectWithTag("frontwheel");
        BackWheel = GameObject.FindGameObjectWithTag("backwheel");
        BikeHandleBar = GameObject.FindGameObjectWithTag("bikeHandlebar");
        panels = GameObject.FindGameObjectsWithTag("panel");

        firstrotate = UnityEditor.TransformUtils.GetInspectorRotation(BikeHandleBar.transform).y;
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
            //RideBike(ridingSpeed);
        }
        if (Input.GetKey("right")) {
            if (y < firstrotate + TurnAngle) {
                ChangeBikeDirection(turnSpeed);
            }
            //RideBike(-ridingSpeed);
        }
    }

    void ChangeBikeDirection(float frequency) {
        BikeHandleBar.transform.Rotate(0, frequency * Time.deltaTime, 0);
        Bike.transform.Rotate(0, frequency * Time.deltaTime, 0);
        //Bike.transform.Translate(-frequency * Time.deltaTime, 0, 0);
        FrontWheel.transform.Rotate(new Vector3(0, frequency * Time.deltaTime, 0), Space.World);
    }

    void RideBike(float speed) {
        Bike.transform.Translate(speed * Time.deltaTime, 0, 0);
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
    
}
