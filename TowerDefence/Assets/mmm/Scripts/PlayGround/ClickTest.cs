using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ClickTest : MonoBehaviour
{
    public GameObject obj;
    public void OnClickListener()
    {
        if(obj != null)
        {
            Debug.Log(obj.name + "がクリックされました");
        }
        else
        {
            Debug.Log("objが見つかりません");
        }
    }
}
