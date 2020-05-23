using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SlotChange : MonoBehaviour
{
    public int ownFormationNum = 1;
    public GameObject[] slotBtns;

    public void A(GameObject test)
    {
        Debug.Log("aaaa");
        for (int i = 0; i < slotBtns.Length; i++)
        {
            slotBtns[i].SetActive(true);
            if (slotBtns[i] == test)
            {
                //TODO:
                test.SetActive(false);
            }
        }
    }
}
