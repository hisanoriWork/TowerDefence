using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PngnScript : MonoBehaviour
{
    /*****public field*****/
    public UnitScript baseUnit;
    public GameObject ground = null;
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
    void OnCollisionEnter2D(Collision2D collision)
    {
        if (!Pauser.isPaused)
        {
            if ((collision.transform.tag == "Ship" || collision.transform.tag == "Block")
                & this.transform.position.y >= collision.contacts[0].point.y)
            {
                ground = collision.gameObject;
            }
        }
    }
    void OnCollisionExit2D(Collision2D collision)
    {
        if (!Pauser.isPaused)
        {
            if (ground.GetInstanceID() == collision.gameObject.GetInstanceID())
            {
                baseUnit.Hurt(10000);
            }
        }
    }
}
