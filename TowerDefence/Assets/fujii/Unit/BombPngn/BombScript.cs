using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UniRx;
using System;
using MyLibrary;

public class BombScript : MonoBehaviour
{
    /*****public field*****/
    public WeaponScript baseWeapon;
    public BombData data;
    public GameObject body;
    public ExplosionScript explosion;
    /*****private field*****/
    protected float m_angle;
    protected float m_speed;
    protected float m_gravity;
    protected Vector3 m_move;
    protected float m_velocity;
    /*****Monobehaviour*****/
    void Awake()
    {
        Init(Vector3.zero);
        baseWeapon.onHit.Subscribe(_ =>
        {
            body.SetActive(false);
            explosion.Init(baseWeapon.power, data.explosionInitialSize, data.explosionFinalSize, data.explosionTime);
            explosion.gameObject.SetActive(true);
        });
        explosion.onDespawned.Subscribe(_ => baseWeapon.Despawn());
    }
    void FixedUpdate()
    {
        if (!baseWeapon.canHit)
            return;
        m_move.x = m_speed * Mathf.Cos(m_angle * Mathf.Deg2Rad) * Time.fixedDeltaTime;
        m_velocity += m_gravity * Time.fixedDeltaTime;
        m_move.y = m_speed * Mathf.Sin(m_angle * Mathf.Deg2Rad) * Time.fixedDeltaTime - m_velocity * Time.fixedDeltaTime;
        transform.localPosition += m_move;
        transform.localEulerAngles += -5 * Vector3.forward;
    }
    /*****public method*****/
    public void Init(Vector3 pos, UnitScript unitScript = null)
    {
        baseWeapon.Init(pos, unitScript, false);
        body.SetActive(true);
        m_speed = data.speed;
        m_gravity = data.gravity;
        m_move = Vector3.zero;
        m_velocity = 0f;
        m_angle = data.angle + UnityEngine.Random.Range(-data.deviation, data.deviation);
    }
}