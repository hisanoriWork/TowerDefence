using System;
using UniRx;
using UnityEngine;

public class WeaponScript : MonoBehaviour
{
    public IObservable<Unit> onDespawned
    { get { return m_despawnSubject; } }
    /*****private field*****/
    protected Subject<Unit> m_despawnSubject = new Subject<Unit>();
    protected UnitScript m_unitScript;
    protected bool m_hitFlag;
}
