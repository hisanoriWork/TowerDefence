using System;
using UniRx;
using UnityEngine;
using MyLibrary;
public class WeaponScript : MonoBehaviour
{
    /*****public filed*****/
    public IObservable<Unit> onDespawned
    { get { return m_despawnSubject; } }
    public IObservable<Unit> onHit
    { get { return m_hitSubject; }}
    /*****protected field*****/
    protected Subject<Unit> m_despawnSubject = new Subject<Unit>();
    protected Subject<Unit> m_hitSubject = new Subject<Unit>();
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
        if (m_hitFlag & m_canHitAfterHit) return;
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
                m_hitSubject.OnNext(Unit.Default);
                return;
            }
        }
        if (collider.gameObject.tag == "Weapon")
        {
            WeaponScript otherWeapon = collider.transform.parent.GetComponent<WeaponScript>();
            if (otherWeapon)
            {
                m_hitFlag = true;
                m_hitSubject.OnNext(Unit.Default);
            }
        }

    }
    /*****public method*****/
    public void Init(Vector3 pos, bool canHitAfterHit = false,UnitScript unitScript = null)
    {
        transform.position = pos;
        if (transform.localScale.x < 0)
            transform.localScale = Vector3.Scale(transform.localScale, new Vector3(-1, 1, 1));
        m_hitFlag = false;
        m_canHitAfterHit = canHitAfterHit;
        m_unitScript = unitScript;
        m_power = unitScript ? unitScript.power : 0;
        string layer = (!unitScript | unitScript.playerNum == PlayerNum.Player2) ? Constant.WeaponLayer1 : Constant.WeaponLayer2;
        Utility.SetLayerRecursively(gameObject, layer);
    }

    
}
