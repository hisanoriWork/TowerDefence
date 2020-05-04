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
    public int[] gridinfo = new int[100];
    public int shiptype;
}


public class PrefsManager
{ 
    Formation formation = new Formation();


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
            formation.formationDataExists = true;
            formation = JsonUtility.FromJson<Formation>(json);
            return formation;
        }
    }


    public bool SetFormation(int[] gridinfo,int shiptype)
    {
        formation.gridinfo = gridinfo;
        formation.shiptype = shiptype;

        string json = JsonUtility.ToJson(formation);

        Debug.Log("json:"+json);

        PlayerPrefs.SetString("formation",json);

        return true;
    }


    public void Delete()
    {
        PlayerPrefs.DeleteAll();

        return;
    }


}
