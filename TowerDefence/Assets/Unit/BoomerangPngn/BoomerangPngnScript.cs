using UnityEngine;

public class BoomerangPngnScript : MonoBehaviour
{
    /*****public field*****/
    public BoomerangPool pool;
    public Transform attackTransform;
    /*****Menobehaviour method*****/
    /*****public method*****/
    public void AttackEvent(int power)
    {
        pool.Pop(attackTransform.position);
    }

    public void ShotSE()
    {
        SEManager.instance.Play("矢");
    }
}
