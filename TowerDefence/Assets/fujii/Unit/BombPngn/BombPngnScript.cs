using UnityEngine;

public class BombPngnScript : MonoBehaviour
{
    /*****public field*****/
    public BombPool pool;
    public Transform attackTransform;
    /*****Menobehaviour method*****/
    /*****public method*****/
    public void Attack(int power)
    {
        pool.Pop(attackTransform.position);
    }
}
