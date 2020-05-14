using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UniRx;
using System;
public class UIManager : MonoBehaviour
{
    /*****public field*****/
    public GameObject OptionCanvas;
    public IObservable<Unit> whenDisplayed { get { return displaySubject; } }
    public IObservable<Unit> whenHidden { get { return hideSubject; } }
    /*****protected field*****/
    protected Subject<Unit> displaySubject = new Subject<Unit>();
    protected Subject<Unit> hideSubject = new Subject<Unit>();

    public void Display()
    {
        OptionCanvas.SetActive(true);
        displaySubject.OnNext(Unit.Default);
    }

    public void Hide()
    {
        OptionCanvas.SetActive(true);
        hideSubject.OnNext(Unit.Default);
    }
}
 