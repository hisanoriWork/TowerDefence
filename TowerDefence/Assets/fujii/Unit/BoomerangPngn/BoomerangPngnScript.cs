using UnityEngine;

public class BoomerangPngnScript : MonoBehaviour
{
    /*****public field*****/
    public BoomerangPool pool;
    public Transform attackTransform;
    /*****Menobehaviour method*****/
    /*****public method*****/
    public void Attack(int power)
    {
        pool.Pop(attackTransform.position);
    }
}
