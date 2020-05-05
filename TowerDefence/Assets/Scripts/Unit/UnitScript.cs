﻿using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class UnitScript : MonoBehaviour
{
    public enum Attack_Strategy
    {
        ConcentratedAttack, //集中攻撃
        ClosestEnemy, //一番近い敵をターゲット

    }
    [Serializable] protected class IntEvent : UnityEvent<int> { }

    /*****public field*****/
    public UnitData data;
    public int power { get { return m_power; } }
    public int maxHP { get { return m_maxHP; } }
    public int HP { get { return m_HP; } }
    public float maxCT { get { return m_maxCT; } }
    public float CT { get { return m_CT; } }

    public bool isPlaying { get; set; } = true;
    /*****protected field*****/
    protected Animator m_animator;
    protected int m_power;
    protected int m_maxHP;
    protected int m_HP;
    protected float m_maxCT;
    protected float m_CT;
    protected Vector3 m_movePos = Vector3.zero;
    protected bool m_invension = false;
    protected bool m_isDead = false;
    

    protected readonly int m_HashIdleParam = Animator.StringToHash("IdleParam");
    protected readonly int m_HashAttackParam = Animator.StringToHash("AttackParam");
    protected readonly int m_HashHurtParam = Animator.StringToHash("HurtParam");
    protected readonly int m_HashDeadParam = Animator.StringToHash("DeadParam");

    [SerializeField] protected IntEvent attackEvent = null;
    [SerializeField] protected IntEvent hurtEvent = null;
    [SerializeField] protected UnityEvent deadEvent = null;

    protected Attack_Strategy m_strategy;

    /*****Monobehavour method*****/
    void Awake()
    {
        Init();
    }

    void Update()
    {
        if (isPlaying)
        {
            transform.position += m_movePos;
            m_movePos = Vector3.zero;

            if (m_CT < 0) Attack(m_power);

            if (m_HP < 0 & !m_isDead) Dead();

            if (m_isDead)
            {
                gameObject.SetActive(false);
            }
        }
    }

    void FixedUpdate()
    {
        if (isPlaying)
        {
            if (m_CT > 0) m_CT -= Time.fixedDeltaTime;
        }
    }

    /*****public method*****/
    //初期化
    public void Init()
    {
        m_power = data.Power;
        m_maxHP = data.HP;
        m_HP = data.HP;
        m_maxCT = data.CT;
        m_CT = data.CT;
        Invert(m_invension);
        m_animator = GetComponent<Animator>();
    }

    //スプライトを左右反対にする
    public void Invert(bool b = true)
    {
        Vector3 size = transform.localScale;
        size.x *= (b ^ m_invension) ? -1 : 1;
        transform.localScale = size;
        m_invension = b;
    }

    //攻撃をして，Attackアニメーションに移行する
    public void Attack(int power)
    {
        if (m_animator != null
            &&m_animator.GetCurrentAnimatorStateInfo(0).IsName("Idle")
            && !m_animator.IsInTransition(0))
        {
            m_CT = m_maxCT;
            m_animator.SetTrigger(m_HashAttackParam);
            m_animator.SetTrigger(m_HashIdleParam);
            
            attackEvent.Invoke(power);
        }
    }

    //死んで，Deadアニメーションに移行する
    public void Dead()
    {
        if (m_animator != null
            &&!m_animator.GetCurrentAnimatorStateInfo(0).IsName("Dead")
            && !m_animator.IsInTransition(0))
        {
            m_isDead = true;
            m_animator.SetTrigger(m_HashDeadParam);
            
            deadEvent.Invoke();
        }
    }

    //ダメージを負い，Hurtアニメーションに移行する
    public void Hurt(int damage)
    {

        if ( m_animator != null
            && !m_animator.GetCurrentAnimatorStateInfo(0).IsName("Hurt")
            && !m_animator.IsInTransition(0))
        {
            m_HP -= damage;
            if (m_animator != null)
            {
                m_animator.SetTrigger(m_HashHurtParam);
                m_animator.SetTrigger(m_HashIdleParam);
            }
            hurtEvent.Invoke(damage);
        }
    }

    //回復する
    public void Recover(int point)
    {
        m_HP = (m_HP + point > m_maxHP) ? m_maxHP : m_maxHP + point;
    }

    //移動する
    public void Move(Vector3 vec)
    {
        m_movePos += vec;
    }

}

