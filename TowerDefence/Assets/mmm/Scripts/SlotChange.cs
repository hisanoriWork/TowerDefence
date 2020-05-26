using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SlotChange : MonoBehaviour
{
    public int ownFormationNum = 1;
    public GameObject[] slotBtns;
    public Sprite[] buttonSprites;
    public Sprite[] selectButtonSprites;

    void Start()
    {
        ownFormationNum = int.Parse(PlayerPrefs.GetString("ownFormationNum", "1"));
        if (ownFormationNum <= slotBtns.Length + 1)
        {
            for (int i = 0; i < slotBtns.Length; i++)
            {
                var m_Image = slotBtns[i].GetComponent<Image>();
                m_Image.sprite = buttonSprites[i];
                if (ownFormationNum == i + 1)
                {
                    m_Image.sprite = selectButtonSprites[i];
                }
            }
        }
    }

    public void ChangeSlot(GameObject button)
    {
        for (int i = 0; i < slotBtns.Length; i++)
        {
            var m_Image = slotBtns[i].GetComponent<Image>();
            m_Image.sprite = buttonSprites[i];
            if (slotBtns[i] == button)
            {
                m_Image.sprite = selectButtonSprites[i];
                PlayerPrefs.SetString("ownFormationNum", (i + 1).ToString());
            }
        }
    }
}
