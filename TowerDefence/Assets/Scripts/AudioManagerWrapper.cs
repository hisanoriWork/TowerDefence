using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class AudioManagerWrapper : MonoBehaviour
{
    public Slider SESlider;
    public Slider BGMSlider;

    void Start()
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

    public void PlaySE(string clipname)
    {
        SEManager.instance.Play(clipname);
    }

    public void PlayBGM(string clipname)
    {
        BGMManager.instance.Play(clipname);
    }

    public void ReplayBGM(string clipname)
    {
        BGMManager.instance.Replay(clipname);
    }
}
