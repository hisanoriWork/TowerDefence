﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Test : MonoBehaviour
{

    //編成データを読み書きするときはPrefManager.csにあります

    //編成データ読み書き用
    PrefsManager prefs = new PrefsManager();
    //データ
    Formation formation = new Formation();//マス目部分int[] gridinfo = new int[100] ,船部分 int shiptype;

    public void debugship()
    {
        //編成データ書き込み例
        for (int i = 0; i < 10; i++)
        {
            for (int j = 0; j < 10; j++)
            {
                formation.gridinfo[i,j] = Random.Range(100, 200);
            }
        }
        formation.shiptype = 114514;
        prefs.SetFormation(formation.gridinfo, formation.shiptype);


        //編成データ読み込み、使用例
        formation = prefs.GetFormation();
        for (int i = 0; i < 10; i++)
        {
            for (int j = 0; j < 10; j++)
            {
                Debug.Log(formation.gridinfo[i, j]);
            }
        }
        Debug.Log(formation.shiptype);


    }
    public void setship()
    {
        for (int i = 0; i < 10; i++)
        {
            for (int j = 0; j < 10; j++)
            {
                formation.gridinfo[i, j] = Random.Range(100, 200);
            }
        }
        formation.shiptype = 114514;
        Debug.Log(prefs.SetFormation(formation.gridinfo, formation.shiptype));
    }

    public void getship()
    {
        formation = prefs.GetFormation();
        for (int i = 0; i < 10; i++)
        {
            for (int j = 0; j < 10; j++)
            {
                Debug.Log(formation.gridinfo[i,j]);
            }
        }
        Debug.Log(formation.shiptype);
    }

    public void deleteship()
    {
        prefs.Delete();
    }

}
