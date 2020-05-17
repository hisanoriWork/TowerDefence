using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using MyLibrary;

public class InstManager : MonoBehaviour
{
    /*****public field*****/
    public int shipHP { get { return m_instData.ship.unitScript.HP; } }
    public int pngnNum
    {
        get
        {
            int num = 0;
            foreach (var i in m_instData.pngnList)
            {
                if (i.obj.activeSelf) num++;
            }
            return num;
        }
    }
    /*****private field*****/
    [SerializeField] private MasterDataScript m_masterData = default;
    [SerializeField] private InstDataScript m_instData = default;
    float m_dx = 0.8f, m_dy = 0.8f;
    /*****monoBehaviour method*****/
    void Awake()
    {
        //PrefsManager prefs = new PrefsManager();
        //Formation formation = prefs.getFormation();
        //gird = formation.girdinfo;
        //下はデバッグ用
        Formation formation = new Formation();
        formation.formationDataExists = true;
        formation.gridinfo = new int[10, 10]
        {
            {  0,  0,  0,  0,  0,  0,  0,  0,  0,  0},
            {  0,  0,  0,  0,  0, 14,100,100, 11, 10},
            {  0,  0,  0,100, 10,  0,100,100,  0,  0},
            { 12,  0, 10,100,  0,  0,100,  0,  0,  0},
            {  0,  0,  0,100,  0,  0,  0,  0,  0,  0},
            {  0,  0,  0,100,  0,  0,  0,  0,  0,  0},
            {  0,  0,  0,100,  0,  0,  0,  0,  0,  0},
            {  0,  0,  0,100,  0,  0,  0,  0,  0,  0},
            {  0,  0,  0,100,  0,  0,  0,  0,  0,  0},
            {  0,  0,  0, 11,  0,  0,  0,  0,  0,  0},
        };
        formation.shiptype = 10010;
        CreateInst(formation);
        ChangeLayer();
        if (m_instData.playerNum == PlayerNum.Player2)
            Invert(true);
    }
    /*****public method*****/
    public void Invert(bool b)
    {
        foreach (var i in m_instData.unitList)
        {
            i.script.isInverted = true;
        }
        m_instData.ship.unitScript.isInverted = true;
        Vector3 size = m_instData.place.localScale;
        size.x *= (b ^ size.x < 0f) ? -1 : 1;
        m_instData.place.localScale = size;
    }
    /*****private method*****/
    private void ChangeLayer()
    {
        int pngnLayerNum, shipLayerNum;
        if (m_instData.playerNum == PlayerNum.Player1)
        {
            pngnLayerNum = LayerMask.NameToLayer(Constant.PngnLayer1);
            shipLayerNum = LayerMask.NameToLayer(Constant.ShipLayer1);
        }
        else
        {
            pngnLayerNum = LayerMask.NameToLayer(Constant.PngnLayer2);
            shipLayerNum = LayerMask.NameToLayer(Constant.ShipLayer2);
        }
        foreach (var unitInst in m_instData.unitList)
        {
            unitInst.script.playerNum = m_instData.playerNum;
            if (unitInst.script.data.unitType == UnitType.Pngn) Utility.SetLayerRecursively(unitInst.obj, pngnLayerNum);
            else unitInst.obj.layer = shipLayerNum;
        }
        Utility.SetLayerRecursively(m_instData.ship.obj,shipLayerNum);
    }
    private void CreateInst(Formation formation)
    {
        int gridX = formation.gridinfo.GetLength(1);
        int gridY = formation.gridinfo.GetLength(0);
        Vector3 pos = Vector3.zero;
        pos.x = m_instData.place.position.x - (gridX / 2 - 0.5f) * m_dx;
        pos.y = m_instData.place.position.y - (gridY / 2 - 0.5f) * m_dy;
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
            pos.x = m_instData.place.position.x - (gridX / 2 - 0.5f) * m_dx;
            pos.y += m_dy;
        }
        m_instData.ship = CreateShip(formation.shiptype, m_instData.place.position);
    }
    private UnitInst CreateUnit(int unitID, Vector3 pos)
    {
        UnitData data = m_masterData.FindUnitData(unitID);
        if (data != null)
        {
            GameObject obj = Instantiate(data.prefab, pos + data.offset, Quaternion.Euler(Vector3.zero), m_instData.place.transform);
            UnitInst inst = new UnitInst();
            inst.obj = obj;
            inst.script = obj.GetComponent<UnitScript>();
            return inst;
        }
        return null;
    }
    private ShipInst CreateShip(int shipID, Vector3 pos)
    {
        ShipData data = m_masterData.FindShipData(shipID);
        if (data != null)
        {
            GameObject obj = Instantiate(data.unitData.prefab, pos + data.unitData.offset, Quaternion.Euler(Vector3.zero), m_instData.place.transform);
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
        m_instData.unitList.Add(Inst);
        if (Inst.script.data.unitType == UnitType.Pngn)
        {
            m_instData.pngnList.Add(Inst);
        }
    }
    private void Remove(GameObject obj)
    {
        if (obj == null) return;

        for (int i = 0; i < m_instData.unitList.Count(); i++)
        {
            if (m_instData.unitList[i].obj.GetInstanceID() == obj.GetInstanceID())
            {
                m_instData.unitList.RemoveAt(i);
                return;
            }
        }
        Debug.Log(obj + "はlistに存在しません");
        return;
    }
    private void Clear()
    {
        m_instData.unitList.Clear();
    }
}