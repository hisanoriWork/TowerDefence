using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;
using UniRx;
public class TimeView : MonoBehaviour
{
    /*****public field*****/
    public Timer timer;
    public Text timeText;
    public bool isFinished {get;set;} = false;
    /*****monoBehaviour*****/
    void Awake()
    {
        timer.onDigitalTimeChanged.Subscribe(time =>
        {
            timeText.text = time.ToString();
        });
        timer.whenTimeIsUp.Subscribe(_=>
        {
            this.isFinished = true;
        });
        timer.Stop();
    }
    void Start()
    {
        timer.Play();
    }
}
