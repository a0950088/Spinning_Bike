using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class ScrollbarControl : MonoBehaviour
{
    private Scrollbar scrollbar;
    void Start()
    {
        scrollbar = GetComponent<Scrollbar>();
    }
    public void ListScroll(RectTransform list)
    {
        list.localPosition = new Vector3(list.localPosition.x, 190+scrollbar.value * 380.0f, list.localPosition.z);////(1)
    }
}
