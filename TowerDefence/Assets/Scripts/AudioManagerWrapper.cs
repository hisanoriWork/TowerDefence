using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class AudioManagerWrapper : MonoBehaviour
{
    public Slider SESlider;
    public Slider BGMSlider;

    void Awake()
    {
        SESlider.value = SEManager.instance.GetVolume();
        BGMSlider.value = BGMManager.instance.GetVolume();
    }


    public void SetSEVolume()
    {
        if(SESlider)
            SEManager.instance.SetVolume(SESlider.value);
    }

    public void SetBGMVolume()
    {
        if(BGMSlider)
            BGMManager.instance.SetVolume(BGMSlider.value);
    }
}
