using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IcePngnScript : MonoBehaviour
{
    /*****public field*****/
    public IcePool pool;
    public Transform attackTransform;
    /*****Menobehaviour method*****/
    /*****public method*****/
    public void Attack(int power)
    {
        pool.Pop(attackTransform.position);
    }
}
