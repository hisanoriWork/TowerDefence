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
    public TimeGauge timeGauge;
    public bool isFinished {get;set;} = false;
    /*****monoBehaviour*****/
    void Awake()
    {
        timeGauge.maxValue = (int)timer.time;
        timeGauge.value = 0;
        timer.onDigitalTimeChanged.Subscribe(time =>
        {
            timeGauge.value = (int)timer.time - timer.digitalTime;
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
