using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Video;

public class VideoController : MonoBehaviour
{
    private VideoPlayer videoPy;
    private float verticalInput;
    private RectTransform imagesize;
    private RawImage rawimage;
    //public string videoFilePath = "Assets/output_Trim.mp4";
    public string videoFilePath;
    //Assets/video_Trim.mp4
    //D:\\Banana\\coding\\Unity\\TEST.mp4
    public long nowframe;

    private void Awake()
    {
        Debug.Log("VideoController Awake");
        videoPy = GetComponent<VideoPlayer>();
        //videoPy.source = VideoSource.Url;
        videoFilePath = PlayerPrefs.GetString("VideoPath");
        videoPy.url = videoFilePath;
        videoPy.renderMode = VideoRenderMode.APIOnly;
        videoPy.playOnAwake = false;
        videoPy.waitForFirstFrame = true;
        videoPy.Prepare();
        Debug.Log("VideoPath:" + videoPy.url);
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
            ScaleUIWithVideo();
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
    
    private void ScaleUIWithVideo()
    {
        float originWidth = 2560f;
        float originHeight = 1440f;

        RectTransform videoSize = imagesize;

        float videoWidth = videoSize.rect.width;
        float videoHeight = videoSize.rect.height;
        Debug.Log("videoSize: ", videoSize);


        // 計算比例
        float scalingRatioWidth = videoWidth / originWidth;
        float scalingRatioHeight = videoHeight / originHeight;
        float scalingRatio = Mathf.Min(scalingRatioWidth, scalingRatioHeight);

        // 取得 UI 
        RectTransform speed = GameObject.Find("Speed").GetComponent<RectTransform>();
        RectTransform cadence = GameObject.Find("Cadence").GetComponent<RectTransform>();
        RectTransform angle = GameObject.Find("Angle").GetComponent<RectTransform>();
        RectTransform score = GameObject.Find("Score").GetComponent<RectTransform>();
        RectTransform score_minus = GameObject.Find("score_minus").GetComponent<RectTransform>();


        // 重新計算 Scale、位置
        Vector3 originalScale = speed.localScale;
        Vector3 newScale = originalScale * scalingRatio;

        Vector3 newPositionSpeed = speed.localPosition * scalingRatio;
        Vector3 newPositionCadence = cadence.localPosition * scalingRatio;
        Vector3 newPositionAngle = angle.localPosition * scalingRatio;
        Vector3 newPositionScore = score.localPosition * scalingRatio;

        Vector3 MinusScale = score_minus.localScale;
        Vector3 newMinusScale = MinusScale * scalingRatio;
        Vector3 newPositionScoreMinus = score_minus.localPosition * scalingRatio;

        // 調整 UI
        speed.localScale = newScale;
        speed.localPosition = newPositionSpeed;

        cadence.localScale = newScale;
        cadence.localPosition = newPositionCadence;

        angle.localScale = newScale;
        angle.localPosition = newPositionAngle;

        score.localScale = newScale;
        score.localPosition = newPositionScore;

        score_minus.localScale = newMinusScale;
        score_minus.localPosition = newPositionScoreMinus;

    }
    
}
