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
    public BombData data;
    public GameObject body;
    public ExplosionScript explosion;
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
    protected bool m_hitFlag;
    /*****Monobehaviour*****/
    void Awake()
    {
        Init(Vector3.zero, m_unitScript);
        explosion.onDespawned.Subscribe(_ => m_despawnSubject.OnNext(Unit.Default));
    }
    void FixedUpdate()
    {
        if (m_hitFlag)
            return;
        m_move.x = m_speed * Mathf.Cos(m_angle * Mathf.Deg2Rad) * Time.fixedDeltaTime;
        m_velocity += m_gravity * Time.fixedDeltaTime;
        m_move.y = m_speed * Mathf.Sin(m_angle * Mathf.Deg2Rad) * Time.fixedDeltaTime - m_velocity * Time.fixedDeltaTime;
        transform.localPosition += m_move;
        transform.localEulerAngles += - 5 * Vector3.forward;
    }
    void OnTriggerEnter2D(Collider2D collider)
    {
        if (Pauser.isPaused) return;
        if (m_hitFlag) return;
        if (collider.gameObject.tag == "Outside")
        {
            m_despawnSubject.OnNext(Unit.Default);
            return;
        }
        if (collider.gameObject.tag == "Pngn" | collider.gameObject.tag == "Ship" | collider.gameObject.tag == "Block")
        {
            m_hitFlag = true;
            body.SetActive(false);
            explosion.Init(m_power, data.explosionInitialSize, data.explosionFinalSize, data.explosionTime);
            explosion.gameObject.SetActive(true);
        }
    }
    /*****public method*****/
    public void Init(Vector3 pos, UnitScript unitScript)
    {
        body.SetActive(true);
        onDespawned.Subscribe(_ => gameObject.SetActive(false));
        m_power = 0;
        m_speed = data.speed;
        m_gravity = data.gravity;
        m_move = Vector3.zero;
        m_velocity = 0f;
        m_hitFlag = false;
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
    }
}