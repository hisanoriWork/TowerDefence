using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
public class GameManager : MonoBehaviour
{
    /*****public class*****/
    public class UnitManager
    {
        /*****public class*****/
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
        /*****public field*****/
        public MasterDataScript masterData;
        public List<UnitInst> unitInstList;
        public List<UnitInst> pngnInstList;
        public ShipInst shipInst;
        public GameObject parentObj;

        public int shipHP { get { return shipInst.unitScript.HP; } }
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
        bool m_invension = false;
        /*****public method*****/
        public void Init(MasterDataScript masterData, GameObject parentObj, bool invension)
        {
            unitInstList = new List<UnitInst>();
            pngnInstList = new List<UnitInst>();
            this.masterData = masterData;
            this.parentObj = parentObj;
            m_invension = invension;
        }
        public void CreateInit(Formation formation)
        {
            int gridX = formation.gridinfo.GetLength(1);
            int gridY = formation.gridinfo.GetLength(0);
            Vector3 pos = Vector3.zero;
            pos.x = parentObj.transform.position.x - (gridX / 2 - 0.5f) * m_dx;
            pos.y = parentObj.transform.position.y - (gridY / 2 - 0.5f) * m_dy;
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
                pos.x = parentObj.transform.position.x - (gridX / 2 - 0.5f) * m_dx;
                pos.y += m_dy;
            }
            shipInst = CreateShip(formation.shiptype, parentObj.transform.position);
        }
        public void Invert(bool b)
        {
            m_invension = b;
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

                obj.transform.position = pos + ((m_invension) ? Vector3.Scale(data.offSet, new Vector3(-1, 1, 1)) : data.offSet);
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
    public class TextManager<TInfo>
    {
        public Text text{ set{ m_text = value; } }
        virtual public TInfo info{ get{ return m_info; } set { info = value; m_text.text = info.ToString(); } }

        protected Text m_text;
        protected TInfo m_info;
    }
    public class TextManager<TInfo, TCast> :TextManager<TInfo>
    {
        override public TInfo info { get { return m_info; } set { info = value; m_text.text = Convert.ChangeType(info, typeof(TCast)).ToString(); } }
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
        m_player1HP.text = HP1Bar;
        m_player2HP = new TextManager<int>();
        m_player2HP.text = HP2Bar;
        m_timeLimit = new TextManager<float, int>();
        m_timeLimit.text = timeText;


        //PrefsManager prefs = new PrefsManager();
        //Formation formation = prefs.getFormation();
        Formation formation = new Formation();
        formation.formationDataExists = true;
        formation.gridinfo = new int[10, 10]
        {
            {0,0,0,0,0,0,10,10,10,10},
            {0,0,0,10,10,0,0,0,0,0},
            {10,10,10,0,0,0,0,0,0,0},
            {0,0,0,0,0,0,0,0,0,0},
            {0,0,0,0,0,0,0,0,0,0},
            {0,0,0,0,0,0,0,0,0,0},
            {0,0,0,0,0,0,0,0,0,0},
            {0,0,0,0,0,0,0,0,0,0},
            {0,0,0,0,0,0,0,0,0,0},
            {0,0,0,0,0,0,0,0,0,0},
        };
        formation.shiptype = 10010;

        m_player1UnitMgr.CreateInit(formation);
        m_player2UnitMgr.CreateInit(formation);
    }
    void Start()
    {
        m_player1HP.info = m_player1UnitMgr.shipHP;
        m_player2HP.info = m_player2UnitMgr.shipHP;
        m_pngnNum1 = m_player1UnitMgr.pngnNum;
        m_pngnNum2 = m_player2UnitMgr.pngnNum;
        m_timeLimit.info = 10;
    }
    void Update()
    {
        m_player1HP.info = m_player1UnitMgr.shipHP;
        m_player2HP.info = m_player2UnitMgr.shipHP;
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
        if (m_timeLimit.info > 0)
        {
            m_timeLimit.info += - Time.fixedDeltaTime;
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

        if (m_player1HP.info == 0 && m_player2HP.info == 0) return 3;
        else if (m_player1HP.info == 0) return 2;
        else if (m_player2HP.info == 0) return 1;

        if (m_timeLimit.info < 0)
        {
            if (m_player1HP.info == m_player2HP.info) return 3;
            else if (m_player1HP.info < m_player2HP.info) return 2;
            else return 1;
        }
        return 0;
    }
}


