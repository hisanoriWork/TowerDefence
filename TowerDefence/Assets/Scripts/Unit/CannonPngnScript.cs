using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CannonPngnScript : MonoBehaviour
{
    /*****public field*****/
    public UnitScript beseScript;
    public ShellPool pool;
    public Transform attackTransform;
    /*****Menobehaviour method*****/
    InfoToWeapon info;
    public void Awake()
    {
        info = new InfoToWeapon(attackTransform.position, "PlayerWeapon1", beseScript.power);
        Debug.Log("pos選択");
    }
    /*****public method*****/
    public void Attack(int power)
    {
        info.pos = attackTransform.position;
        if (!beseScript.isInverted)
            info.layer = "PlayerWeapon1";
        else
            info.layer = "PlayerWeapon2";
        ShellObject obj = pool.Pop(info);
    }
}
