using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class UnitScript : MonoBehaviour
{

    /*****public field*****/
    
    public enum Attack_Strategy
    {
        ConcentratedAttack, //集中攻撃
        ClosestEnemy, //一番近い敵をターゲット

    }
    public UnitData Data;
    public Collider2D Collider;
    public bool Invension = false;
    public Attack_Strategy Strategy;
    [System.NonSerialized] public bool IsDead = false;
    public delegate void IntFanc(int i); // delegate 型の宣言
    public IntFanc AttackEvent = null; // delegate 型の変数を宣言
    public IntFanc HurtEvent = null;
    public delegate void VoidFanc();
    public VoidFanc DeadEvent = null;
    /*****protected field*****/
    protected Animator m_Animator;
    protected int m_Power;
    protected int m_MaxHP;
    protected int m_HP;
    protected float m_MaxCT;
    protected float m_CT;
    protected Vector3 m_MovePos = Vector3.zero;
    protected readonly int m_HashIdleParam = Animator.StringToHash("IdleParam");
    protected readonly int m_HashAttackParam = Animator.StringToHash("AttackParam");
    protected readonly int m_HashHurtParam = Animator.StringToHash("HurtParam");
    protected readonly int m_HashDeadParam = Animator.StringToHash("DeadParam");

    /*****Monobehavour method*****/
    //void Awake()
    //{
    //    Init();
    //}

    void Update()
    {
        transform.position += m_MovePos;
        m_MovePos = Vector3.zero;

        if (m_CT < 0) Attack(m_Power);
        
        if (m_HP < 0 & !IsDead) Dead();

        if (IsDead)
        {
            gameObject.SetActive(false);
        }
    }

    void FixedUpdate()
    {
        if (m_CT > 0) m_CT -= Time.fixedDeltaTime;
    }

    /*****public method*****/
    //初期化ですStart()にいれてください
    public void Init(IntFanc attack,IntFanc hurt, VoidFanc dead)
    {
        AttackEvent = attack;
        HurtEvent = hurt;
        DeadEvent = dead;
        Init();
    }

    public void Init()
    {
        m_Power = Data.Power;
        m_MaxHP = Data.HP;
        m_HP = Data.HP;
        m_MaxCT = Data.CT;
        m_CT = Data.CT;
        Invert(Invension);
        m_Animator = GetComponent<Animator>();
    }

    //スプライトを左右反対にする
    public void Invert(bool b = true)
    {
        Vector3 size = transform.localScale;
        size.x *= (b ^ Invension) ? -1 : 1;
        transform.localScale = size;
        Invension = b;
    }

    //攻撃をして，Attackアニメーションに移行する
    public void Attack(int power)
    {
        if (m_Animator != null
            &&m_Animator.GetCurrentAnimatorStateInfo(0).IsName("Idle")
            && !m_Animator.IsInTransition(0))
        {
            m_CT = m_MaxCT;
            m_Animator.SetTrigger(m_HashAttackParam);
            m_Animator.SetTrigger(m_HashIdleParam);
            if (AttackEvent != null)
            {
                AttackEvent(power);
            }
        }
    }

    //死んで，Deadアニメーションに移行する
    public void Dead()
    {
        if (m_Animator != null
            &&!m_Animator.GetCurrentAnimatorStateInfo(0).IsName("Dead")
            && !m_Animator.IsInTransition(0))
        {
            IsDead = true;
            m_Animator.SetTrigger(m_HashDeadParam);
            if (DeadEvent != null)
            {
                DeadEvent();
            }
        }
    }

    //ダメージを負い，Hurtアニメーションに移行する
    public void Hurt(int damage)
    {
        if ( m_Animator != null
            && !m_Animator.GetCurrentAnimatorStateInfo(0).IsName("Hurt")
            && !m_Animator.IsInTransition(0))
        {
            m_HP -= damage;
            if (m_Animator != null)
            {
                m_Animator.SetTrigger(m_HashHurtParam);
                m_Animator.SetTrigger(m_HashIdleParam);
            }
            if (HurtEvent != null)
            {
                HurtEvent(damage);
            }
        }
    }

    //回復する
    public void Recover(int point)
    {
        m_HP = (m_HP + point > m_MaxHP) ? m_MaxHP : m_MaxHP + point;
    }

    //移動する
    public void Move(Vector3 vec)
    {
        m_MovePos += vec;
    }

}

