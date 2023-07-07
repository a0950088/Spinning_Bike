using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Video;
using UnityEngine.SceneManagement;
using TMPro;
using System;
using System.Text.RegularExpressions;

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
    private TMP_Text scores;
    private float time_start;
    private float time_end;
    private int time_total;
    TCP_Client tcpClient;


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
        //影片 EndReached
        time_start=Time.time;
        videoPy.loopPointReached+=EndReached;
        tcpClient = TCP_Client.Instance;
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
        if(tcpClient.conn_state == 1){
            if(videoPy.isPrepared)
            {
                videoPy.Play();
                SetImageTexture();
                // VideoSpeedControl();
                nowframe = videoPy.frame;
                // Debug.Log("Frame control:" + nowframe);
            }
        }
        else{
            videoPy.Pause();
            Debug.Log("Pause");
        }

    }
    void EndReached(UnityEngine.Video.VideoPlayer vp){
        //影片放完切畫面
        //Debug.Log("Frame control:" + nowframe);
        //videoPy.Pause();
        time_end=Time.time;
        time_start=time_end-time_start;
        time_total=Mathf.FloorToInt(time_start);
        LoadScene("Score_Scene");
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
        //RectTransform cadence = GameObject.Find("Cadence").GetComponent<RectTransform>();
        //RectTransform angle = GameObject.Find("Angle").GetComponent<RectTransform>();
        RectTransform score = GameObject.Find("Score").GetComponent<RectTransform>();
        RectTransform score_minus = GameObject.Find("score_minus").GetComponent<RectTransform>();
        RectTransform left = GameObject.Find("Left").GetComponent<RectTransform>();
        RectTransform right = GameObject.Find("Right").GetComponent<RectTransform>();

        // 重新計算 Scale、位置
        Vector3 originalScale = speed.localScale;
        Vector3 newScale = originalScale * scalingRatio;

        Vector3 newPositionSpeed = speed.localPosition * scalingRatio;
        //Vector3 newPositionCadence = cadence.localPosition * scalingRatio;
        //Vector3 newPositionAngle = angle.localPosition * scalingRatio;
        Vector3 newPositionScore = score.localPosition * scalingRatio;
        Vector3 newPositionScoreMinus = score_minus.localPosition * scalingRatio;
        Vector3 newPositionLeft = left.localPosition * scalingRatio;
        Vector3 newPositionRight = right.localPosition * scalingRatio;

        // 調整 UI
        speed.localScale = newScale;
        speed.localPosition = newPositionSpeed;

        //cadence.localScale = newScale;
        //cadence.localPosition = newPositionCadence;

        //angle.localScale = newScale;
        //angle.localPosition = newPositionAngle;

        score.localScale = newScale;
        score.localPosition = newPositionScore;

        score_minus.localScale = newScale;
        score_minus.localPosition = newPositionScoreMinus;

        left.localScale = newScale;
        left.localPosition = newPositionLeft;

        right.localScale = newScale;
        right.localPosition = newPositionRight;


        //GameObject leftObject = GameObject.Find("Left");
        //leftObject.SetActive(false);

        GameObject rightObject = GameObject.Find("Right");
        rightObject.SetActive(false);


    }
    public void LoadScene(string sceneName)
    {
        //讀取分數+時間
        scores = GameObject.Find("Score_text").GetComponent<TMP_Text>();
        string Final_point=scores.text;
        string pattern = @"\d+";
        Match match = Regex.Match(Final_point, pattern);
        if (match.Success){
            Final_point = match.Value;
        }
        PlayerPrefs.SetString("Final_point_str", Final_point);
        PlayerPrefs.SetString("Time_total",time_total.ToString());
        //string justtest = PlayerPrefs.GetString("Final_point_str");
        //Debug.Log("Final Score: " + justtest);
        SceneManager.LoadScene(sceneName);
    }
    
}
