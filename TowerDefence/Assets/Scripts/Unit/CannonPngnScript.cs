using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CannonPngnScript : MonoBehaviour
{
    /*****public field*****/
    public UnitScript beseScript;
    public ShellPool pool;
    public Transform rightAttackTransform;
    public Transform leftAttackTransform;
    /*****Menobehaviour method*****/
    InfoToWeapon info1,info2;
    public void Awake()
    {
        Init();
    }

    public void Start()
    {
        GameObject obj = new GameObject("ShellPool");
        pool = obj.AddComponent(typeof(ShellPool)) as ShellPool;
        info1 = new InfoToWeapon(rightAttackTransform.position, "PlayerWeapon1", beseScript.power);
        info2 = new InfoToWeapon(rightAttackTransform.position, "PlayerWeapon2", beseScript.power);
    }
    /*****public method*****/
    public void Init()
    {
    }

    public void Attack(int power)
    {
        if (!beseScript.isInverted)
        {
            ShellObject obj = pool.Pop(info1);
        }
        else
        {
            ShellObject obj = pool.Pop(info2);
        }
    }
}
