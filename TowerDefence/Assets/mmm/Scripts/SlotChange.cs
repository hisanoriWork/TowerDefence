using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SlotChange : MonoBehaviour
{
    public int ownFormationNum = 1;
    public GameObject[] slotBtns;

    void Start()
    {
        ownFormationNum = int.Parse(PlayerPrefs.GetString("ownFormationNum", "1"));
        if (ownFormationNum - 1 <= slotBtns.Length)
        {
            //TODO: 対応するボタンの有効化
            slotBtns[ownFormationNum - 1].SetActive(false); //test
        }
    }

    public void ChangeSlot(GameObject test)
    {
        for (int i = 0; i < slotBtns.Length; i++)
        {
            //TODO: 対応するボタンの有効化
            slotBtns[i].SetActive(true);
            if (slotBtns[i] == test)
            {
                PlayerPrefs.SetString("ownFormationNum", (i + 1).ToString());
                test.SetActive(false);
            }
        }
    }
}
