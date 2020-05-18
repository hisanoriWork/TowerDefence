using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using MyLibrary;
public class ArtilleryGunPngnScript : MonoBehaviour
{
    /*****public field*****/
    public BulletPool pool;
    public Transform attackTransform;
    public float time;
    public int num;
    /*****Menobehaviour method*****/
    /*****public method*****/
    public void Attack(int power)
    {
        Shot(num);
    }

    public void Shot(int num)
    {
        if (num > 0)
        {
            pool.Pop(attackTransform.position);
            StartCoroutine(Utility.WaitForSecond(time, () =>
            {
                Shot(--num);
            }));
        }
    }
}
