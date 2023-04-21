using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShowVideo : MonoBehaviour
{
    private RectTransform rec;
    // Start is called before the first frame update
    //assign video time to play 
    void Start()
    {
        rec = GetComponent<RectTransform>();
        rec.anchoredPosition3D = new Vector3(0,0,0);
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
