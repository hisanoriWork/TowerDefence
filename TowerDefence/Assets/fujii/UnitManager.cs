using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnitManager : MonoBehaviour
{
    public struct obj_script
    {
        public GameObject obj;
        public UnitScript script;
    }
    public MasterDataScript masterDataScript;
    public List<obj_script> Obj_ScriptList;

    public void Add(int UnitID,Vector3 pos)
    {
        UnitData data = masterDataScript.FindUnitData(UnitID);
        if (data != null)
        {
            GameObject obj = Instantiate(data.Prefab);
            obj.transform.position = pos;
            obj_script temp;
            temp.obj = obj;
            temp.script = obj.GetComponent<UnitScript>();
            Obj_ScriptList.Add(temp);
        }
    }

    public void Remove(GameObject obj)
    {

    }

    public void Invert()
    {
    }
}
