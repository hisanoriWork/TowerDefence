using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UniRx;
using System;
using MyLibrary;
public class IceScript : MonoBehaviour
{
    /*****public field*****/
    public WeaponScript baseWeapon;
    public IceData data;
    public Transform body;
    /*****private field*****/
    protected enum e_state
    {
        Accumulate,
        Release,

    }
    protected float m_angle;
    protected float m_speed;
    protected float m_gravity;
    protected Vector3 m_move;
    protected float m_velocity;
    protected int m_canHitNum;
    protected e_state m_state;
    protected float m_time;
    protected Transform m_attackPos;
    /*****Monobehaviour*****/
    void Awake()
    {
        Init(Vector3.zero);
        baseWeapon.onHitUnit.Subscribe(other =>
        {
            other.Hurt(baseWeapon.power);
            SEManager.instance.Play("氷");
            baseWeapon.Despawn();
        });
    }

    void FixedUpdate()
    {
        m_time -= Time.fixedDeltaTime;

        if (m_state == e_state.Accumulate)
        {
            if (m_time <= 0)
            {
                m_state = e_state.Release;
                m_time = data.releaseTime;
                return;
            }
            body.localScale = 1.5f* Vector3.one * Mathf.Lerp(0.3f, 1, (data.releaseTime- m_time) / data.releaseTime);
            if(m_attackPos != null)
                transform.position = m_attackPos.position;
        }
        else
        {
            if (m_time <= 0)
            {
                baseWeapon.Despawn();
            }
            body.localScale = 1.5f* Vector3.one * Mathf.Lerp(0.3f, 1, m_time / data.releaseTime);
            m_move.x = m_speed * Mathf.Cos(m_angle * Mathf.Deg2Rad) * Time.fixedDeltaTime;
            m_velocity += m_gravity * Time.fixedDeltaTime;
            m_move.y = m_speed * Mathf.Sin(m_angle * Mathf.Deg2Rad) * Time.fixedDeltaTime - m_velocity * Time.fixedDeltaTime;
            transform.localPosition += m_move;
            transform.localEulerAngles = Mathf.Atan2(m_move.y, m_move.x) * Mathf.Rad2Deg * Vector3.forward;
        }
    }
    /*****public method*****/
    public void Init(Vector3 pos,Transform attackPos = null, UnitScript unitScript = null)
    {
        baseWeapon.Init(pos, unitScript,true);
        m_attackPos = attackPos;
        m_time = data.accumulateTime;
        m_state = e_state.Accumulate;
        m_speed = data.speed;
        m_gravity = data.gravity;
        m_move = Vector3.zero;
        m_velocity = 0f;
        if (unitScript)
        {
            if (unitScript.transform.lossyScale.x >= 0)
                m_angle = unitScript.transform.eulerAngles.z + data.angle + UnityEngine.Random.Range(-data.deviation, data.deviation);
            else
                m_angle = -unitScript.transform.eulerAngles.z + data.angle + UnityEngine.Random.Range(-data.deviation, data.deviation);
        }
        transform.localEulerAngles = m_angle * Vector3.forward;
    }
}