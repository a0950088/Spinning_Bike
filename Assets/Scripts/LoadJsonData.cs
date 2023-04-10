using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

// Add json key from client data
[System.Serializable]
public class LevelData{
    // public byte[] image;
    // public string direction;
    // public float angle;
    // public float[] left_line_range;
    // public float[] right_line_range;
    public List<int> objects;
}

public class LoadJsonData : MonoBehaviour
{
    // Start is called before the first frame update
    string LoadData;
    LevelData MyData;
    LevelData levelData = new LevelData();
    void Start()
    {
        LoadData = File.ReadAllText(Application.dataPath + "/JsonData/test.json");
        Debug.Log(LoadData);
        MyData = JsonUtility.FromJson<LevelData>(LoadData);
        // Debug.Log(MyData.direction);
        // Debug.Log(MyData.angle);
        // Debug.Log(MyData.left_line_range[0]);
        // Debug.Log(MyData.right_line_range[0]);
        Debug.Log(MyData.objects[0]);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
