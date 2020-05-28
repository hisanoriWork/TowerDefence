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
        pool.Pop(attackTransform.position);
        SEManager.instance.Play("現代砲");
    }

    public void Shot(int num)
    {
        if (num > 0)
        {
            pool.Pop(attackTransform.position);
            SEManager.instance.Play("現代砲");
            StartCoroutine(Utility.WaitForSecond(time, () =>
            {
                Shot(--num);
            }));
        }
    }
}
