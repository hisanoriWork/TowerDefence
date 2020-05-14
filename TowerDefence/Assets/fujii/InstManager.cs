using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class InstManager : MonoBehaviour
{
    /*****public field*****/
    public Transform place;
    public MasterDataScript masterData;
    public InstDataScript instData;
    public string pngnLayer;
    public string shipLayer;
    public string weaponLayer;
    public int shipHP { get { return instData.ship.unitScript.HP; } }
    public int pngnNum
    {
        get
        {
            int num = 0;
            foreach (var i in instData.pngnList)
            {
                if (i.obj.activeSelf) num++;
            }
            return num;
        }
    }
    /*****private field*****/
    float m_dx = 0.8f, m_dy = 0.8f;

    /*****public method*****/
    public void Init(Formation formation)
    {
        instData.unitList = new List<UnitInst>();
        instData.pngnList = new List<UnitInst>();
        instData.ship = new ShipInst();
        CreateInst(formation);
    }
    public void Invert(bool b)
    {
        foreach (var i in instData.unitList)
        {
            i.script.isInverted = true;
        }
        instData.ship.unitScript.isInverted = true;
        Vector3 size = place.localScale;
        size.x *= (b ^ size.x < 0f) ? -1 : 1;
        place.localScale = size;
    }
    public void ChangeLayer()
    {
        int pngnLayerNum = LayerMask.NameToLayer(pngnLayer);
        int shipLayerNum = LayerMask.NameToLayer(shipLayer);
        foreach (var unitInst in instData.unitList)
        {
            if (unitInst.script.data.unitType == UnitType.Pngn) Utility.SetLayerRecursively(unitInst.obj, pngnLayerNum);
            else unitInst.obj.layer = shipLayerNum;
        }
        Utility.SetLayerRecursively(instData.ship.obj, shipLayerNum);
    }
    public void Stop()
    {
        Time.timeScale = 0;
        foreach (var i in instData.unitList)
        {
            i.script.isPlaying = false;
        }
        instData.ship.unitScript.isPlaying = false;
    }
    public void Play()
    {
        Time.timeScale = 1;
        foreach (var i in instData.unitList)
        {
            i.script.isPlaying = true;
        }
        instData.ship.unitScript.isPlaying = true;
    }
    /*****private method*****/
    private void CreateInst(Formation formation)
    {
        int gridX = formation.gridinfo.GetLength(1);
        int gridY = formation.gridinfo.GetLength(0);
        Vector3 pos = Vector3.zero;
        pos.x = place.position.x - (gridX / 2 - 0.5f) * m_dx;
        pos.y = place.position.y - (gridY / 2 - 0.5f) * m_dy;
        UnitInst inst = new UnitInst();
        for (int i = 0; i < gridY; i++)
        {
            for (int j = 0; j < gridX; j++)
            {
                inst = CreateUnit(formation.gridinfo[i, j], pos);
                if (inst != null)
                {
                    UnitAdd(inst);
                }
                pos.x += m_dx;
            }
            pos.x = place.position.x - (gridX / 2 - 0.5f) * m_dx;
            pos.y += m_dy;
        }
        instData.ship = CreateShip(formation.shiptype, place.position);
    }
    private UnitInst CreateUnit(int unitID, Vector3 pos)
    {
        UnitData data = masterData.FindUnitData(unitID);
        if (data != null)
        {
            GameObject obj = Instantiate(data.prefab, pos + data.offset, Quaternion.Euler(Vector3.zero), place.transform);
            UnitInst inst = new UnitInst();
            inst.obj = obj;
            inst.script = obj.GetComponent<UnitScript>();
            return inst;
        }
        return null;
    }
    private ShipInst CreateShip(int shipID, Vector3 pos)
    {
        ShipData data = masterData.FindShipData(shipID);
        if (data != null)
        {
            GameObject obj = Instantiate(data.unitData.prefab);
            obj.transform.SetParent(place.transform);

            obj.transform.position = pos + data.offSet;
            ShipInst inst = new ShipInst();
            inst.obj = obj;
            inst.unitScript = obj.GetComponent<UnitScript>();
            inst.shipScript = obj.GetComponent<ShipScript>();
            return inst;
        }
        return null;
    }
    private void UnitAdd(UnitInst Inst)
    {
        instData.unitList.Add(Inst);
        if (Inst.script.data.unitType == UnitType.Pngn)
        {
            instData.pngnList.Add(Inst);
        }
    }
    private void Remove(GameObject obj)
    {
        if (obj == null) return;

        for (int i = 0; i < instData.unitList.Count(); i++)
        {
            if (instData.unitList[i].obj.GetInstanceID() == obj.GetInstanceID())
            {
                instData.unitList.RemoveAt(i);
                return;
            }
        }
        Debug.Log(obj + "はlistに存在しません");
        return;
    }
    private void Clear()
    {
        instData.unitList.Clear();
    }
}