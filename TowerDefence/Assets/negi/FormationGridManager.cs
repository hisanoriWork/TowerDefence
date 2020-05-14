using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EachGrid
{
    public Image img;
    public Text txt;
    public SelectableUnit sel;

    public void UpdateEachGrid(Sprite sprite, string text,int unitID,UnitType unitType,GridForm[]form,Vector2 offset)
    {
        this.sel = this.img.GetComponent<SelectableUnit>();

        this.img.sprite = sprite;
        this.txt.text = text;
        this.sel.selectableUnitID = unitID;
        this.sel.selectableUnitType = unitType;
        this.sel.selectableUnitForm = form;
        this.sel.selectableUnitOffset = offset;
        
    }
}

public class FormationChanger
{
    //編成情報
    public Formation formation;

    //移動体
    public GameObject movingUnit;
    public Vector2 movingUnitCoordinate;
    public GridForm[] movingUnitForm;

    //左側グリッド
    public EachGrid[,] eachGrids;
    public Vector2[,] eachGridsCoordinate;  

    //左側グリッドのタイリング状況
    public int[,] eachGridsTiling;
    public int[,] enableAttachingGrids;

    SelectableUnit[,] a;

    public bool[,] isEnableToSetting;

    RectTransform contentRectTransform;

    //public List<>

    /*****public method*****/
    public FormationChanger(Formation formation, EachGrid[,] eachGrids, GameObject movingUnit,RectTransform contentRectTransform)
    {
        this.formation = formation;
        this.movingUnit = movingUnit;
        //this.movingUnitCoordinate = movingUnit.transform.position;
        this.movingUnitCoordinate = (Vector2)movingUnit.transform.position - movingUnit.GetComponent<MovingUnit>().movingUnitOffset;

        this.eachGrids = eachGrids;
        this.eachGridsCoordinate = new Vector2[10, 10];
        for (int i = 0; i < 10; i++)
        {
            for (int j = 0; j < 10; j++)
            {
                this.eachGridsCoordinate[i, j] = eachGrids[i, j].img.transform.position;
                //Debug.Log(this.eachGridsCoordinate[i, j]);
                //Debug.Log(eachGrids[i, j].img.transform.position);
            }
        }
        this.isEnableToSetting = new bool[10, 10];
        this.contentRectTransform = contentRectTransform;
    }

    public void UpdateCoordinates(){
        //movingUnitCoordinate = movingUnit.transform.position;
        this.movingUnitCoordinate = (Vector2)movingUnit.transform.position - movingUnit.GetComponent<MovingUnit>().movingUnitOffset;
        for (int i = 0; i < 10; i++)
        {
            for (int j = 0; j < 10; j++)
            {
                eachGridsCoordinate[i, j] = eachGrids[i, j].img.transform.position;
                //Debug.Log("update:"+eachGridsCoordinate[i, j]);
                
            }
        }

        return;
    }

    public int[] SearchNearSquare(int[] beforeAttachingUnitPosition)
    {
        float minDistance = float.MaxValue;
        float tmpDistance;
        int[] minUnitPosition = new int[2];

        UpdateCoordinates();

        // クリックしたスクリーン座標
        var screenPoint = Input.mousePosition;

        Camera camera = null;


        Vector2 movingUnitCoordinateRect;

        RectTransformUtility.ScreenPointToLocalPointInRectangle(contentRectTransform, movingUnitCoordinate, camera,out movingUnitCoordinateRect);



        //Debug.Log(contentRectTransform.rect);
        //Debug.Log(movingUnitCoordinateRect);


        if (contentRectTransform.rect.Contains(movingUnitCoordinateRect))
        {
            
            //Debug.Log("oyoyoyoyo");
            for (int i = 0; i < 10; i++)
            {
                for (int j = 0; j < 10; j++)
                {
                    tmpDistance = Vector2.Distance(movingUnitCoordinate, eachGridsCoordinate[i, j]);
                    //Debug.Log(i +","+ j + ",distance:" + tmpDistance+"coordinate"+fc.eachGridsCoordinate[i,j]);
                    if (tmpDistance < minDistance)
                    {
                        minUnitPosition[0] = i;
                        minUnitPosition[1] = j;
                        minDistance = tmpDistance;
                    }
                }
            }



            //置けるかの判定をする




            //Debug.Log(minUnitPosition[0] + ""+minUnitPosition[1]) ;

            return minUnitPosition;
        }else if(beforeAttachingUnitPosition != null)
        {
            return beforeAttachingUnitPosition;
        }

        return null;
    }

}

public class FormationGridManager : MonoBehaviour
{
    /*****public field*****/
    public MasterDataScript masterData;
    public EditParam editParam;


    //編成データ読み書き用
    PrefsManager prefs = new PrefsManager();
    //データ
    private Formation formation = new Formation();//マス目部分int[] gridinfo = new int[120] ,船部分 int shiptype;

    public EachGrid[,] eachGrids = new EachGrid[10, 10];

    FormationChanger fc;

    public RectTransform contentRectTransform;
    public Image eachGridImage;

    public GameObject movingUnit;
    public GameObject attachingShip;

    public GridLayoutGroup gridLayoutGroup;

    /*****private field*****/
    private UnitData m_unitData;

    private void Start()
    {

        //ここでとりあえずPrefsManagerからformationを取得
        //formation = prefs.GetFormation();

        //以下デバッグ用
        formation.formationDataExists = true;
        formation.gridinfo = new int[10, 10]
        {
            {  0,  0,  0,  0,  0,  0,  0,  0,  0,  0},
            {  0,  0,  0,  0,  0, 10,100,100, 10, 10},
            {  0,  0,  0,100, 10,  0,100,100,  0,  0},
            { 12,  0,100,100,  0,  0,100,  0,  0,  0},
            {  0,  0,  0, 10,  0,  0,  0,  0,  0,  0},
            {  0,  0,  0,  0,  0,  0,  0,  0,  0,  0},
            {  0,  0,  0,  0,  0,  0,  0,  0,  0,  0},
            {  0,  0,  0,  0,  0,  0,  0,  0,  0,  0},
            {  0,  0,  0,  0,  0,  0,  0,  0,  0,  0},
            {  0,  0,  0,  0,  0,  0,  0,  0,  0,  0},
        };
        formation.shiptype = 10010;

        prefs.SetFormation(formation.gridinfo, formation.shiptype);

        //グリッドを生成、初期化
        //GenerateGridInfo();

        InstanceGridInfo();
        
        gridLayoutGroup.CalculateLayoutInputHorizontal();
        gridLayoutGroup.CalculateLayoutInputVertical();
        gridLayoutGroup.SetLayoutHorizontal();
        gridLayoutGroup.SetLayoutVertical();
        gridLayoutGroup.enabled = false;

        UpdateGridInfo();

        fc = new FormationChanger(formation, eachGrids, movingUnit, contentRectTransform);

        //masterData.FindUnitData(10).

        return;
    }

    private void Update()
    {
        //gridLayoutGroup.enabled = false;
    }


    private void SetUnitOffsetAndResizeImgSize(Image img, float imgSize, Vector2 selectableUnitOffset)
    { 

        //var img = transform.GetComponent<Image>();
        var t = img.GetComponent<RectTransform>();

        img.SetNativeSize();

        var width = t.sizeDelta.x * imgSize;
        var height = t.sizeDelta.y * imgSize;

        t.sizeDelta = new Vector2(width, height);

        img.transform.position = (Vector2)img.transform.position + selectableUnitOffset;

        return;
    }

    private void ResetUnitOffsetAndResizeImgSize(Image img, float imgSize, Vector2 selectableUnitOffset)
    {

        //var img = transform.GetComponent<Image>();
        var t = img.GetComponent<RectTransform>();

        img.SetNativeSize();

        var width = t.sizeDelta.x * imgSize;
        var height = t.sizeDelta.y * imgSize;

        t.sizeDelta = new Vector2(width, height);

        img.transform.position = (Vector2)img.transform.position - selectableUnitOffset;

        return;
    }

    private Vector2 ConvertOffsetValue(Vector2 dataOffset)
    {
        return dataOffset * editParam.movingUnitOffset;
    }


    public void InstanceGridInfo()
    {
        for (int i = 0; i < 10; i++)
        {
            for (int j = 0; j < 10; j++)
            {
                Image img = Instantiate(eachGridImage, contentRectTransform);

                EachGrid eachGrid = new EachGrid();
                eachGrid.img = img.GetComponent<Image>();
                eachGrid.txt = img.transform.Find("Text").GetComponent<Text>();
                eachGrid.txt.text = "0";

                eachGrids[i, j] = eachGrid;
                //Debug.Log("start:"+eachGrids[i, j].img.transform.position);
            }
        }
        return;
    }


    public void GenerateGridInfo()
    {
        for (int i = 0; i < 10; i++)
        {
            for (int j = 0; j < 10; j++)
            {
                Image img = Instantiate(eachGridImage, contentRectTransform);

                m_unitData = masterData.FindUnitData(formation.gridinfo[i, j]);

                EachGrid eachGrid = new EachGrid();
                eachGrid.img = img.GetComponent<Image>();
                eachGrid.txt = img.transform.Find("Text").GetComponent<Text>();


                if (m_unitData != null) {
                    SelectableUnit sel = eachGrid.img.GetComponent<SelectableUnit>();
                    eachGrid.img.sprite = m_unitData.sprite;
                    sel.selectableUnitID = m_unitData.ID;
                    sel.selectableUnitType = m_unitData.unitType;
                    sel.selectableUnitForm = m_unitData.form;

                    ConvertOffsetValue(m_unitData.offset);
                    sel.selectableUnitOffset = m_unitData.offset;
                }
                eachGrid.txt.text = formation.gridinfo[i, j].ToString();

                eachGrids[i, j] = eachGrid;
                //Debug.Log("start:"+eachGrids[i, j].img.transform.position);
            }
        }
        return;
    }

    public void UpdateGridInfo()
    {
        for (int i = 0; i < 10; i++)
        {
            for (int j = 0; j < 10; j++)
            {
                //Image img = Instantiate(eachGridImage, contentRectTransform);
                Image img = eachGrids[i, j].img;

                m_unitData = masterData.FindUnitData(formation.gridinfo[i, j]);

                EachGrid eachGrid = new EachGrid();
                eachGrid.img = img.GetComponent<Image>();
                eachGrid.txt = img.transform.Find("Text").GetComponent<Text>();


                if (m_unitData != null)
                {
                    SelectableUnit sel = eachGrid.img.GetComponent<SelectableUnit>();
                    eachGrid.img.sprite = m_unitData.sprite;
                    sel.selectableUnitID = m_unitData.ID;
                    sel.selectableUnitType = m_unitData.unitType;
                    sel.selectableUnitForm = m_unitData.form;

                    sel.selectableUnitOffset = ConvertOffsetValue(m_unitData.offset);

                    SetUnitOffsetAndResizeImgSize(eachGrid.img, editParam.attachingUnitImgSize, sel.selectableUnitOffset);
                }
                else
                {
                    SetUnitOffsetAndResizeImgSize(eachGrid.img, editParam.attachingUnitImgSize, Vector2.zero);
                }
                

                eachGrid.txt.text = formation.gridinfo[i, j].ToString();

                eachGrids[i, j] = eachGrid;
                //Debug.Log("start:"+eachGrids[i, j].img.transform.position);
            }
        }
        return;
    }

    public void ResetGridInfo()
    {
        foreach (RectTransform child in contentRectTransform)
        {
            //削除する
            Destroy(child.gameObject);
        }

        //以下デバッグ用
        formation.formationDataExists = true;
        formation.gridinfo = new int[10, 10]
        {
            {  0,  0,  0,  0,  0,  0,  0,  0,  0,  0},
            {  0,  0,  0,  0,  0, 10,100,100, 10, 10},
            {  0,  0,  0,100, 10,  0,100,100,  0,  0},
            { 12,  0,100,100,  0,  0,100,  0,  0,  0},
            {  0,  0,  0, 10,  0,  0,  0,  0,  0,  0},
            {  0,  0,  0,  0,  0,  0,  0,  0,  0,  0},
            {  0,  0,  0,  0,  0,  0,  0,  0,  0,  0},
            {  0,  0,  0,  0,  0,  0,  0,  0,  0,  0},
            {  0,  0,  0,  0,  0,  0,  0,  0,  0,  0},
            {  0,  0,  0,  0,  0,  0,  0,  0,  0,  0},
        };
        formation.shiptype = 10010;

        GenerateGridInfo();
        
        return;
    }


    public void Attach(int movingUnitID, UnitType movinUnitType,GridForm[] movingUnitForm,Vector2 movingUnitOffset, int[] beforeAttachingUnitPosition)
    {

        int[] attachingUnitPosition = fc.SearchNearSquare(beforeAttachingUnitPosition);

        m_unitData = masterData.FindUnitData(movingUnitID);

        if (attachingUnitPosition != null)
        {
            //Debug.Log("attach"+movingUnitID);
            fc.formation.gridinfo[attachingUnitPosition[0], attachingUnitPosition[1]] = movingUnitID;
            if (m_unitData != null)
            {
                EachGrid eachGrid = fc.eachGrids[attachingUnitPosition[0], attachingUnitPosition[1]];
                eachGrid.UpdateEachGrid(m_unitData.sprite, movingUnitID.ToString(), m_unitData.ID, m_unitData.unitType,m_unitData.form, ConvertOffsetValue(m_unitData.offset));
                SetUnitOffsetAndResizeImgSize(eachGrid.img,editParam.attachingUnitImgSize,movingUnitOffset);
            }

        }

        //Regenerate();

        return;
    }








    public void testtesttest()
    {
        for (int i = 0; i < 10; i++)
        {
            for (int j = 0; j < 10; j++)
            {
                //eachGrids[i, j].txt.text = "aa";
                Debug.Log(eachGrids[i, j].img.transform.position);
            }
        }

        eachGrids[6, 5].txt.text = "aa";
        return;
    }


}
