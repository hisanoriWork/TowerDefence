using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PngnScript : MonoBehaviour
{
    /*****public field*****/
    public UnitScript baseUnit;
    /*****protected field*****/
    protected UnitScript m_groundUnit;
    /*****Monobehaviour method*****/

    void Update()
    {
        if (!Pauser.isPaused)
        {
            //攻撃，ダメージを負う，死ぬ以外の処理を書く
            if (Input.GetKeyDown(KeyCode.Alpha1))
            {
                int damage = Random.Range(1, 11);
                baseUnit.Hurt(damage);
            }
        }
    }

    void OnCollisionEnter2D(Collision2D col)
    {
        if (col.gameObject.tag == "Outside")
        {
            baseUnit.Dead();
            return;
        }
        if (m_groundUnit != null)
            return;

        if (col.gameObject.tag == "Ship" | col.gameObject.tag == "Block")
        {
            UnitScript unit = col.transform.GetComponent<UnitScript>();
            if (unit != null)
            {
                m_groundUnit = unit;
                unit.deadEvent.AddListener(() => {
                    if (baseUnit && baseUnit.isActiveAndEnabled)
                        baseUnit.Dead();
                });
            }
        }
    }
    /*****public method*****/
    public void HurtVoice()
    {
        SEManager.instance.Play("ダメージ");
    }
}
