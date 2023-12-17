using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.IO;

public class CheckVideoStatus : MonoBehaviour
{
    public GameObject listPanel;
    private AddVideo addVideo;
    TCP_Client tcpClient;
    public bool created = false;
    private Button thumbnaliButton;

    void Start()
    {
        tcpClient = TCP_Client.Instance;
        Transform addVideoButton = listPanel.transform.Find("AddVideo");
        addVideo = addVideoButton.GetComponent<AddVideo>();
    }


    public void AddVideoButton()
    {
        string videoFolder = Path.Combine(Application.dataPath, "Video");
        string[] allVideoPath = Directory.GetFiles(videoFolder);

        StartCoroutine(CreateThumbnailButton(allVideoPath));
    }


    IEnumerator CreateThumbnailButton(string[] allVideoPath)
    {
        /*
        foreach (string key in addVideo.buttonDictionary.Keys)
        {
            Debug.Log("Key: " + key);
        }
        */

        foreach (string videoPath in allVideoPath)
        {
            if (Path.GetExtension(videoPath) != ".mp4")
            {
                continue;
            }
            string videoName = Path.GetFileNameWithoutExtension(videoPath);


            if (!addVideo.buttonDictionary.ContainsKey(videoName))
            {
                created = false;
                addVideo.fileName = videoName;
                addVideo.videoURL = videoPath;

                // Debug.Log("videoName: " + videoName);
                //addVideo.thumbnailPath = Path.Combine(addVideo.thumbnailFolder, videoName + ".png");

                addVideo.GetThumbnailFromVideo(videoPath, addVideo.CreateNewButton);

                yield return new WaitUntil(() => created);
            }

        }

        CheckAllVideoStatus();
    }



    public void CheckAllVideoStatus()
    {
        // 搜尋 List Panel 下的所有按鈕
        Button[] buttons = listPanel.GetComponentsInChildren<Button>();
        // 檢查按鈕是否已存在於 List 中
        foreach (Button button in buttons)
        {
            ChangeStatus(button);
            ThumbnailStatus(button);
        }

    }


    public void ChangeStatus(Button videoButton)
    {
        string buttonName = videoButton.gameObject.name; // 取得按鈕的名字
        if (buttonName != "AddVideo")
        {
            if (!JsonExists(buttonName))
            {
                videoButton.interactable = false;
                //改一下 TCP_Client 下面的 processPath
                string videoFileName = buttonName + ".mp4";
                string videoPath = Path.Combine(Application.dataPath, "Video", videoFileName);
                tcpClient.processPath = videoPath;
            }
            else
            {
                videoButton.interactable = true;
            }
        }
    }

    private bool JsonExists(string buttonName)
    {
        string jsonFileName = buttonName + ".json";
        string jsonPath = Path.Combine(Application.dataPath, "JsonData", jsonFileName);
        return File.Exists(jsonPath);
    }

    private void ThumbnailStatus(Button videoButton)
    {
        string buttonName = videoButton.gameObject.name;
        if (buttonName != "AddVideo")
        {
            if (ThumbnailError(videoButton))
            {
                string videoFolder = Path.Combine(Application.dataPath, "Video");
                string videoPath = Path.Combine(videoFolder, buttonName);
                thumbnaliButton = videoButton;
                addVideo.GetThumbnailFromVideo(videoPath, AddThumbnail);

            }

        }
    }

    private bool ThumbnailError(Button videoButton)
    {
        Image image = videoButton.GetComponent<Image>();
        Color[] pixels = image.sprite.texture.GetPixels();
        for (int i = 0; i < pixels.Length; i++)
        {
            if (pixels[i].a > 0)
            {
                return false;
            }
        }
        return true;
    }

    public void AddThumbnail(Sprite thumbnailSprite)
    {
        thumbnaliButton.GetComponent<Image>().sprite = thumbnailSprite;

    }


}
