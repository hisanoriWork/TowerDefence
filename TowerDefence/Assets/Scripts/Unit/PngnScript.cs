using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PngnScript : MonoBehaviour
{
    /*****public field*****/
    public UnitScript Base;
    protected Rigidbody2D Rigidbody;

    // Start is called before the first frame update
    void Start()
    {
        Rigidbody = GetComponent<Rigidbody2D>();
    }

    /*****Monobehaviour method*****/
    void Update()
    {
        if (Base.isPlaying)
        {
            //攻撃，ダメージを負う，死ぬ以外の処理を書く
            if (Input.GetKeyDown(KeyCode.Alpha1))
            {
                int damage = Random.Range(1, 11);
                Base.Hurt(damage);
            }
        }
    }
    /*****protected method*****/
    public void Attack(int damage)
    {
    }

    public void Hurt(int damage)
    {
    }

    public void Dead()
    {
    }
}
