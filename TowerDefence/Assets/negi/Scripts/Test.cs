using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Test : MonoBehaviour
{

    //編成データを読み書きするときはPrefManager.csにあります

    //編成データ読み書き用
    PrefsManager prefs = new PrefsManager();
    //データ
    Formation formation = new Formation();//マス目部分int[] gridinfo = new int[120] ,船部分 int shiptype;

    public void debugship()
    {
        //編成データ書き込み例
        for (int i = 0; i < 120; i++)
        { 
            formation.gridinfo[i] = Random.Range(100, 200);
        }
        formation.shiptype = 114514;
        prefs.setFormation(formation.gridinfo, formation.shiptype);


        //編成データ読み込み、使用例
        formation = prefs.getFormation();
        for (int i = 0; i < 120; i++)
        {
            Debug.Log(formation.gridinfo[i]);
        }
        Debug.Log(formation.shiptype);


    }
    public void setship()
    {
        for (int i = 0; i < 120; i++)
        {
            formation.gridinfo[i] = Random.Range(100, 200);
        }
        formation.shiptype = 114514;
        Debug.Log(prefs.setFormation(formation.gridinfo, formation.shiptype));
    }

    public void getship()
    {
        formation = prefs.getFormation();
        for (int i = 0; i < 120; i++)
        {
            Debug.Log(formation.gridinfo[i]);
        }
        Debug.Log(formation.shiptype);
    }

    public void deleteship()
    {
        prefs.delete();
    }

}
