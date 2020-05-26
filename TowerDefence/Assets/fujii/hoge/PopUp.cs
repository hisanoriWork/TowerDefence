using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UniRx;
using UniRx.Triggers;


public class PopUp : MonoBehaviour
{
    public enum State { Open, Close, UnUsed }

    public State state { get; private set; }
    public TweenScale open, close;
    void Start()
    {
        open.Setup(gameObject);
        open.scaleEndAsObservable.Subscribe(_ => state = State.Open);
        close.Setup(gameObject);
        close.scaleEndAsObservable.Subscribe(_ => state = State.Close);
    }

    public void Open()
    {
        open.Play();
    }
    public void Close()
    {
        close.Play();
    }

    public void Toggle()
    {
        switch (state)
        {
            case State.UnUsed:
            case State.Close:
                open.Play();
                break;
            case State.Open:
                close.Play();
                break;
        }
    }
}




[Serializable]
public class TweenScale
{
    public Vector3 from, to;
    public float duration;
    public AnimationCurve curve = new AnimationCurve(new Keyframe(0, 0), new Keyframe(1, 1));
    public UnityEvent onBegin, onEnd;
    private GameObject target;

    private Subject<Unit> scaleStartStream = new Subject<Unit>();
    public IObservable<Unit> scaleStartAsObservable { get { return scaleStartStream.AsObservable(); } }

    private Subject<Unit> scaleEndStream = new Subject<Unit>();
    public IObservable<Unit> scaleEndAsObservable { get { return scaleEndStream.AsObservable(); } }

    public void Setup(GameObject t)
    {
        target = t;
        scaleStartAsObservable.Subscribe(_ => onBegin.Invoke());
        scaleEndAsObservable.Subscribe(_ => onEnd.Invoke());
        scaleEndAsObservable.Subscribe(_ => target.transform.localScale = Vector3.Lerp(from, to, curve.Evaluate(1.0F)));
    }

    public void Play()
    {
        scaleStartStream.OnNext(Unit.Default);
        Observable.EveryUpdate()
            .Take(System.TimeSpan.FromSeconds(duration))
            .Select(_ => Time.deltaTime)
            .Scan((acc, current) => acc + current)
            .Subscribe(time => {
                float t = time / duration;
                target.transform.localScale = Vector3.Lerp(from, to, curve.Evaluate(t));
            },
            _ => { },
            () => scaleEndStream.OnNext(Unit.Default)
            ).AddTo(target);
    }
}