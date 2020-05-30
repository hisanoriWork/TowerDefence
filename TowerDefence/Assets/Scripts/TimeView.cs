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
    public Gauge gauge;
    public bool isFinished {get;set;} = false;
    /*****monoBehaviour*****/
    void Awake()
    {
        gauge.maxValue = (int)timer.time;
        gauge.value = 0;
        timer.onDigitalTimeChanged.Subscribe(time =>
        {
            gauge.value = (int)timer.time - timer.digitalTime;
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
