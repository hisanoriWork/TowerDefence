using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CannonPngnScript : MonoBehaviour
{
    /*****public field*****/
    public ShellPool pool;
    public Transform attackTransform;
    /*****Menobehaviour method*****/
    /*****public method*****/
    public void Attack(int power)
    {
        pool.Pop(attackTransform.position);
    }
}
