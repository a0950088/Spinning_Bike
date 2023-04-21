using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActiveManager : MonoBehaviour
{
    public GameObject Obj;


    public void SetVisible()
    {
        if (Obj != null)
        {
            Obj.SetActive(true);
        }
    }

    public void SetInvisible()
    {
        if (Obj != null)
        {
            Obj.SetActive(false);
        }
    }

}
