﻿using UniRx;
using UnityEngine;
public class StoneBulletScript : MonoBehaviour
{
    /*****public field*****/
    public WeaponScript baseWeapon;
    public StoneBulletData data;
    /*****protected field*****/
    protected float m_angle;
    protected float m_speed;
    protected float m_gravity;
    protected Vector3 m_move;
    protected float m_velocity;
    /*****Monobehaviour*****/
    void Awake()
    {
        Init(Vector3.zero);
        baseWeapon.onHit.Subscribe(_ =>{
            baseWeapon.Despawn();
            SEManager.instance.Play("衝突");
        });
        baseWeapon.onHitUnit.Subscribe(other => other.Hurt(baseWeapon.power));
        baseWeapon.onHitWeapon.Subscribe(other => other.Hit());
    }
    void FixedUpdate()
    {
        m_move.x = m_speed * Mathf.Cos(m_angle * Mathf.Deg2Rad) * Time.fixedDeltaTime;
        m_velocity += m_gravity * Time.fixedDeltaTime;
        m_move.y = m_speed * Mathf.Sin(m_angle * Mathf.Deg2Rad) * Time.fixedDeltaTime - m_velocity * Time.fixedDeltaTime;
        transform.localPosition += m_move;
        transform.localEulerAngles = 45 * Mathf.Atan(m_move.y / m_move.x) * Vector3.forward;
    }
    /*****public method*****/
    public void Init(Vector3 pos, UnitScript unitScript = null)
    {
        baseWeapon.Init(pos, unitScript, false);
        m_speed = data.speed;
        m_gravity = data.gravity;
        m_move = Vector3.zero;
        m_velocity = 0f;
        if (unitScript)
        {
            if(unitScript.transform.lossyScale.x >=0)
                m_angle = unitScript.transform.eulerAngles.z + data.angle + UnityEngine.Random.Range(-data.deviation, data.deviation);
            else
                m_angle = -unitScript.transform.eulerAngles.z + data.angle + UnityEngine.Random.Range(-data.deviation, data.deviation);
        }
        transform.localEulerAngles = m_angle * Vector3.forward;


    }
}
