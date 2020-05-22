using System;
using UniRx;
using UnityEngine;
public class ExplosionScript : MonoBehaviour
{
    public int power;
    public float time;
    public float initialSize, finalSize;

    public IObservable<Unit> onDespawned
    { get { return m_despawnSubject; } }
    /*****private field*****/
    protected Subject<Unit> m_despawnSubject = new Subject<Unit>();
    protected int m_power = 0;
    protected float m_time = 0;
    protected float m_size = 0;

    void FixedUpdate()
    {
        m_time += Time.fixedDeltaTime;
        if (m_time >= time)
        {
            gameObject.SetActive(false);
            m_despawnSubject.OnNext(Unit.Default);
        }
        m_size = Mathf.Lerp(initialSize, finalSize, m_time / time);
        transform.localScale = new Vector3(m_size, m_size, 1);
    }

    void OnTriggerEnter2D(Collider2D collider)
    {
        if (m_power > 0)
        {
            if (collider.gameObject.tag == "Pngn" | collider.gameObject.tag == "Ship" | collider.gameObject.tag == "Block")
            {
                collider.transform.parent.GetComponent<UnitScript>().Hurt(m_power);
            }
            if (collider.gameObject.tag == "Weapon")
            {
                WeaponScript otherWeapon = collider.transform.parent.GetComponent<WeaponScript>();
                if (otherWeapon)
                {
                    otherWeapon.Hit();
                }
            }
        }
        
    }
    public void Init(int power = -10, float initialSize = -10f, float finalSize = -10f, float time = -10f)
    {
        m_power = power < 0 ? 1 : power;
        this.initialSize = initialSize < 0f ? 0f : initialSize;
        this.finalSize = finalSize < 0f ? 1f : finalSize;
        this.time = time < 0 ? 1 : time;
        m_time = 0;
    }
}

