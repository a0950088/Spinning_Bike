using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class ScrollbarControl : MonoBehaviour
{

    private Scrollbar scrollbar;
    private float range = 244.0f;
    public RectTransform list;
    public int buttonCount = 1;


    void Start()
    {
        scrollbar = GetComponent<Scrollbar>();
    }

    private void CountButtons()
    {
        buttonCount = list.childCount;
        Debug.Log("Buttons: " + buttonCount);
    }



    public void ListScroll()
    {
        CountButtons();
        range = 244.0f * (buttonCount / 4);
        Debug.Log("range: " + range);
        list.localPosition = new Vector3(list.localPosition.x, 190 + scrollbar.value * range, list.localPosition.z);
        Debug.Log("list.localPosition.y" + list.localPosition.y);
    }

}
