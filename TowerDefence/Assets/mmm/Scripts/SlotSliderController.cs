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
        // TODO: ownFormationNumの値を繁栄させる
        slotSlider.value = ownFormationNum;
    }

    public void OnValueChanged()
    {
        ownFormationNum = (int)slotSlider.value;
        slotText.text = "編成 " + ownFormationNum;
    }
}
