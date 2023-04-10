using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Video;

public class MainCameraController : MonoBehaviour
{
    private Camera cam;
    private Transform camPosition;
    private VideoPlayer videoPlayer;
    public float camConstant = 6.25f;
    private void Awake()
    {
        cam = GetComponent<Camera>();
        cam.fieldOfView = 71.5f;

        camPosition = GetComponent<Transform>();
        videoPlayer = GameObject.Find("Screen").GetComponent<VideoPlayer>();
    }
    // Start is called before the first frame update
    void Start()
    {
        videoPlayer.prepareCompleted += (source) =>
        {
            Debug.Log("Video preparation completed!");
            float magnification = videoPlayer.width / 16;
            camPosition.position = new Vector3(0,0, magnification* camConstant);
        };
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
