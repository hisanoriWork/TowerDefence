using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

/*[Serializable]*/


public class Formation
{
    public int[] gridinfo = new int[120];
    public int shiptype;
}


public class PrefsManager
{ 
    Formation formation = new Formation();


    public Formation getFormation()
    {

        string json = PlayerPrefs.GetString("formation","oyo");
        Formation formation = JsonUtility.FromJson<Formation>(json);

        return formation;
    }


    public void setFormation(int[] gridinfo,int shiptype)
    {
        formation.gridinfo = gridinfo;
        formation.shiptype = shiptype;

        string json = JsonUtility.ToJson(formation);

        Debug.Log("json:"+json);

        PlayerPrefs.SetString("formation",json);

        return;
    }


    public void delete()
    {
        PlayerPrefs.DeleteAll();

        return;
    }


}
