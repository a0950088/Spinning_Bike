using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using UnityEngine.Playables;

public class LoadJsonData : MonoBehaviour
{
    // Start is called before the first frame update
    string LoadData;
    public FrameDataWrapper dataWrapper;
    //LevelData[] MyData;
    void Awake(){
        Debug.Log("Load data Awake");
        LoadData = File.ReadAllText(Application.dataPath + "/JsonData/test.json");
        dataWrapper = JsonUtility.FromJson<FrameDataWrapper>(LoadData);
    }
    void Start()
    {
        Debug.Log("Load data Start");
        // Debug.Log(LoadData);
        // foreach (FrameData frameData in dataWrapper.FrameData)
        // {
        //     Debug.Log("Frame: " + frameData.frame);
        //     Debug.Log("Direction: " + frameData.direction);
        //     Debug.Log("Angle: " + frameData.angle);
        //     Debug.Log("Left line range: " + frameData.left_line_range[0]);
        //     Debug.Log("Right line range: " + string.Join(", ", frameData.right_line_range));
        //     Debug.Log("Top point: " + string.Join(", ", frameData.top_point));
        //     Debug.Log("Objects Appear Frame count: " + string.Join(", ", frameData.objects_appear_frame_count));
        //     Debug.Log("Objects Final PositionX: " + string.Join(", ", frameData.objects_final_positionX));
        // }
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
