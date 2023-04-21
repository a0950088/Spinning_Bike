using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System.IO;
using System.Collections;
using System.Collections.Generic;
using SFB;

using UnityEngine.Video;

public class OpenFilePanel : MonoBehaviour
{
    public Button buttonPrefab;  // 需要在Unity編輯器中指定預製的按鈕
    private Texture2D thumbnailTexture;
    public int thumbnailWidth = 224;
    public int thumbnailHeight = 144;
    public VideoPlayer videoPlayer;

    public void OpenFile()
    {
        string title = "Select a video file";
        string directory = "";
        string extension = "mp4";
        string path = StandaloneFileBrowser.OpenFilePanel(title, directory, extension, false)[0];

        if (!string.IsNullOrEmpty(path))
        {
            string fileName = Path.GetFileNameWithoutExtension(path);
            string targetFolder = Path.Combine(Application.dataPath, "Video");

            // Create the target folder if it doesn't exist
            if (!Directory.Exists(targetFolder))
            {
                Directory.CreateDirectory(targetFolder);
            }

            string targetPath = Path.Combine(targetFolder, fileName + ".mp4");
            File.Copy(path, targetPath);

            /*
            // Prepare the VideoPlayer to extract thumbnail
            videoPlayer.url = targetPath;
            videoPlayer.Prepare();

            // Wait until VideoPlayer is ready
            while (!videoPlayer.isPrepared)
            {
                continue;
            }
            */


            // Extract thumbnail
            /*
            RenderTexture renderTexture = videoPlayer.texture as RenderTexture;
            RenderTexture previous = RenderTexture.active;
            RenderTexture.active = renderTexture;
            */
            thumbnailTexture = new Texture2D(thumbnailWidth, thumbnailHeight);
            thumbnailTexture.ReadPixels(new Rect(0, 0, thumbnailWidth, thumbnailHeight), 0, 0);
            thumbnailTexture.Apply();
            //RenderTexture.active = previous;
            

            CreateNewButton();
        }
    }


    public void CreateNewButton()
    {
        //Button buttonPrefab = EventSystem.current.currentSelectedGameObject.GetComponent<Button>();

        // 建立 newButton 及設定大小
        Button newButton = Instantiate(buttonPrefab, buttonPrefab.transform.parent);
        newButton.GetComponent<RectTransform>().sizeDelta = buttonPrefab.GetComponent<RectTransform>().sizeDelta;
        newButton.gameObject.SetActive(true);

        // 處理圖片，現在是失敗的，現在只會複製 test 的
        //newButton.image.sprite = Sprite.Create(thumbnailTexture, new Rect(0, 0, thumbnailTexture.width, thumbnailTexture.height), Vector2.one * 0.5f);
        //newButton.GetComponent<Image>().sprite = thumbnailSprite;

        // 加 onClick 時觸發的函式
        //newButton.GetComponent<Button>().onClick.AddListener(() => CreateNewButton());
    }

}