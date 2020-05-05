using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MasterDataScript : MonoBehaviour
{
    public List<UnitData> unitDataList;
    public List<ShipData> shipDataList;
    public UnitData FindUnitData(int unitID)
    {

        foreach (UnitData data in unitDataList)
        {
            if (data.ID == unitID) return data;
        }
        Debug.Log("IDが" + unitID + "であるUnitは存在しません");
        return null;
    }

    public UnitData FindUnitData(string unitName)
    {
        foreach (UnitData data in unitDataList)
        {
            if (data.Name == unitName) return data;
        }
        Debug.Log("名前が" + unitName + "であるUnitは存在しません");
        return null;
    }

    public ShipData FindShipData(int unitID)
    {

        foreach (ShipData shipData in shipDataList)
        {
            if (shipData.unitData.ID == unitID) return shipData;
        }
        Debug.Log("IDが" + unitID + "であるShipは存在しません");
        return null;
    }
}
