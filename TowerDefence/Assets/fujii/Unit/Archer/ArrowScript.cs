﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UniRx;
using System;
using MyLibrary;
public class ArrowScript : MonoBehaviour
{
    /*****public field*****/
    public ArrowData data;
    public IObservable<Unit> onDespawned
    { get { return m_despawnSubject; } }
    /*****private field*****/
    protected Subject<Unit> m_despawnSubject = new Subject<Unit>();
    protected UnitScript m_unitScript;
    protected float m_angle;
    protected float m_speed;
    protected float m_gravity;
    protected Vector3 m_move;
    protected float m_velocity;
    protected int m_power;
    protected bool m_hitFlag = false;
    /*****Monobehaviour*****/
    void Awake()
    {
        Init(Vector3.zero, m_unitScript);
    }

    void FixedUpdate()
    {
        if (m_hitFlag) return;
        m_move.x = m_speed * Mathf.Cos(m_angle * Mathf.Deg2Rad) * Time.fixedDeltaTime;
        m_velocity += m_gravity * Time.fixedDeltaTime;
        m_move.y = m_speed * Mathf.Sin(m_angle * Mathf.Deg2Rad) * Time.fixedDeltaTime - m_velocity * Time.fixedDeltaTime;
        transform.localPosition += m_move;
        transform.localEulerAngles = 45 * Mathf.Atan(m_move.y / m_move.x) * Vector3.forward;
    }
    void OnTriggerEnter2D(Collider2D collider)
    {
        if (Pauser.isPaused) return;
        if (!m_hitFlag &(collider.gameObject.tag == "Pngn" | collider.gameObject.tag == "Ship" | collider.gameObject.tag == "Block"))
        {
            m_hitFlag = true;
            collider.transform.parent.GetComponent<UnitScript>().Hurt(m_power);
            StartCoroutine(Utility.WaitForSecond(2f, () => {
                m_despawnSubject.OnNext(Unit.Default);
            }));
        }
    }
    /*****public method*****/
    public void Init(Vector3 pos, UnitScript unitScript)
    {
        onDespawned.Subscribe(_ => gameObject.SetActive(false));
        m_hitFlag = false;
        m_power = 0;
        m_speed = data.speed;
        m_gravity = data.gravity;
        m_move = Vector3.zero;
        m_velocity = 0f;
        m_angle = data.angle + UnityEngine.Random.Range(-data.deviation, data.deviation);
        transform.localEulerAngles = m_angle * Vector3.forward;
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
    }
}