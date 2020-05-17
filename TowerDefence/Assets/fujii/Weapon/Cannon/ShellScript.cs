using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UniRx;
using System;
using MyLibrary;
public class ShellScript : MonoBehaviour
{
    /*****public field*****/
    public CannonData data;
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
    /*****Monobehaviour*****/
    void Awake()
    {
        Init(Vector3.zero,m_unitScript);
    }
    void FixedUpdate()
    {
            m_move.x = m_speed * Mathf.Cos(m_angle * Mathf.Deg2Rad) * Time.fixedDeltaTime;
            m_velocity += m_gravity * Time.fixedDeltaTime;
            m_move.y = m_speed * Mathf.Sin(m_angle * Mathf.Deg2Rad) * Time.fixedDeltaTime - m_velocity * Time.fixedDeltaTime;
            transform.localPosition += m_move;
    }
    void OnTriggerEnter2D(Collider2D collider)
    {
        if (!Pauser.isPaused)
        {
            if (collider.gameObject.tag == "Pngn" | collider.gameObject.tag == "Ship" | collider.gameObject.tag == "Block")
            {
                collider.transform.parent.GetComponent<UnitScript>().Hurt(m_power);
                m_despawnSubject.OnNext(Unit.Default);
            }
        }
    }
    /*****public method*****/
    public void Init(Vector3 pos,UnitScript unitScript)
    {
        transform.position = pos;
        onDespawned.Subscribe(_ => gameObject.SetActive(false));
        m_power = 0;
        m_angle = data.angle;
        m_speed = data.speed;
        m_gravity = data.gravity;
        m_move = Vector3.zero;
        m_velocity = 0f;
        m_angle += UnityEngine.Random.Range(-data.deviation, data.deviation);
        m_unitScript = unitScript;
        string layer = Constant.WeaponLayer1;
        if (unitScript != null)
        {
            Debug.Log(unitScript.playerNum);
            if (unitScript.playerNum == PlayerNum.Player2) layer = Constant.WeaponLayer2;
            m_power = unitScript.power;
        }
        Utility.SetLayerRecursively(gameObject, layer);
    }
}