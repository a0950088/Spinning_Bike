using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class ScrollbarControl : MonoBehaviour
{

    private Scrollbar scrollbar;
    private float videoListRange = 244.0f;
    public RectTransform list;
    public int buttonCount = 0;
    private int RuleScrollArea_high = 600;
    private float originalY;


    void Start()
    {
        scrollbar = GetComponent<Scrollbar>();
        originalY = list.localPosition.y;
    }

    private void CountButtons()
    {
        buttonCount = list.childCount;
        Debug.Log("Buttons: " + buttonCount);
    }



    public void VideoListScroll()
    {
        CountButtons();
        videoListRange = 244.0f * ((buttonCount - 1) / 4);
        list.localPosition = new Vector3(list.localPosition.x, originalY + scrollbar.value * videoListRange, list.localPosition.z);
    }

    public void RuleScroll()
    {
        list.localPosition = new Vector3(list.localPosition.x, scrollbar.value * RuleScrollArea_high, list.localPosition.z);
    }


}
