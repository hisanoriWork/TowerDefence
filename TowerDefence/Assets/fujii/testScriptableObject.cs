using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class testScriptableObject : MonoBehaviour
{
    public StageData stageData;

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Alpha5))
        {
            stageData = ScriptableObject.CreateInstance<StageData>();
        }

        if (Input.GetKeyDown(KeyCode.Alpha6))
        {
            Debug.Log(stageData.detailContent);
        }
    }
}
