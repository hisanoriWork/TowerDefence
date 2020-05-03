using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PistolPngnScript : MonoBehaviour
{
    /*****public field*****/
    public UnitScript Base;
    public Rigidbody2D Rigidbody;

    // Start is called before the first frame update
    void Start()
    {
        //Bese.InitをStartに入れてください
        //第１引数：攻撃するときにしてほしい関数名(引数int Power)
        //第２引数：攻撃を受けるするときにしてほしい関数名(引数はint damage)
        //第３引数：死ぬときにしてほしい関数名(引数void)
        Base.Init(Attack, Hurt, Dead);
        Debug.Log("ピストルペンギンはデバッグのため１キーで，1から10のダメージを食らうようにしてます");
    }

    /*****Monobehaviour method*****/
    void Update()
    {
        //攻撃，ダメージを負う，死ぬ以外の処理を書く
        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            int damage = Random.Range(1, 11);
            Base.Hurt(damage);
        }
    }
    /*****protected method*****/
    protected void Attack(int damage)
    {
        Debug.Log("ピストルペンギンは攻撃をしました");
    }

    protected void Hurt(int damage)
    {
        Debug.Log("ピストルペンギンは" + damage + "攻撃を受けました");
    }

    protected void Dead()
    {
        //下に死んだときの処理を描く
        Debug.Log("ピストルペンギンはやられてしまいました");
    }
}
