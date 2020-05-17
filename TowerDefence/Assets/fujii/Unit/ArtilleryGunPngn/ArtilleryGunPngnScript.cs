using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArtilleryGunPngnScript : MonoBehaviour
{
    /*****public field*****/
    public BulletPool pool;
    public Transform attackTransform;
    /*****Menobehaviour method*****/
    /*****public method*****/
    public void Attack(int power)
    {
        pool.Pop(attackTransform.position);
    }
}
