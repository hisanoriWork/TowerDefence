using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShellScript : MonoBehaviour
{
    /*****public field*****/
    public MissileData data;
    /*****private field*****/
    protected float m_angle;
    protected float m_speed;
    protected float m_gravity;
    protected Vector3 m_move;
    protected float m_velocity;
    protected int m_power;
    /*****Monobehaviour*****/
    void Awake()
    {
        //Init(Vector3.zero,"PlayerWeapon1",10);
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
        Debug.Log(collider.gameObject.tag);
        if (collider.gameObject.tag == "Pngn" | collider.gameObject.tag == "Ship" | collider.gameObject.tag == "Block")
        {
            collider.GetComponent<UnitScript>().Hurt(m_power);
        }
    }
    /*****public method*****/
    public void Init(Vector3 pos, string layer,int power)
    {
        transform.localPosition = pos;
        Utility.SetLayerRecursively(gameObject, layer);
        m_power = power;
        m_angle = data.angle;
        m_speed = data.speed;
        m_gravity = data.gravity;
        m_move = Vector3.zero;
        m_velocity = 0f;
        m_angle += UnityEngine.Random.Range(-data.deviation, data.deviation);
    }
}