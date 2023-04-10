using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowPlayer : MonoBehaviour
{
    // Start is called before the first frame update
    public GameObject biker;
    public Vector3 offset = new Vector3(22,0,0);
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        transform.position = biker.transform.position + offset;
    }
}
