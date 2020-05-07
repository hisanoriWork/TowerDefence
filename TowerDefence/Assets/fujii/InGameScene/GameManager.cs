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
    public class TextManager<TInfo>
    {
        protected Text m_text;
        protected TInfo m_info;

        public TextManager(Text text)
        {
            m_text = text;
        }
        virtual public TInfo info {
            get { return m_info; }
            set
            {
                m_info = value;
                if (m_text != null) m_text.text = info.ToString();
            }
        }

        
    }
    public class TextManager<TInfo, TCast> :TextManager<TInfo>
    {
        public TextManager(Text text)
            : base(text) { }
        override public TInfo info {
            get { return m_info; }
            set
            {
                m_info = value;
                if(m_text != null) m_text.text = Convert.ChangeType(info, typeof(TCast)).ToString();
            }
        }
    }
    //public class GaugeManager<Tinfo>
    //{
    //    protected Image m_image;
    //    protected Tinfo m_maxInfo;
    //    protected Tinfo m_Info;
    //    public GaugeManager(Image fillImage)
    //    {
    //        m_fillImage = fillImage;
    //        m_emptyImage = emptyImage;
    //    }

    //}
    public class Player
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
        public GameObject parentObj;
        public MasterDataScript masterData;
        public List<UnitInst> unitInstList;
        public List<UnitInst> pngnInstList;
        public ShipInst shipInst;
        public int layerNum;
        public int shipHP{get{return shipInst.unitScript.HP;}}
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
        bool m_isInverted = false;
        
        /*****public method*****/
        public Player(MasterDataScript masterData, GameObject parentObj,Formation formation)
        {
            unitInstList = new List<UnitInst>();
            pngnInstList = new List<UnitInst>();
            shipInst = new ShipInst();
            this.masterData = masterData;
            this.parentObj = parentObj;
            CreateInst(formation);
        }
        public void Invert(bool b)
        {
            m_isInverted = b;
            foreach (var i in unitInstList)
            {
                i.script.isInverted = true;
            }
            shipInst.unitScript.isInverted = true;
            Vector3 size = parentObj.transform.localScale;
            size.x *= (b ^ size.x<0f) ? -1 : 1;
            parentObj.transform.localScale = size;
        }
        public void ChangeLayer(string pngnLayerName,string shipLayerName)
        {
            int pngnLayerNum = LayerMask.NameToLayer(pngnLayerName);
            int shipLayerNum = LayerMask.NameToLayer(shipLayerName);
            foreach (var unitInst in unitInstList)
            {
                if (unitInst.script.data.unitType ==UnitType.Pngn) SetLayerRecursively(unitInst.obj , pngnLayerNum);
                else unitInst.obj.layer = shipLayerNum;
            }
            SetLayerRecursively(shipInst.obj, shipLayerNum);
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
        /*****private method*****/
        private void CreateInst(Formation formation)
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
        private UnitInst CreateUnit(int unitID, Vector3 pos)
        {
            UnitData data = masterData.FindUnitData(unitID);
            if (data != null)
            {
                GameObject obj = Instantiate(data.prefab);
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
                GameObject obj = Instantiate(data.unitData.prefab);
                obj.transform.SetParent(parentObj.transform);

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
            unitInstList.Add(Inst);
            if (Inst.script.data.unitType == UnitType.Pngn)
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
        private void SetLayerRecursively(GameObject self,int layer)
        {
            self.layer = layer;

            foreach (Transform n in self.transform)
            {
                SetLayerRecursively(n.gameObject, layer);
            }
        }
    }
    /*****public field*****/
    public MasterDataScript masterData;
    public GameObject player1Place;
    public GameObject player2Place;
    public Text HP1Bar, HP2Bar,timeText;
    public GameObject winCanvas, loseCanvas, drawCanvas;
    /*****private field*****/
    private TextManager<int> m_player1HP , m_player2HP;
    private TextManager<float, int> m_timeLimit;
    private Player m_player1;
    private Player m_player2;
    private int[,] gird;
    public bool isPlaying { get; set; } = true;
    //bool isFinished = false;
    /*****Mobehabiour method*****/
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
            {0,0,0,0,0,0,0,0,0,0},
            {0,0,0,0,0,11,10,10,10,10},
            {0,0,0,11,10,0,0,0,0,0},
            {11,10,10,0,0,0,0,0,0,0},
            {0,0,0,0,0,0,0,0,0,0},
            {0,0,0,0,0,0,0,0,0,0},
            {0,0,0,0,0,0,0,0,0,0},
            {0,0,0,0,0,0,0,0,0,0},
            {0,0,0,0,0,0,0,0,0,0},
            {0,0,0,0,0,0,0,0,0,0},
        };
        formation.shiptype = 10010;

        m_player1 = new Player(masterData,player1Place,formation);
        m_player2 = new Player(masterData,player2Place,formation);
        m_player1HP = new TextManager<int>(HP1Bar);
        m_player2HP = new TextManager<int>(HP2Bar);
        m_timeLimit = new TextManager<float, int>(timeText);

        RegardAsFriend(m_player1);
        RegardAsOpponent(m_player2);
    }
    void Start()
    {
        m_player1HP.info = m_player1.shipHP;
        m_player2HP.info = m_player2.shipHP;
        m_timeLimit.info = 10;
    }
    void Update()
    {
        if (isPlaying)
        {
            m_player1HP.info = m_player1.shipHP;
            m_player2HP.info = m_player2.shipHP;
            int victoryNum = CheckVictory(m_player1.shipHP, m_player2.shipHP, m_player1.pngnNum, m_player2.pngnNum);
            switch (victoryNum)
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
        isPlaying = true;
        m_player1.Play();
        m_player2.Play();
    }
    public void Stop()
    {
        isPlaying = false;
        m_player1.Stop();
        m_player2.Stop();
    }
    public void TransitionScene(String sceneName)
    {
        SceneManager.LoadScene(sceneName);
    }
    /*****private method*****/
    private int CheckVictory(int shipHP1 ,int shipHP2,int pngnNum1,int pngnNum2)
    {
        /*決まってない：0
         * プレイヤー1の勝利：1
         * プレイヤー2の勝利：2
         * ドロー：3
         */
        if (pngnNum1 == 0 && pngnNum2 == 0) return 3;
        else if (pngnNum1 == 0) return 2;
        else if (pngnNum1 == 0) return 1;

        if (shipHP1 == 0 && shipHP2 == 0) return 3;
        else if (shipHP1 == 0) return 2;
        else if (shipHP2 == 0) return 1;

        if (m_timeLimit.info < 0)
        {
            if (shipHP1 == shipHP2) return 3;
            else if (shipHP1 < shipHP2) return 2;
            else return 1;
        }
        return 0;
    }
    private void RegardAsFriend(Player player)
    {
        player.ChangeLayer("Player1", "PlayerShip1");
    }
    private void RegardAsOpponent(Player player)
    {
        player.Invert(true);
        player.ChangeLayer("Player2","PlayerShip2");
    }
}
