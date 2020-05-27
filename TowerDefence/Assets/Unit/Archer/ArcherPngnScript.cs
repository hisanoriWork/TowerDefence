using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArcherPngnScript : MonoBehaviour
{
    /*****public field*****/
    public ArrowPool pool;
    public Transform attackTransform;
    /*****Menobehaviour method*****/
    /*****public method*****/
    public void AttackEvent(int power)
    {
        pool.Pop(attackTransform.position);
    }

    public void ShotVoice()
    {
        SEManager.instance.Play("矢");
    }
}
