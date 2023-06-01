using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Video;
using UnityEngine.EventSystems;
using System.IO;
using System.Collections;
using System.Collections.Generic;
using SFB;


public class OpenFilePanel : MonoBehaviour
{
    private struct ThumbnailRequest
    {
        public VideoClip ClipToProcess;
        public string UrlToProcess;
        public int FrameToCapture;
        public ThumnailCreated callback;
    }

    //public delegate void ThumnailCreated(Sprite thumbnail);
    public delegate void ThumnailCreated();

    public Button buttonPrefab;
    public int thumbnailWidth = 224;
    public int thumbnailHeight = 144;

    public GameObject listPanel;

    private string fileName;
    private string videoURL;
    private string videoFolder;
    private string thumbnailPath;
    private string thumbnailFolder;

    public void OpenFile()
    {
        string title = "Select a video file";
        string directory = "";
        string extension = "mp4";

        string path = StandaloneFileBrowser.OpenFilePanel(title, directory, extension, false)[0];


        fileName = Path.GetFileNameWithoutExtension(path);
        videoFolder = Path.Combine(Application.dataPath, "Video");
        videoURL = Path.Combine(videoFolder, fileName + ".mp4");
        thumbnailFolder = Path.Combine(Application.dataPath, "Thumbnail");
        thumbnailPath = Path.Combine(thumbnailFolder, fileName + ".png");

        if (!Directory.Exists(videoFolder))
        {
            Directory.CreateDirectory(videoFolder);
        }
        if (!Directory.Exists(thumbnailFolder))
        {
            Directory.CreateDirectory(thumbnailFolder);
        }

        Transform buttonTransform = listPanel.transform.Find("filename");
        if (buttonTransform == null)
        {

            if (!File.Exists(videoURL))
            {
                File.Copy(path, videoURL);

            }
        }

        GetThumbnailFromVideo(videoURL, CreateNewButton);
    }

    private static VideoPlayer videoPlayer
    {
        get
        {
            if (_videoPlayer == null)
            {
                GameObject ThumbnailProcessor = new GameObject("Thumbnail Processor");
                Object.DontDestroyOnLoad(ThumbnailProcessor);
                _videoPlayer = ThumbnailProcessor.AddComponent<VideoPlayer>();
                _videoPlayer.renderMode = VideoRenderMode.APIOnly;
                _videoPlayer.playOnAwake = false;
                _videoPlayer.audioOutputMode = VideoAudioOutputMode.None;
            }
            return _videoPlayer;
        }
    }
    private static VideoPlayer _videoPlayer;

    private static ThumnailCreated thumbNailCreatedCallback;

    private static bool processInProgress;
    private static Queue<ThumbnailRequest> thumbnailQueue = new Queue<ThumbnailRequest>();

    /// <summary>
    /// Internal Function used only to consume enqueued requests
    /// </summary>
    private void GetThumbnailFromVideo(ThumbnailRequest request)
    {
        if (request.ClipToProcess != null)
        {
            GetThumbnailFromVideo(request.ClipToProcess, request.callback, request.FrameToCapture);
        }
        else if (request.UrlToProcess != null)
        {
            GetThumbnailFromVideo(request.UrlToProcess, request.callback, request.FrameToCapture);
        }
    }

    /// <summary>
    /// Creates a thumbnail from the string video passed in. The thumbnail will be asynchronously passed to the callback function
    /// </summary>
    /// <param name="videoURL">The URL to load the video from</param>
    /// <param name="callback">Where to pass the created Sprite</param>
    public void GetThumbnailFromVideo(string videoURL, ThumnailCreated callback, int frameToCapture = 0)
    {
        if (string.IsNullOrEmpty(videoURL))
        {
            Debug.LogWarning("Null videoURL detected. Unable to generate thumbnail.");
            return;
        }

        if (processInProgress)
        {
            thumbnailQueue.Enqueue(new ThumbnailRequest()
            {
                UrlToProcess = videoURL,
                FrameToCapture = frameToCapture,
                callback = callback
            });
            return;
        }
        processInProgress = true;
        thumbNailCreatedCallback = callback;
        videoPlayer.source = VideoSource.Url;
        videoPlayer.url = videoURL;
        PrepareVideoForProcessing(frameToCapture);

        videoPlayer.frame = 0;
        videoPlayer.Play();
        videoPlayer.Pause();
    }

    /// <summary>
    /// Creates a thumbnail from the string video passed in. The thumbnail will be asynchronously passed to the callback function
    /// </summary>
    /// <param name="videoClip">The clip to take the screenshot from</param>
    /// <param name="callback">Where to pass the created Sprite</param>
    public void GetThumbnailFromVideo(VideoClip videoClip, ThumnailCreated callback, int frameToCapture = 0)
    {
        if (videoClip == null)
        {
            Debug.LogWarning("Null videoURL detected. Unable to generate thumbnail.");
            return;
        }

        if (processInProgress)
        {
            thumbnailQueue.Enqueue(new ThumbnailRequest()
            {
                ClipToProcess = videoClip,
                FrameToCapture = frameToCapture,
                callback = callback
            });
            return;
        }
        processInProgress = true;

        thumbNailCreatedCallback = callback;

        videoPlayer.source = VideoSource.VideoClip;
        videoPlayer.clip = videoClip;
        PrepareVideoForProcessing(frameToCapture);

    }

    private void PrepareVideoForProcessing(int frameToCapture)
    {
        videoPlayer.sendFrameReadyEvents = true;
        videoPlayer.frameReady += ThumbnailReady;
        videoPlayer.frame = frameToCapture;
        // TODO This is bugged in Unity, check in the future if we can remove play and still recieve the frame with just .frame and Pause()
        videoPlayer.Play();
        videoPlayer.Pause();
    }

    private void ThumbnailReady(VideoPlayer source, long frameIdx)
    {
        videoPlayer.sendFrameReadyEvents = false;
        videoPlayer.frameReady -= ThumbnailReady;
        Texture2D tex = new Texture2D(2, 2);
        //Texture2D tex = new Texture2D(thumbnailWidth, thumbnailHeight);
        RenderTexture renderTexture = source.texture as RenderTexture;

        if (tex.width != renderTexture.width || tex.height != renderTexture.height)
        {
            tex.Reinitialize(renderTexture.width, renderTexture.height);
        }

        RenderTexture currentActiveRT = RenderTexture.active;
        RenderTexture.active = renderTexture;
        tex.ReadPixels(new Rect(0, 0, renderTexture.width, renderTexture.height), 0, 0);
        tex.Apply();
        
        renderTexture.Release();
        RenderTexture.active = currentActiveRT;



        byte[] bytes = tex.EncodeToPNG();
        File.WriteAllBytes(thumbnailPath, bytes);


        /*
        Sprite thumbnailSprite = Sprite.Create(tex, new Rect(0f, 0f, tex.width, tex.height), new Vector2(0.5f, 0.5f), 100f);

        EyeTrackerManager.Instance.Sprites.Add(thumbnailSprite);
        thumbNailCreatedCallback?.Invoke(thumbnailSprite);
        */
        thumbNailCreatedCallback?.Invoke();

        processInProgress = false;
        if (thumbnailQueue.Count > 0)
        {
            GetThumbnailFromVideo(thumbnailQueue.Dequeue());
        }
    }



    public void CreateNewButton()
    {

        if (ButtonExists(fileName))
        {
            Debug.Log("Button already exists.");
            return;
        }

        // 將當前影片的縮圖路徑設為 thumbnailPath
        thumbnailPath = Path.Combine(thumbnailFolder, fileName + ".PNG");

        // 讀取縮圖資料
        byte[] bytes = File.ReadAllBytes(thumbnailPath);
        Texture2D thumbnailTexture = new Texture2D(2, 2);
        thumbnailTexture.LoadImage(bytes);
        //Debug.Log("Thumbnail bytes length: " + bytes.Length);



        // 建立 newButton 及設定大小
        Button newButton = Instantiate(buttonPrefab, transform.parent);
        newButton.name = fileName;
        newButton.GetComponent<RectTransform>().sizeDelta = buttonPrefab.GetComponent<RectTransform>().sizeDelta;
        newButton.gameObject.SetActive(true);

        // 將縮圖套用到按鈕上
        Sprite sprite = Sprite.Create(thumbnailTexture, new Rect(0, 0, thumbnailTexture.width, thumbnailTexture.height), Vector2.one * 0.5f);
        newButton.GetComponent<Image>().sprite = sprite;

        ChangeState(newButton);

        // 加 onClick 時觸發的函式
        //newButton.GetComponent<Button>().onClick.AddListener(() => CreateNewButton());
    }

    public void ChangeState(Button newButton)
    {
        if (!JsonExists(fileName))
        {
            newButton.interactable = false;
            //改一下 TCP_Client 下面的 processPath
            TCP_Client.processPath = videoURL;
        }
        else
        {
            newButton.interactable = true;
        }

    }


    private bool ButtonExists(string buttonName)
    {
        // 搜尋 List Panel 下的所有按鈕
        Button[] buttons = listPanel.GetComponentsInChildren<Button>();

        // 檢查按鈕是否已存在於列表中
        foreach (Button button in buttons)
        {
            if (button.name == buttonName)
            {
                return true;
            }
        }

        return false;
    }

    private bool JsonExists(string buttonName)
    {
        string jsonFileName = buttonName + ".json";
        string jsonPath = Path.Combine(Application.dataPath, "JsonData", jsonFileName);
        return File.Exists(jsonPath);
    }
}
