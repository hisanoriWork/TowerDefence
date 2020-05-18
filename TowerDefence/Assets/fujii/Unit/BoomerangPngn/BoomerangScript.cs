using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UniRx;
using System;
using MyLibrary;
public class BoomerangScript : MonoBehaviour
{
    /*****public field*****/
    public BoomerangData data;
    public Rigidbody2D rb;
    public IObservable<Unit> onDespawned
    { get { return m_despawnSubject; } }
    /*****protected field*****/
    protected Subject<Unit> m_despawnSubject = new Subject<Unit>();
    protected UnitScript m_unitScript;
    protected Vector3 m_pos;
    protected Vector3 m_move;
    protected int m_power;
    protected Transform m_throwPos;
    protected float m_time;
    protected float m_maxTime;
    protected float m_x, m_y;
    //protected float 
    /*****Monobehaviour*****/
    void Awake()
    {
        Init(Vector3.zero, m_unitScript,transform);
    }

    void FixedUpdate()
    {
        if (m_time >= m_maxTime)
        {
            m_despawnSubject.OnNext(Unit.Default);
            return;
        }
        m_time += Time.fixedDeltaTime;
        m_pos.x = m_x * data.xCurve.Evaluate(m_time/m_maxTime);
        m_pos.y = m_y * data.yCurve.Evaluate(m_time/m_maxTime);
        transform.localPosition = m_pos;
        transform.localEulerAngles += -5 * Vector3.forward;

    }
    void OnTriggerEnter2D(Collider2D collider)
    {

        if (Pauser.isPaused) return;
        if (collider.gameObject.tag == "Pngn" | collider.gameObject.tag == "Ship" | collider.gameObject.tag == "Block")
        {
            collider.transform.parent.GetComponent<UnitScript>().Hurt(m_power);
        }
    }
    /*****public method*****/
    public void Init(Vector3 pos, UnitScript unitScript, Transform throwPos)
    {
        onDespawned.Subscribe(_ => gameObject.SetActive(false));
        m_power = 0;
        m_pos = Vector3.zero;
        transform.position = pos;
        if (transform.localScale.x < 0)
            transform.localScale = Vector3.Scale(transform.localScale, new Vector3(-1, 1, 1));
        m_unitScript = unitScript;
        string layer = Constant.WeaponLayer1;
        if (unitScript != null)
        {
            if (unitScript.playerNum == PlayerNum.Player2) layer = Constant.WeaponLayer2;
            m_power = unitScript.power;
        }
        Utility.SetLayerRecursively(gameObject, layer);
        m_throwPos = throwPos;
        m_time = 0f;
        m_maxTime = data.time;
        m_x = data.dx;
        m_y = data.dy;
    }
}