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
    private Vector3 offSet;
    InfoToWeapon posLayer1,posLayer2;
    public void Awake()
    {
        Init();
        
    }
    /*****public method*****/
    public void Init()
    {
        posLayer1 = new InfoToWeapon(rightAttackTransform.position, "PlayerWeapon1",beseScript.power);
        posLayer2 = new InfoToWeapon(rightAttackTransform.position, "PlayerWeapon2",beseScript.power);
    }

    public void Attack(int power)
    {
        if (!beseScript.isInverted)
        {
            ShellObject obj = pool.Pop(posLayer1);
        }
        else
        {
            ShellObject obj = pool.Pop(posLayer2);
        }
    }

    public void Dead()
    { }
}
