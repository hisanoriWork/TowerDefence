using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class AudioManagerWrapper : MonoBehaviour
{
    public Slider SESlider;
    public Slider BGMSlider;
    public void SetSEVolume()
    {
        if(SESlider)
            SEManager.instance.SetVolume(SESlider.value);
    }

    public void SetBGMVolume()
    {
        if(BGMSlider)
            SEManager.instance.SetVolume(SESlider.value);
    }
}
