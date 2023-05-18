using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Video;

public class VideoController : MonoBehaviour
{
    private VideoPlayer videoPy;
    private float verticalInput;
    private RectTransform imagesize;
    private RawImage rawimage;
    public string videoFilePath = "Assets/output_Trim.mp4";
    //Assets/video_Trim.mp4
    //D:\\Banana\\coding\\Unity\\TEST.mp4
    public long nowframe;

    private void Awake()
    {
        Debug.Log("VideoController Awake");
        videoPy = GetComponent<VideoPlayer>();
        //videoPy.source = VideoSource.Url;
        videoPy.url = videoFilePath;
        videoPy.renderMode = VideoRenderMode.APIOnly;
        videoPy.playOnAwake = false;
        videoPy.waitForFirstFrame = true;
        videoPy.Prepare();

        imagesize = GameObject.Find("VideoFrame").GetComponent<RectTransform>();
        rawimage = GameObject.Find("VideoFrame").GetComponent<RawImage>();

        //videoPy.Play();
    }
    // Start is called before the first frame update
    void Start()
    {
        Debug.Log("VideoController Start");
        videoPy.prepareCompleted += (source) =>
        {
            //imagesize.sizeDelta = new Vector2(videoPy.width, videoPy.height);
            imagesize.sizeDelta = new Vector2(videoPy.width, videoPy.height);
            Debug.Log("Video dimensions: " + videoPy.width + "x" + videoPy.height);
            videoPy.Play();
        };
    }

    // Update is called once per frame
    void Update()
    {
        // Debug.Log("Video Update");
        if(videoPy.isPrepared)
        {
            SetImageTexture();
            // VideoSpeedControl();
            nowframe = videoPy.frame;
            // Debug.Log("Frame control:" + nowframe);
        }
        else
        {
            Debug.Log("Not yet");
        }
    }

    private void SetImageTexture()
    {
        //rawimage.texture = videoPy.texture;
        rawimage.texture = videoPy.texture;
    }
    private void VideoSpeedControl()
    {
        verticalInput = Input.GetAxis("Vertical");
        float nowPlayBackSpeed = videoPy.playbackSpeed + (verticalInput * 0.01f);
        if (nowPlayBackSpeed > 2)
        {
            videoPy.playbackSpeed = 2;
        }
        else if (nowPlayBackSpeed < 0)
        {
            videoPy.playbackSpeed = 0;
        }
        else
        {
            videoPy.playbackSpeed = nowPlayBackSpeed;
        }
    }
}
