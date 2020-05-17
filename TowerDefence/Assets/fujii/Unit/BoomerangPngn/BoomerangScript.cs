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
    public IObservable<Unit> onDespawned
    { get { return m_despawnSubject; } }
    /*****protected field*****/
    protected Subject<Unit> m_despawnSubject = new Subject<Unit>();
    protected UnitScript m_unitScript;
    protected float m_angle;
    protected Vector3 m_pos;
    protected float m_velocity;
    protected int m_power;
    protected Transform m_throwPos;
    protected Vector3 m_o,m_a, m_b, m_p, m_q, m_r;
    protected int m_state;
    protected float m_time;
    /*****Monobehaviour*****/
    void Awake()
    {
        Init(Vector3.zero, m_unitScript,transform);
    }

    void FixedUpdate()
    {
        m_time += Time.fixedDeltaTime;
        if (m_time >= 1f)
        {
            m_time = 0f;
            m_state++;
        }
        SetPoint();
        switch (m_state)
        {
            case 0:
                m_pos.x = Mathf.Lerp(m_o.x, m_a.x, m_time) + Mathf.Lerp(0, m_p.x, Mathf.Sin(Mathf.PI * m_time));
                m_pos.y = Mathf.Lerp(m_o.y, m_a.y, m_time) + Mathf.Lerp(0, m_p.x, Mathf.Sin(Mathf.PI * m_time));
                break;
            case 1:
                m_pos.x = Mathf.Lerp(m_a.x, m_b.x, m_time) + Mathf.Lerp(0, m_q.x, Mathf.Sin(Mathf.PI * m_time));
                m_pos.y = Mathf.Lerp(m_a.y, m_b.y, m_time) + Mathf.Lerp(0, m_q.x, Mathf.Sin(Mathf.PI * m_time));
                break;
            case 2:
                m_pos.x = Mathf.Lerp(m_b.x, m_o.x, m_time) + Mathf.Lerp(0, m_r.x, Mathf.Sin(Mathf.PI * m_time));
                m_pos.y = Mathf.Lerp(m_b.y, m_o.y, m_time) + Mathf.Lerp(0, m_r.y, Mathf.Sin(Mathf.PI * m_time));
                break;
            case 3:
                m_despawnSubject.OnNext(Unit.Default);
                break;

        }
        transform.position = m_pos;
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
        m_angle = data.angle + UnityEngine.Random.Range(-data.deviation, data.deviation);
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
        SetPoint();
        m_state = 0;
        m_time = 0f;
    }

    private void SetPoint()
    {
        m_o = m_throwPos.position;
        m_a = m_throwPos.position + data.a;
        m_b = m_throwPos.position + data.b;
        m_p = data.dOA * Vector3.Normalize(Quaternion.Euler(0, 0, 90) * m_a);
        m_q = data.dAB * Vector3.Normalize(Quaternion.Euler(0, 0, 90) * (m_b - m_a));
        m_r = data.dOB * Vector3.Normalize(Quaternion.Euler(0, 0, 90) * -m_b);
        Debug.Log("o:" + m_o + "a:" + m_a + "b:" + m_b + "p:" + m_p + "q:" + m_q + "r:" + m_r);
    }
}