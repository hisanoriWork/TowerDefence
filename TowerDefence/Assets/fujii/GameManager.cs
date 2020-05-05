using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
public class GameManager : MonoBehaviour
{
    public class UnitManager
    {
        /*****public field*****/
        public class UnitInst
        {
            public GameObject obj;
            public UnitScript script;
        }
        public class ShipInst
        {
            public GameObject obj;
            public UnitScript unitScript;
            public ShipScript shipScript;
        }

        public MasterDataScript masterData;
        public List<UnitInst> unitInstList;
        public List<UnitInst> pngnInstList;
        public ShipInst shipInst;
        public GameObject parentObj;
        public bool invension = true;

        public int shipHP {get { return shipInst.unitScript.HP;} }
        public int pngnNum
        {
            get
            {
                int num = 0;
                foreach (var i in pngnInstList)
                {
                    if (i.obj.activeSelf) num++;
                }
                return num;
            }
        }

        /*****private field*****/
        float m_dx = 0.75f, m_dy = 0.80f;
        int m_gridX = 10, m_gridY = 10;

        /*****public method*****/
        public void Init(MasterDataScript masterData, GameObject parentObj, bool invension)
        {
            unitInstList = new List<UnitInst>();
            pngnInstList = new List<UnitInst>();
            this.masterData = masterData;
            this.parentObj = parentObj;
            this.invension = invension;
        }
        public void SetFormation(List<int> IDlist, int shipID)
        {
            Vector3 pos = parentObj.transform.position;
            pos.x += -(m_gridX / 2 - 0.5f) * m_dx;
            pos.y += -(m_gridY / 2 - 0.5f) * m_dy;
            UnitInst inst = new UnitInst();
            for (int i = 0; i < m_gridY; i++)
            {
                for (int j = 0; j < m_gridX; j++)
                {
                    inst = CreateUnit(IDlist[i], pos);
                    if (inst != null)
                    {
                        UnitAdd(inst);
                    }
                    pos.x += m_dx;
                }
                pos.x = parentObj.transform.position.x - (m_gridX / 2 - 0.5f) * m_dx;
                pos.y += m_dy;
            }
            shipInst = CreateShip(shipID, parentObj.transform.position);
        }
        public void Invert(bool b)
        {
            invension = b;
            foreach (var i in unitInstList)
            {
                i.script.Invert(b);
            }
            shipInst.unitScript.Invert(b);
        }

        public void Stop()
        {
            Time.timeScale = 0;
            foreach (var i in unitInstList)
            {
                i.script.isPlaying = false;
            }
            shipInst.unitScript.isPlaying = false;
        }

        public void Play()
        {
            Time.timeScale = 1;
            foreach (var i in unitInstList)
            {
                i.script.isPlaying = true;
            }
            shipInst.unitScript.isPlaying = true;
        }

        private UnitInst CreateUnit(int unitID, Vector3 pos)
        {
            UnitData data = masterData.FindUnitData(unitID);
            if (data != null)
            {
                GameObject obj = Instantiate(data.Prefab);
                obj.transform.SetParent(parentObj.transform);
                obj.transform.position = pos;
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
                GameObject obj = Instantiate(data.unitData.Prefab);
                obj.transform.SetParent(parentObj.transform);

                obj.transform.position = pos + ((invension) ? Vector3.Scale(data.offSet, new Vector3(-1, 1, 1)) : data.offSet);
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
            unitInstList.Add(Inst);
            if (Inst.script.data.IsPngn == true)
            {
                pngnInstList.Add(Inst);
            }
        }
        private void Remove(GameObject obj)
        {
            if (obj == null) return;

            for (int i = 0; i < unitInstList.Count(); i++)
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
        private void Clear()
        {
            unitInstList.Clear();
        }

        
    }

    
    public class TextManager<Info>
    {
        protected Text text;
        protected Info info;

        virtual public void SetInfo(Info info)
        {
            this.info = info;
            text.text = info.ToString();
        }

        public Info GetInfo() { return info; }

        public void SetText(Text text)
        {
            this.text = text;
        }
    }
    public class TextManager<Info, CastInfo> :TextManager<Info>
    {
        override public void SetInfo(Info info)
        {
            this.info = info;
            text.text = Convert.ChangeType(info, typeof(CastInfo)).ToString();
        }
    }
    /*****public field*****/
    public MasterDataScript masterData;
    public GameObject player1;
    public GameObject player2;
    public Text HP1Bar, HP2Bar,timeText;
    public GameObject winCanvas, loseCanvas, drawCanvas;
    
    /*****private field*****/
    TextManager<int> m_player1HP , m_player2HP;
    TextManager<float, int> m_timeLimit;
    UnitManager m_player1UnitMgr;
    UnitManager m_player2UnitMgr;
    int m_pngnNum1, m_pngnNum2;
    //bool isFinished = false;

    /*****Mobehabiour method*****/
    void Awake()
    {
        m_player1UnitMgr = new UnitManager();
        m_player1UnitMgr.Init(masterData, player1, false);
        m_player2UnitMgr = new UnitManager();
        m_player2UnitMgr.Init(masterData, player2, true);
        m_player1HP = new TextManager<int>();
        m_player1HP.SetText(HP1Bar);
        m_player2HP = new TextManager<int>();
        m_player2HP.SetText(HP2Bar);
        m_timeLimit = new TextManager<float, int>();
        m_timeLimit.SetText(timeText);


        //PrefsManager prefs = new PrefsManager();
        //Formation formation = prefs.getFormation();
        //Formation formation = prefs.getFormation();
        //上の項目ができたと仮定する
        //Formation formation = new Formation();
        //for (int i = 0; i < 120; i++)
        //{
        //    formation.gridinfo[i] = UnityEngine.Random.Range(10, 11);
        //}
        //formation.shiptype = 114514;
        //List<int> temp = new List<int>();
        //for (int i = 0; i < formation.gridinfo.Length; i++)
        //{
        //    temp.Add(formation.gridinfo[i]);
        //}
        //ここまで

        //m_player1UnitMgr.SetFormation(temp, formation.shiptype);
        //m_player2UnitMgr.SetFormation(temp, formation.shiptype);
        //m_player2UnitMgr.Invert(true);
    }
    void Start()
    {
        m_player1HP.SetInfo(m_player1UnitMgr.shipHP);
        m_player2HP.SetInfo(m_player2UnitMgr.shipHP);
        m_pngnNum1 = m_player1UnitMgr.pngnNum;
        m_pngnNum2 = m_player2UnitMgr.pngnNum;
        m_timeLimit.SetInfo(100);
    }
    void Update()
    {
        m_player1HP.SetInfo(m_player1UnitMgr.shipHP);
        m_player2HP.SetInfo(m_player2UnitMgr.shipHP);
        m_pngnNum1 = m_player1UnitMgr.pngnNum;
        m_pngnNum2 = m_player2UnitMgr.pngnNum;
        int victoryNum = CheckVictory();
        switch(victoryNum)
        {
            case 3:
                drawCanvas.SetActive(true);
                break;
            case 2:
                loseCanvas.SetActive(true);
                break;
            case 1:
                winCanvas.SetActive(true);
                break;
            default:
                break;
        }

    }
    void FixedUpdate()
    {
        if (m_timeLimit.GetInfo() > 0)
        {
            m_timeLimit.SetInfo(m_timeLimit.GetInfo() - Time.fixedDeltaTime);
        }
    }

    /*****public method*****/
    public void Play()
    {
        m_player1UnitMgr.Play();
        m_player2UnitMgr.Play();
    }

    public void Stop()
    {
        m_player1UnitMgr.Stop();
        m_player2UnitMgr.Stop();
    }

    public void TransitionScene(String sceneName)
    {
        SceneManager.LoadScene(sceneName);
    }
    /*****private method*****/
    int CheckVictory()
    {
        /*決まってない：0
         * プレイヤー1の勝利：1
         * プレイヤー2の勝利：2
         * ドロー：3
         */
        if (m_pngnNum1 == 0 && m_pngnNum2 == 0) return 3;
        else if (m_pngnNum1 == 0) return 2;
        else if (m_pngnNum1 == 0) return 1;

        if (m_player1HP.GetInfo() == 0 && m_player2HP.GetInfo() == 0) return 3;
        else if (m_player1HP.GetInfo() == 0) return 2;
        else if (m_player2HP.GetInfo() == 0) return 1;

        if (m_timeLimit.GetInfo() < 0)
        {
            if (m_player1HP.GetInfo() == m_player2HP.GetInfo()) return 3;
            else if (m_player1HP.GetInfo() < m_player2HP.GetInfo()) return 2;
            else return 1;
        }
        return 0;
    }
}


