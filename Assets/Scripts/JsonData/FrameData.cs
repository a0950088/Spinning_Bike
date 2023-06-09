using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Add json key from client data
[System.Serializable]
public class FrameData
{
    public int frame;
    public string direction;
    public float angle;
    public float[] left_line_range;
    public float[] right_line_range;
    public float[] top_point;
    public float[] objects_position;
}
[System.Serializable]
public class FrameDataWrapper
{
    public FrameData[] FrameData;
}