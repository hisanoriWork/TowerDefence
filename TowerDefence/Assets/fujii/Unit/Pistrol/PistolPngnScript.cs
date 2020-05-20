using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PistolPngnScript : MonoBehaviour
{
    /*****public field*****/
    public StoneBulletPool pool;
    public Transform attackTransform;
    /*****public method*****/
    public void Attack(int power)
    {
        pool.Pop(attackTransform.position);
    }
}
