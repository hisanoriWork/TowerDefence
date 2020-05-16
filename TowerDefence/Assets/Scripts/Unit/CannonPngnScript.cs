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
    /*****public method*****/
    public void Attack(int power)
    {
        ShellObject obj = pool.Pop(attackTransform.position);
    }
}
