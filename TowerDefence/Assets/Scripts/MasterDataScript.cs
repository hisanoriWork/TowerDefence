using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MasterDataScript : MonoBehaviour
{
    public List<UnitData> unitDataList;
    public List<UnitData> pngnDataList;
    public List<UnitData> blockDataList;
    public List<ShipData> shipDataList;

    public UnitData FindUnitData(int unitID)
    {
        foreach (UnitData data in unitDataList)
        {
            if (data.ID == unitID) return data;
        }
        Debug.Log("IDが" + unitID + "であるPngnは存在しません");
        return null;
    }

    public UnitData FindUnitData(string unitName)
    {
        foreach (UnitData data in unitDataList)
        {
            if (data.name == unitName) return data;
        }
        Debug.Log("名前が" + unitName + "であるPngnは存在しません");
        return null;
    }

    public UnitData FindPngnData(int unitID)
    {
        foreach (UnitData data in pngnDataList)
        {
            if (data.ID == unitID) return data;
        }
        Debug.Log("IDが" + unitID + "であるPngnは存在しません");
        return null;
    }

    public UnitData FindPngnData(string unitName)
    {
        foreach (UnitData data in pngnDataList)
        {
            if (data.name == unitName) return data;
        }
        Debug.Log("名前が" + unitName + "であるPngnは存在しません");
        return null;
    }

    
    public UnitData FindBlockData(int unitID)
    {
        foreach (UnitData data in blockDataList)
        {
            if (data.ID == unitID) return data;
        }
        Debug.Log("IDが" + unitID + "であるPngnは存在しません");
        return null;
    }

    public UnitData FindBlockData(string unitName)
    {
        foreach (UnitData data in blockDataList)
        {
            if (data.name == unitName) return data;
        }
        Debug.Log("名前が" + unitName + "であるPngnは存在しません");
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

    public ShipData FindShipData(string unitName)
    {
        foreach (ShipData shipData in shipDataList)
        {
            if (shipData.unitData.name == unitName) return shipData;
        }
        Debug.Log("名前が" + unitName + "であるPngnは存在しません");
        return null;
    }
}
