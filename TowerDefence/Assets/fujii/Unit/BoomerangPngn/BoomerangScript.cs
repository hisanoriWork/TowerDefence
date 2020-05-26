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
    public WeaponScript baseWeapon;
    public BoomerangData data;
    /*****protected field*****/
    protected Transform m_throwPos;
    protected Vector3 m_pos;
    protected Vector3 m_move;
    protected float m_time;
    protected float m_maxTime;
    protected float m_x, m_y;
    /*****Monobehaviour*****/
    void Awake()
    {
        Init(Vector3.zero,null,transform);
        baseWeapon.onHit.Subscribe(other => SEManager.instance.Play("衝突"));
        baseWeapon.onHitUnit.Subscribe(other => other.Hurt(baseWeapon.power));
        baseWeapon.onHitWeapon.Subscribe(other => other.Hit());
    }
    void FixedUpdate()
    {
        if (m_time >= m_maxTime)
        {
            baseWeapon.Despawn();
            return;
        }
        m_time += Time.fixedDeltaTime;
        m_pos.x = m_x * data.xCurve.Evaluate(m_time/m_maxTime);
        m_pos.y = m_y * data.yCurve.Evaluate(m_time/m_maxTime);
        transform.localPosition = m_pos;
        transform.localEulerAngles += -5 * Vector3.forward;

    }
    /*****public method*****/
    public void Init(Vector3 pos, UnitScript unitScript, Transform throwPos)
    {
        baseWeapon.Init(pos, unitScript, true);
        m_throwPos = throwPos;
        m_move = Vector3.zero;
        m_pos = Vector3.zero;
        m_time = 0f;
        m_maxTime = data.time;
        m_x = data.dx;
        m_y = data.dy;
    }
}