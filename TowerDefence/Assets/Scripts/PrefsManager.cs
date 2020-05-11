using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

/*[Serializable]*/


public class Formation
{
    public bool formationDataExists;
    //1がデータあり、0がなし

    public int[,] gridinfo = new int[10,10];
    //座標の指定の仕方は
    //90 91 92 93 94 95 ... 99
    //...
    //...
    //...
    //...
    //...
    //...
    //...
    //10 11 12 13 14 15 ... 19
    //00 01 02 03 04 05 ... 09

    //0は「存在しない」


    public int shiptype;
    //0は「存在しない」
}

public class FormationForJson
{
    public bool formationDataExists;
    //1がデータあり、0がなし

    public int[] gridinfoForJson = new int[100];

    //0は「存在しない」


    public int shiptype;
    //0は「存在しない」
}


public class PrefsManager
{ 
    Formation formation = new Formation();
    FormationForJson formationForJson = new FormationForJson();


    public Formation GetFormation()
    {

        string json = PlayerPrefs.GetString("formation","NoData");

        if(json == "NoData")
        {
            formation.formationDataExists = false;
            return formation;
        }
        else
        {
            formationForJson = JsonUtility.FromJson<FormationForJson>(json);
            formation.formationDataExists = true;
            formation.shiptype = formationForJson.shiptype;
            for (int i = 0; i < 10; i++)
            {
                for (int j = 0; j < 10; j++)
                {
                    formation.gridinfo[i, j] = formationForJson.gridinfoForJson[i * 10 + j];
                }
            }

            return formation;
        }
    }


    public bool SetFormation(int[,] gridinfo,int shiptype)
    {
        formation.formationDataExists = true;
        formation.gridinfo = gridinfo;
        formation.shiptype = shiptype;


        formationForJson.formationDataExists = formation.formationDataExists;
        formationForJson.shiptype = formation.shiptype;

        for (int i = 0; i < 10; i++)
        {
            for (int j = 0; j < 10; j++)
            {
                formationForJson.gridinfoForJson[i * 10 + j] = formation.gridinfo[i, j];
            }
        }

        string json = JsonUtility.ToJson(formationForJson);

        //Debug.Log("json:"+json);

        PlayerPrefs.SetString("formation",json);

        return true;
    }


    public void Delete()
    {
        PlayerPrefs.DeleteAll();

        return;
    }


}
