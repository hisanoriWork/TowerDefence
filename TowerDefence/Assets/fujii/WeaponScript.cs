using MyLibrary;
using System;
using UniRx;
using UnityEngine;
public class WeaponScript : MonoBehaviour
{
    /*****public filed*****/
    public bool canHit { get{return !(m_hitFlag & m_canHitAfterHit);}}
    public UnitScript unit { get { return m_unitScript; } }

    public int power { get { return m_power; } }
    public IObservable<Unit> onDespawned
    { get { return m_despawnSubject; } }
    public IObservable<UnitScript> onHitUnit
    { get { return m_hitUnit; }}
    public IObservable<WeaponScript> onHitWeapon
    { get { return m_hitWeapon; } }
    public IObservable<Unit> onHit
    { get { return m_hit; } }
    /*****protected field*****/
    protected Subject<Unit> m_despawnSubject = new Subject<Unit>();
    protected Subject<Unit> m_hit = new Subject<Unit>();
    protected Subject<UnitScript> m_hitUnit = new Subject<UnitScript>();
    protected Subject<WeaponScript> m_hitWeapon = new Subject<WeaponScript>();
    protected UnitScript m_unitScript;
    protected int m_power;
    protected bool m_hitFlag;
    protected bool m_canHitAfterHit;
    /*****monoBehaviour method*****/
    void Awake()
    {
        onDespawned.Subscribe(_ => gameObject.SetActive(false));
    }
    void OnTriggerEnter2D(Collider2D collider)
    {
        if (Pauser.isPaused) return;
        if (!canHit) return;
        if (collider.gameObject.tag == "Outside")
        {
            m_despawnSubject.OnNext(Unit.Default);
            return;
        }
        if (collider.gameObject.tag == "Pngn" | collider.gameObject.tag == "Ship" | collider.gameObject.tag == "Block")
        {
            UnitScript otherUnit = collider.transform.parent.GetComponent<UnitScript>();
            if (otherUnit)
            {
                m_hitFlag = true;
                HitUnit(otherUnit);
                return;
            }
        }
        if (collider.gameObject.tag == "Weapon")
        {
            WeaponScript otherWeapon = collider.transform.parent.GetComponent<WeaponScript>();
            if (otherWeapon)
            {
                m_hitFlag = true;
                HitWeapon(otherWeapon);
            }
        }

    }
    /*****public method*****/
    public void Init(Vector3 pos,UnitScript unitScript = null, bool canHitAfterHit = false)
    {
        transform.position = pos;
        if (transform.localScale.x < 0)
            transform.localScale = Vector3.Scale(transform.localScale, new Vector3(-1, 1, 1));
        m_hitFlag = false;
        m_canHitAfterHit = canHitAfterHit;
        m_unitScript = unitScript;
        m_power = unitScript ? unitScript.power : 0;
        string layer = (unitScript != null && unitScript.playerNum == PlayerNum.Player2) ? Constant.WeaponLayer2 : Constant.WeaponLayer1;
        Utility.SetLayerRecursively(gameObject, layer);
    }

    public void Despawn()
    {
        m_despawnSubject.OnNext(Unit.Default);
    }

    public void HitUnit(UnitScript unit)
    {
        m_hitUnit.OnNext(unit);
        Hit();
    }

    public void HitWeapon(WeaponScript weapon)
    {
        m_hitWeapon.OnNext(weapon);
        Hit();
    }

    public void Hit()
    {
        m_hit.OnNext(Unit.Default);
    }
}
