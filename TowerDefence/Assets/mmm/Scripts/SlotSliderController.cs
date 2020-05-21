using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SlotSliderController : MonoBehaviour
{
    public int ownFormationNum = 1;
    public Slider slotSlider;
    public Text slotText;


    void Start()
    {
        ownFormationNum = int.Parse(PlayerPrefs.GetString("ownFormationNum", "1"));
        slotSlider.value = ownFormationNum;
    }

    public void OnValueChanged()
    {
        ownFormationNum = (int)slotSlider.value;
        PlayerPrefs.SetString("ownFormationNum", ownFormationNum.ToString());
        slotText.text = "編成 " + ownFormationNum;
    }
}
