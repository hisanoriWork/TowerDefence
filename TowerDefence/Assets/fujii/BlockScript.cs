using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlockScript : MonoBehaviour
{
    public UnitScript baseUnit;
    void Update()
    {
        if (!Pauser.isPaused)
        {
            //攻撃，ダメージを負う，死ぬ以外の処理を書く
            if (Input.GetKeyDown(KeyCode.Alpha2))
            {
                Debug.Log("ブロックにダメージを与えた");
                baseUnit.Hurt(10);
            }
        }
    }
}
