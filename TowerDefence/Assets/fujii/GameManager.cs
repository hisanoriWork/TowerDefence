using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class GameManager : MonoBehaviour
{
    
    public MasterDataScript masterData;
    public GameObject Player1;
    public GameObject Player2;
    UnitManager Player1UnitMgr;
    UnitManager Player2UnitMgr;
    
    //PrefsManager prefs;

    void Awake()
    {
        Player1UnitMgr = new UnitManager();
        Player1UnitMgr.Init(masterData,Player1);
        Player2UnitMgr = new UnitManager();
        Player2UnitMgr.Init(masterData,Player2);
        //prefs = new PrefsManager();
    }

    void Start()
    {

        //Formation formation = prefs.getFormation();
        //上の項目ができたと仮定する
        //Formation formation = new Formation();
        //for (int i = 0; i < 120; i++)
        //{
        //    formation.gridinfo[i] = Random.Range(10,11);
        //}
        //formation.shiptype = 114514;


        //List<int> temp = new List<int>();
        //for (int i = 0; i < formation.gridinfo.Length; i++)
        //{
        //    temp.Add(formation.gridinfo[i]);
        //}
        
        //Player1UnitMgr.SetFormation(temp, formation.shiptype);
        //Player2UnitMgr.SetFormation(temp, formation.shiptype);
        //Player2UnitMgr.Invert();
    }

    public class UnitManager
    {
        public class Obj_Script
        {
            public GameObject obj;
            public UnitScript script;
        }
        public MasterDataScript masterData;
        public List<Obj_Script> unitInstList;
        public List<Obj_Script> pngnInstList;
        public Obj_Script shipInst;
        public GameObject parentObj;
        public int PngnNum = 0;
        float dx = 0.75f, dy = 0.75f;
        int gridX = 12, gridY = 10;
        public void Init(MasterDataScript masterData,GameObject parentObj)
        {
            unitInstList = new List<Obj_Script>();
            pngnInstList = new List<Obj_Script>();
            this.masterData = masterData;
            this.parentObj = parentObj;
        }
        public void SetFormation(List<int> IDlist,int shipID)
        {
            Vector3 pos = parentObj.transform.position;
            pos.x += -(gridX / 2 - 0.5f) * dx;
            pos.y += -(gridY / 2 - 0.5f) * dy;
            Obj_Script inst = new Obj_Script();
            for (int i = 0; i < gridY; i++)
            {
                for (int j = 0;j <gridX; j++)
                {
                    inst = Create(IDlist[i], pos);
                    if (inst != null)
                    {
                        Add(inst);
                    }
                    pos.x += dx;
                }
                pos.x = parentObj.transform.position.x -(gridX / 2 - 0.5f) * dx;
                pos.y += dy;
            }

            inst = Create(shipID, parentObj.transform.position);
            shipInst = inst;
            Debug.Log(unitInstList.Count());
            PngnNum = unitInstList.Count();
        }
        public Obj_Script Create(int unitID, Vector3 pos)
        {
            UnitData data = masterData.FindUnitData(unitID);
            if (data != null)
            {
                GameObject obj = Instantiate(data.Prefab);
                obj.transform.SetParent(parentObj.transform);
                obj.transform.position = pos;
                Obj_Script inst = new Obj_Script();
                inst.obj = obj;
                inst.script = obj.GetComponent<UnitScript>();
                return inst;
            }
            return null;
        }
        public void Add(Obj_Script Inst)
        {
            unitInstList.Add(Inst);
            if(Inst.script.Data.IsPngn == true)
            {
                pngnInstList.Add(Inst);
            }
        }
        public  void Remove(GameObject obj)
        {
            if (obj == null) return;

            for(int i = 0;i< unitInstList.Count(); i++)
            {
                if (unitInstList[i].obj.GetInstanceID() == obj.GetInstanceID())
                {
                    unitInstList.RemoveAt(i);
                    return;
                }
            }
            Debug.Log(obj + "はlistに存在しません");
            return;
        }

        public void Clear()
        {
            unitInstList.Clear();
        }
        public void Invert()
        {
            foreach (var i in unitInstList)
            {
                i.script.Invert(true);
            }
            shipInst.script.Invert(true);
        }

    }

}


