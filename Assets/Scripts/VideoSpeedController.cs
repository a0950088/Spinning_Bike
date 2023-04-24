using System.Collections;
using System.Collections.Generic;
using UnityEngine.Video;
using UnityEngine;
using UnityEngine.UI;

public class VideoSpeedController : MonoBehaviour
{
    // public GameObject canvas;
    private VideoPlayer videoPy;
    private float verticalInput;
    private ShowVideo image;
    public long nowframe;
    
    // Start is called before the first frame update
    void Start()
    {
        image = GameObject.Find("VideoFrame").GetComponent<ShowVideo>();
        videoPy = GetComponent<VideoPlayer>();
        videoPy.playOnAwake = false;
        videoPy.renderMode = VideoRenderMode.APIOnly;
        videoPy.Prepare();
        videoPy.Play();
        
        var imagesize = image.GetComponent<RectTransform>();
        var imageload = image.GetComponent<RawImage>();
        imageload.texture = videoPy.texture;
        // imagesize.anchoredPosition3D = new Vector3(0,0,0);
        imagesize.sizeDelta = new Vector2(videoPy.width,videoPy.height);
        
        Debug.Log(videoPy.width);
        Debug.Log(videoPy.height);
        Debug.Log(videoPy.frameCount);
    }

    // Update is called once per frame
    void Update()
    {
        var imageload = image.GetComponent<RawImage>();
        imageload.texture = videoPy.texture;
        videoPy.Play();
        verticalInput = Input.GetAxis("Vertical");
        float nowPlayBackSpeed = videoPy.playbackSpeed+(verticalInput*0.01f);
        if (nowPlayBackSpeed > 2){
            videoPy.playbackSpeed = 2;
        }
        else if(nowPlayBackSpeed < 0){
            videoPy.playbackSpeed = 0;
        }
        else{
            videoPy.playbackSpeed = nowPlayBackSpeed;
        }
        // Debug.Log(videoPy.playbackSpeed);
        // Debug.Log(videoPy.canSetPlaybackSpeed);
        // Debug.Log(videoPy.canSetSkipOnDrop);
        nowframe = videoPy.frame; 
        // Debug.Log("Frame:"+nowframe);
        // Debug.Log(verticalInput);
    }
}
