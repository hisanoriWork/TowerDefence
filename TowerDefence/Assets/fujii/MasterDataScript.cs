using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MasterDataScript : MonoBehaviour
{
    public List<UnitData> UnitDataList;
    public UnitData FindUnitData(int unitID)
    {

        foreach (UnitData data in UnitDataList)
        {
            if (data.ID == unitID) return data;
        }
        Debug.Log("IDが" + unitID + "であるUnitは存在しません");
        return null;
    }

    public UnitData FindUnitData(string unitName)
    {
        foreach (UnitData data in UnitDataList)
        {
            if (data.Name == unitName) return data;
        }
        Debug.Log("名前が" + unitName + "であるUnitは存在しません");
        return null;
    }

}
