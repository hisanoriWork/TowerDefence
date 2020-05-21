using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EachGrid
{
    public Image img;
    public Text txt;
    public SelectableUnit sel;

    public void UpdateEachGrid(Sprite sprite,int unitID,UnitType unitType,GridForm[]form,Vector2 offset,int cost,int[] beforeAttachingPosition)
    {
        this.sel = this.img.GetComponent<SelectableUnit>();

        this.img.sprite = sprite;
        this.txt.text = unitID.ToString();
        this.sel.selectableUnitID = unitID;
        this.sel.selectableUnitType = unitType;
        this.sel.selectableUnitForm = form;
        this.sel.selectableUnitOffset = offset;
        this.sel.selectableUnitCost = cost;
        this.sel.beforeAttachingPosition = beforeAttachingPosition;


    }
}

public enum TilingType
{
    Pngn,
    Object,
    Empty
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
    public TilingType[,] eachGridsTiling;
    public bool[,] enableToAttaching;

    //船
    public EachGrid shipEachGrid;

    RectTransform contentRectTransform;
    RectTransform deleteButtonRectTransform;

    //public List<>


    /*****private method*****/

    private void SetEachGridsTiling(TilingType unitTilingType,int unitY,int unitX,GridForm[] gridForms)
    {
        if (unitTilingType != TilingType.Empty)
        {
            eachGridsTiling[unitY, unitX] = unitTilingType;

            if (gridForms != null)
            {
                foreach (GridForm form in gridForms)
                {
                    eachGridsTiling[unitY + form.y, unitX + form.x] = unitTilingType;
                }
            }
        }

        return;
    }

    private void ResetEachGridsTiling()
    {

        for (int i = 0; i < 10; i++)
        {
            for (int j = 0; j < 10; j++)
            {
                eachGridsTiling[i,j] = TilingType.Empty;
            }
        }

        return;
    }

    public TilingType ConvertTilingType(int unitID, UnitType unitType)
    {
        if (unitID == 0) return TilingType.Empty;
        else if (unitType == UnitType.Pngn) return TilingType.Pngn;
        else if (unitType == UnitType.Block) return TilingType.Object;
        else if (unitType == UnitType.Ship) return TilingType.Object;
        return TilingType.Empty;
    }

    /*****public method*****/
    public FormationChanger(Formation formation, EachGrid[,] eachGrids, GameObject movingUnit,RectTransform contentRectTransform,RectTransform deleteButtonRectTransform, EachGrid shipEachGrid)
    {
        this.formation = formation;
        this.movingUnit = movingUnit;
        //this.movingUnitCoordinate = movingUnit.transform.position;
        this.movingUnitCoordinate = (Vector2)movingUnit.transform.position - movingUnit.GetComponent<MovingUnit>().movingUnitOffset;

        this.eachGrids = eachGrids;
        this.eachGridsCoordinate = new Vector2[10, 10];

        this.eachGridsTiling = new TilingType[10, 10];

        this.shipEachGrid = shipEachGrid;


        SetEachGridsTiling(ConvertTilingType(shipEachGrid.sel.selectableUnitID,shipEachGrid.sel.selectableUnitType),0,0, shipEachGrid.sel.selectableUnitForm);


        for (int i = 0; i < 10; i++)
        {
            for (int j = 0; j < 10; j++)
            {
                this.eachGridsCoordinate[i, j] = eachGrids[i, j].img.transform.position;
                //Debug.Log(this.eachGridsCoordinate[i, j]);
                //Debug.Log(eachGrids[i, j].img.transform.position);
                SetEachGridsTiling(ConvertTilingType(shipEachGrid.sel.selectableUnitID, shipEachGrid.sel.selectableUnitType), i, j, eachGrids[i,j].sel.selectableUnitForm);

            }
        }


        /*for (int i = 0; i < 10; i++)
        {
            for (int j = 0; j < 10; j++)
            {
                Debug.Log(eachGridsTiling[i, j]);
            }
        }*/

        this.enableToAttaching = new bool[10, 10];
        this.contentRectTransform = contentRectTransform;
        this.deleteButtonRectTransform = deleteButtonRectTransform;
    }

    public void UpdateCoordinates(){

        //ResetEachGridsTiling();

        //movingUnitCoordinate = movingUnit.transform.position;
        this.movingUnitCoordinate = (Vector2)movingUnit.transform.position - movingUnit.GetComponent<MovingUnit>().movingUnitOffset;

        //SetEachGridsTiling(ConvertTilingType(shipEachGrid.sel.selectableUnitID, shipEachGrid.sel.selectableUnitType), 0, 0, shipEachGrid.sel.selectableUnitForm);

        for (int i = 0; i < 10; i++)
        {
            for (int j = 0; j < 10; j++)
            {
                eachGridsCoordinate[i, j] = eachGrids[i, j].img.transform.position;
                //Debug.Log("update:"+eachGridsCoordinate[i, j]);
                //SetEachGridsTiling(ConvertTilingType(shipEachGrid.sel.selectableUnitID, shipEachGrid.sel.selectableUnitType), i, j, eachGrids[i, j].sel.selectableUnitForm);
            }
        }

        return;
    }

    public void UpdateEachGridsTiling()
    {
        ResetEachGridsTiling();

        SetEachGridsTiling(ConvertTilingType(shipEachGrid.sel.selectableUnitID, shipEachGrid.sel.selectableUnitType), 0, 0, shipEachGrid.sel.selectableUnitForm);

        for (int i = 0; i < 10; i++)
        {
            for (int j = 0; j < 10; j++)
            {
                SetEachGridsTiling(ConvertTilingType(eachGrids[i, j].sel.selectableUnitID, eachGrids[i, j].sel.selectableUnitType), i, j, eachGrids[i, j].sel.selectableUnitForm);
            }
        }
    }

    public int[] SearchNearSquare(int[] beforeAttachingUnitPosition, int unitID, UnitType unitType, GridForm[] unitForm)
    {
        float minDistance = float.MaxValue;
        float tmpDistance;
        int[] minUnitPosition = new int[2];

        UpdateCoordinates();

        // クリックしたスクリーン座標
        var screenPoint = Input.mousePosition;

        Camera camera = null;


        Vector2 movingUnitCoordinateRect;
        Vector2 movingUnitCoordinateRectForDeleteButton;


        RectTransformUtility.ScreenPointToLocalPointInRectangle(contentRectTransform, movingUnitCoordinate, camera,out movingUnitCoordinateRect);
        RectTransformUtility.ScreenPointToLocalPointInRectangle(deleteButtonRectTransform, movingUnitCoordinate, camera, out movingUnitCoordinateRectForDeleteButton);


        //Debug.Log(contentRectTransform.rect);
        //Debug.Log(movingUnitCoordinateRect);

        //Debug.Log("a"+deleteButtonRectTransform.rect);
        //Debug.Log("a"+movingUnitCoordinateRectForDeleteButton);

        if (deleteButtonRectTransform.rect.Contains(movingUnitCoordinateRectForDeleteButton))
        {
            //Debug.Log("OOYOYOYOYO");
            return null;
        }


        if (contentRectTransform.rect.Contains(movingUnitCoordinateRect))
        {
            
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
            if(!CheckingEnableToAttaching(unitID, unitType, unitForm, minUnitPosition))return beforeAttachingUnitPosition;



            //Debug.Log(minUnitPosition[0] + ""+minUnitPosition[1]) ;

            return minUnitPosition;
        }

        return beforeAttachingUnitPosition;
    }

    public bool CheckingEnableToAttaching(int unitID, UnitType unitType, GridForm[] unitForm ,int[] minUnitPosition)
    {
        bool canAttaching;
        TilingType unitTylingType = ConvertTilingType(unitID, unitType);

        canAttaching = true;

        //見ること
        //それぞれのフォームごとに
        //枠からはみ出していない
        //enableToAttachingと重なっている

        //クリアしているならば、trueを返す

        checking(minUnitPosition[0], minUnitPosition[1]);


        foreach (GridForm attachingEachGridForm in unitForm)
        {
            checking(minUnitPosition[0] + attachingEachGridForm.y, minUnitPosition[1] + attachingEachGridForm.x);
        }

        return canAttaching;


        void checking(int y, int x)
        {
            if (y >= 0 && y < 10 && x >= 0 && x < 10)
            {
                if (enableToAttaching[y, x] == false) canAttaching = false;

            }
            else canAttaching = false;
        }
    }



    public void UpdateEnableToAttaching(int unitID, UnitType unitType, GridForm[] unitForm, Vector2 unitOffset)
    {
        enableToAttaching = new bool[10, 10];
        bool canGetOnInEachSituation;
        bool canAttaching;
        TilingType unitTylingType = ConvertTilingType(unitID, unitType);

        for (int y = 0; y < 10; y++)
        {
            for (int x = 0; x < 10; x++)
            {
                canAttaching = true;
                canGetOnInEachSituation = false;


                //見ること
                //それぞれのフォームごとに
                //マスからはみ出ていない
                //そのブロックにオブジェクトが重なっていない(1度でも満たしたらアウト)
                //タイプごとにその下または4方にブロックオブジェクトが隣接している(1度でも満たせばよい)

                //すべてクリアしているならば、その範囲をenableToAttachingに埋め込める

                checking(y, x);
                

                foreach(GridForm attachingEachGridForm in unitForm)
                {
                    checking(y + attachingEachGridForm.y, x + attachingEachGridForm.x);
                }


                if(canAttaching == true && canGetOnInEachSituation == true)
                {
                    //enableToAttachingに埋め込む

                    enableToAttaching[y, x] = true;

                    foreach (GridForm attachingEachGridForm in unitForm)
                    {
                        enableToAttaching[y + attachingEachGridForm.y, x + attachingEachGridForm.x] = true;
                    }
                }

            }
        }

        void checking(int y,int x)
        {
            if (y >= 0 && y < 10 && x >= 0 && x < 10)
            {
                if (eachGridsTiling[y, x] != TilingType.Empty)
                {
                    canAttaching = false;
                }
                else if (unitTylingType == TilingType.Pngn)
                {
                    if (y - 1 >= 0)
                    {
                        if (eachGridsTiling[y - 1, x] == TilingType.Object)
                        {
                            canGetOnInEachSituation = true;
                        }
                    }
                }
                else if (unitTylingType == TilingType.Object)
                {
                    if (y - 1 >= 0)
                    {
                        if (eachGridsTiling[y - 1, x] == TilingType.Object)
                        {
                            canGetOnInEachSituation = true;
                        }
                    }
                    if (y + 1 < 10)
                    {
                        if (eachGridsTiling[y + 1, x] == TilingType.Object)
                        {
                            canGetOnInEachSituation = true;
                        }
                    }
                    if (x - 1 >= 0)
                    {
                        if (eachGridsTiling[y, x-1] == TilingType.Object)
                        {
                            canGetOnInEachSituation = true;
                        }
                    }
                    if (x + 1 < 10)
                    {
                        if (eachGridsTiling[y, x + 1] == TilingType.Object)
                        {
                            canGetOnInEachSituation = true;
                        }
                    }
                }
            }
            else canAttaching = false;
        }

        return;
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
    private Formation formation = new Formation();//マス目部分int[] gridinfo = new int[10,10] ,船部分 int shiptype;

    public EachGrid[,] eachGrids = new EachGrid[10, 10];

    public RectTransform contentRectTransform;
    public RectTransform deleteButtonRectTransform;

    public Image eachGridImage;

    public GameObject movingUnit;

    public Image attachingShip;
    public EachGrid shipEachGrid = new EachGrid();

    public GridLayoutGroup gridLayoutGroup;

    public Sprite nullSprite;

    public Text formationCostText;

    /*****private field*****/
    private UnitData m_unitData;

    FormationChanger fc;

    private void Start()
    {

        //ここでとりあえずPrefsManagerからformationを取得
        formation = prefs.GetFormation(editParam.ownFormationNum);

        //以下デバッグ用
        /*formation.formationDataExists = true;
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
        formation.shiptype = 10010;*/

        //prefs.SetFormation(formation.gridinfo, formation.shiptype,editParam.ownFormationNum);

        //グリッドを生成、初期化
        //GenerateGridInfo();

        InstanceGridInfo();
        
        gridLayoutGroup.CalculateLayoutInputHorizontal();
        gridLayoutGroup.CalculateLayoutInputVertical();
        gridLayoutGroup.SetLayoutHorizontal();
        gridLayoutGroup.SetLayoutVertical();
        gridLayoutGroup.enabled = false;

        UpdateGridInfo();
        fc = new FormationChanger(formation, eachGrids, movingUnit, contentRectTransform, deleteButtonRectTransform, shipEachGrid);

        //UpdateGridInfo();

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

    private void ResizeImgSize(Image img, float imgSize)
    {

        //var img = transform.GetComponent<Image>();
        var t = img.GetComponent<RectTransform>();

        img.SetNativeSize();

        var width = t.sizeDelta.x * imgSize;
        var height = t.sizeDelta.y * imgSize;

        t.sizeDelta = new Vector2(width, height);

        return;
    }

    private Vector2 ConvertOffsetValue(Vector2 dataOffset)
    {
        return dataOffset * editParam.movingUnitOffset;
    }

    public void BackToSaveFormation()
    {
        formation = prefs.GetFormation(editParam.ownFormationNum);

        //prefs.SetFormation(formation.gridinfo, formation.shiptype,editParam.ownFormationNum);

        //グリッドを生成、初期化
        //GenerateGridInfo();

        /*gridLayoutGroup.CalculateLayoutInputHorizontal();
        gridLayoutGroup.CalculateLayoutInputVertical();
        gridLayoutGroup.SetLayoutHorizontal();
        gridLayoutGroup.SetLayoutVertical();
        gridLayoutGroup.enabled = false;*/
        ResetUnitOffsetAndResizeImgSize(shipEachGrid.img, editParam.attachingShipImgSize, shipEachGrid.sel.selectableUnitOffset);
        UpdateGridInfo();
        fc = new FormationChanger(formation, eachGrids, movingUnit, contentRectTransform, deleteButtonRectTransform, shipEachGrid);
    }

    public void SaveFormation()
    {
        prefs.SetFormation(formation.gridinfo, formation.shiptype, editParam.ownFormationNum);
    }

    public void InstanceGridInfo()
    {
        int[] attachingPosition = new int[2];

        for (int i = 0; i < 10; i++)
        {
            for (int j = 0; j < 10; j++)
            {
                attachingPosition[0] = i;
                attachingPosition[1] = j;

                Image img = Instantiate(eachGridImage, contentRectTransform);

                EachGrid eachGrid = new EachGrid();
                eachGrid.img = img.GetComponent<Image>();
                eachGrid.txt = img.transform.Find("Text").GetComponent<Text>();
                eachGrid.txt.text = "0";
                eachGrid.sel = img.GetComponent<SelectableUnit>();
                eachGrid.sel.beforeAttachingPosition = attachingPosition;

                eachGrids[i, j] = eachGrid;
                //Debug.Log("start:"+eachGrids[i, j].img.transform.position);
            }
        }

        shipEachGrid.img = attachingShip.GetComponent<Image>();
        shipEachGrid.txt = attachingShip.transform.Find("Text").GetComponent<Text>();
        shipEachGrid.txt.text = "0";

        editParam.formationCost = editParam.formationCostMax;
        UpdateFormationCostDisplay();

        return;
    }

    public void UpdateGridInfo()
    {
        editParam.formationCost = editParam.formationCostMax;

        for (int i = 0; i < 10; i++)
        {
            for (int j = 0; j < 10; j++)
            {
                m_unitData = masterData.FindUnitData(formation.gridinfo[i, j]);

                EachGrid eachGrid = eachGrids[i, j];

                
                int[] attachingPosition = new int[2];
                attachingPosition[0] = i;
                attachingPosition[1] = j;

                //Debug.Log(attachingPosition[0] + ":" + attachingPosition[1]);

                if (m_unitData != null)
                {
                    ResetUnitOffsetAndResizeImgSize(eachGrid.img, editParam.attachingUnitImgSize, eachGrid.sel.selectableUnitOffset);
                    eachGrid.UpdateEachGrid(m_unitData.sprite, m_unitData.ID, m_unitData.unitType, m_unitData.form, ConvertOffsetValue(m_unitData.offset), m_unitData.cost, attachingPosition);
                    //eachGrid.UpdateEachGrid(m_unitData.sprite, m_unitData.ID, m_unitData.unitType, m_unitData.form, ConvertOffsetValue(m_unitData.offset), m_unitData.cost, null);
                    SetUnitOffsetAndResizeImgSize(eachGrid.img, editParam.attachingUnitImgSize, eachGrid.sel.selectableUnitOffset);
                    editParam.formationCost = editParam.formationCost - m_unitData.cost;
                }
                else
                {
                    ResetUnitOffsetAndResizeImgSize(eachGrid.img, editParam.attachingUnitImgSize, eachGrid.sel.selectableUnitOffset);
                    //eachGrid.UpdateEachGrid(nullSprite, 0, UnitType.Pngn, null, Vector2.zero, 0, attachingPosition);
                    eachGrid.UpdateEachGrid(nullSprite, 0, UnitType.Pngn, null, Vector2.zero, 0, null);
                    SetUnitOffsetAndResizeImgSize(eachGrid.img, editParam.attachingUnitImgSize, Vector2.zero);
                }
                

                eachGrid.txt.text = formation.gridinfo[i, j].ToString();

                eachGrids[i, j] = eachGrid;

                //Debug.Log(eachGrids[i, j].sel.beforeAttachingPosition[0] + ":::" + eachGrids[i, j].sel.beforeAttachingPosition[1]);
                //Debug.Log("start:"+eachGrids[i, j].img.transform.position);
            }
        }



        m_unitData = masterData.FindUnitData(formation.shiptype);


        if (m_unitData != null)
        {
            //ResetUnitOffsetAndResizeImgSize(shipEachGrid.img, editParam.attachingShipImgSize, ConvertOffsetValue(m_unitData.offset));

            shipEachGrid.UpdateEachGrid(m_unitData.sprite, m_unitData.ID, m_unitData.unitType, m_unitData.form, ConvertOffsetValue(m_unitData.offset), m_unitData.cost, null);

            SetUnitOffsetAndResizeImgSize(shipEachGrid.img, editParam.attachingShipImgSize, ConvertOffsetValue(m_unitData.offset));
            editParam.formationCost = editParam.formationCost - m_unitData.cost;
        }
        else
        {
            //ResetUnitOffsetAndResizeImgSize(shipEachGrid.img, editParam.attachingShipImgSize, ConvertOffsetValue(m_unitData.offset));
            SetUnitOffsetAndResizeImgSize(shipEachGrid.img, editParam.attachingShipImgSize, Vector2.zero);
        }

        UpdateFormationCostDisplay();


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

        //GenerateGridInfo();
        
        return;
    }



    public void SelectEachShip(int shipID)
    {

        ResetUnitOffsetAndResizeImgSize(shipEachGrid.img, editParam.attachingShipImgSize, shipEachGrid.sel.selectableUnitOffset);

        formation.formationDataExists = true;
        formation.gridinfo = new int[10, 10];
        formation.shiptype = shipID;

        UpdateGridInfo();
        fc = new FormationChanger(formation, eachGrids, movingUnit, contentRectTransform, deleteButtonRectTransform, shipEachGrid);

        return;
    }

    public void DisplayEnableGrid(int unitID,UnitType unitType,GridForm[] unitForm,Vector2 unitOffset)
    {

        fc.UpdateEachGridsTiling();

        fc.UpdateEnableToAttaching(unitID, unitType, unitForm, unitOffset);

        /*fc.enableToAttaching = new bool[10, 10]
        {
            {  true,  true, false,  true,  true,  true,  true,  true,  true,  true},
            {  true,  true,  true,  true,  true,  true,  true,  true,  true,  true},
            {  true,  true,  true,  true,  true,  true,  true,  true,  true,  true},
            {  true,  true,  true,  true,  true,  true,  true,  true,  true,  true},
            {  true,  true, false,  true,  true,  true,  true,  true,  true,  true},
            {  true,  true,  true,  true,  true,  true,  true,  true,  true,  true},
            {  true,  true,  true,  true,  true,  true,  true,  true,  true,  true},
            {  true,  true,  true,  true,  true,  true,  true,  true,  true,  true},
            {  true,  true,  true,  true,  true,  true,  true,  true,  true,  true},
            {  true,  true,  true,  true,  true,  true,  true,  true,  true, false},
        };*/

        /*for (int i = 0; i < 10; i++)
        {
            for (int j = 0; j < 10; j++)
            {
                //enabletoattachingのマス目を光らせる
                if (fc.eachGridsTiling[i, j] != TilingType.Empty)
                {
                    //eachGrids[i, j].img.color = editParam.AttachingColor;
                    eachGrids[i, j].img.sprite = editParam.attachingSprite;
                    ResizeImgSize(eachGrids[i, j].img, editParam.attachingUnitImgSize);
                }
            }
        }*/

        for (int i = 0; i < 10; i++)
        {
            for (int j = 0; j < 10; j++)
            {
                //enabletoattachingのマス目を光らせる
                if (fc.enableToAttaching[i, j] == true)
                {
                    //eachGrids[i, j].img.color = editParam.AttachingColor;
                    eachGrids[i, j].img.sprite = editParam.attachingSprite;
                    ResizeImgSize(eachGrids[i, j].img, editParam.attachingUnitImgSize);
                }
            }
        }

        return;
    }

    public void HideEnableGrid()
    {
        /*for (int i = 0; i < 10; i++)
        {
            for (int j = 0; j < 10; j++)
            {
                //enabletoattachingのマス目の画像をもとに戻す
                if (fc.eachGridsTiling[i, j] == TilingType.Empty)
                {
                    //eachGrids[i, j].img.color = editParam.AttachingColor;
                    eachGrids[i, j].img.sprite = editParam.nullSprite;
                    ResizeImgSize(eachGrids[i, j].img, editParam.attachingUnitImgSize);
                }

            }
        }*/

        for (int i = 0; i < 10; i++)
        {
            for (int j = 0; j < 10; j++)
            {
                //enabletoattachingのマス目の画像をもとに戻す
                if (fc.enableToAttaching[i, j] == true)
                {
                    //eachGrids[i, j].img.color = editParam.AttachingColor;
                    eachGrids[i, j].img.sprite = editParam.nullSprite;
                    ResizeImgSize(eachGrids[i, j].img, editParam.attachingUnitImgSize);
                }

            }
        }

        return;
    }


    public void Attach(int movingUnitID, UnitType movinUnitType,GridForm[] movingUnitForm,Vector2 movingUnitOffset, int[] beforeAttachingUnitPosition)
    {

        int[] attachingUnitPosition = fc.SearchNearSquare(beforeAttachingUnitPosition, movingUnitID, movinUnitType, movingUnitForm);

        m_unitData = masterData.FindUnitData(movingUnitID);

        if (attachingUnitPosition != null)
        {
            //Debug.Log("attach"+movingUnitID);
            fc.formation.gridinfo[attachingUnitPosition[0], attachingUnitPosition[1]] = movingUnitID;
            if (m_unitData != null)
            {
                EachGrid eachGrid = fc.eachGrids[attachingUnitPosition[0], attachingUnitPosition[1]];

                ResetUnitOffsetAndResizeImgSize(eachGrid.img, editParam.attachingUnitImgSize, eachGrid.sel.selectableUnitOffset);

                eachGrid.UpdateEachGrid(m_unitData.sprite, m_unitData.ID, m_unitData.unitType, m_unitData.form, ConvertOffsetValue(m_unitData.offset), m_unitData.cost, attachingUnitPosition);
                editParam.formationCost = editParam.formationCost - m_unitData.cost;
                UpdateFormationCostDisplay();

                SetUnitOffsetAndResizeImgSize(eachGrid.img,editParam.attachingUnitImgSize,movingUnitOffset);

                //fc.UpdateCoordinates();
                

                //GridColorSelecting();
            }

        }

        //Regenerate();

        return;
    }

    public void GridColorDefault()
    {
        for (int i = 0; i < 10; i++)
        {
            for (int j = 0; j < 10; j++)
            {
                if (fc.eachGridsTiling[i, j] == 0)
                {
                    eachGrids[i, j].img.sprite = nullSprite;
                    ResizeImgSize(eachGrids[i, j].img, editParam.attachingUnitImgSize);
                }

                eachGrids[i, j].img.color = editParam.defaultColor;
            }
        }

        return;
    }


    public void UpdateFormationCostDisplay()
    {
        formationCostText.text = "残りコスト:" + editParam.formationCost;
        return;
    }

    public bool CheckingCutVertex(GridForm[] unitForm,int[] attachingPosition)
    {
        fc.UpdateEachGridsTiling();

        //Debug.Log(attachingPosition[0]+","+ attachingPosition[1]);


        TilingType[,] eachGridsTilingTmp = new TilingType[10,10];
        bool[,] checkedObjectTiledGrids = new bool[10, 10];
        bool[,] pngnGuard = new bool[10, 10];
        bool[,] pngnCheckedGuard = new bool[10, 10];
        bool alreadyFirstCheckingIsFinish = false;

        //bool jud_pngn;

        for (int y = 0; y < 10; y++)
        {
            for (int x = 0; x < 10; x++)
            {
                eachGridsTilingTmp[y, x] = fc.eachGridsTiling[y, x];

            }
        }

        eachGridsTilingTmp[attachingPosition[0], attachingPosition[1]] = TilingType.Empty;
        foreach(GridForm form in unitForm)
        {
            eachGridsTilingTmp[attachingPosition[0] + form.y, attachingPosition[1] + form.x] = TilingType.Empty;
        }

        for (int y = 0; y < 10; y++)
        {
            for (int x = 0; x < 10; x++)
            {
                if (eachGridsTilingTmp[y, x] != TilingType.Empty)
                {
                    /*if (eachGridsTilingTmp[y, x] == TilingType.Pngn && fc.eachGrids[y, x].sel.selectableUnitType == UnitType.Pngn)
                    {
                        pngnGuard[y, x] = true;

                        if (fc.eachGrids[y, x].sel.selectableUnitForm != null)
                        {
                            foreach (GridForm form in fc.eachGrids[y, x].sel.selectableUnitForm)
                            {
                                pngnGuard[y + form.y, x + form.x] = true;
                            }
                        }
                    }*/

                    if (checkedObjectTiledGrids[y, x] == false && alreadyFirstCheckingIsFinish == true && eachGridsTilingTmp[y, x] != TilingType.Pngn)
                    {
                        /*if (pngnCheckedGuard[y, x] != false)
                        {
                            Debug.Log("false" + y + "" + x);
                            return false;
                        }*/

                        return false;

                    }
                    else if(checkedObjectTiledGrids[y, x] == false)
                    {
                        //Debug.Log("notchecked"+y + "," + x + ":" + eachGridsTilingTmp[y, x]);
                        checking(y,x);
                        alreadyFirstCheckingIsFinish = true;
                    }

                    /*jud_pngn = true;
                    for (int py = 0; py < 10; py++)
                    {
                        for (int px = 0; px < 10; px++)
                        {
                            eachGridsTilingTmp[y, x] = fc.eachGridsTiling[y, x];
                            if (pngnGuard[py, px] != pngnCheckedGuard[py, px])
                            {
                                jud_pngn = false;
                            }

                        }
                    }

                    if (jud_pngn) deletepngnGuard();*/

                }
            }
        }



        /*void deletepngnGuard()
        {

            for (int y = 0; y < 10; y++)
            {
                for (int x = 0; x < 10; x++)
                {
                    pngnGuard[y, x] = false;
                    pngnCheckedGuard[y, x] = false;

                }
            }

            return;
        }*/



        void checking(int y,int x)
        {
            if (checkedObjectTiledGrids[y, x] == true) return;

            if (eachGridsTilingTmp[y, x] == TilingType.Empty) return;
            else checkedObjectTiledGrids[y, x] = true;

            //Debug.Log(y + "," + x);

            /*if (eachGridsTilingTmp[y, x] == TilingType.Pngn)
            {
                //下方向は確認する
                if (y - 1 >= 0) checking(y - 1, x);

                //やばいかも
                if (y + 1 < 10) checking(y + 1, x);
                if (x - 1 >= 0) checking(y, x - 1);
                if (x + 1 < 10) checking(y, x + 1);
            }
            else */if(eachGridsTilingTmp[y,x] == TilingType.Object)
            {
                if (y - 1 >= 0)
                {
                    //Debug.Log(y - 1);
                    
                    checking(y - 1, x);
                }
                if (y + 1 < 10) checking(y + 1, x);
                if (x - 1 >= 0) checking(y, x - 1);
                if (x + 1 < 10) checking(y, x + 1);
            }

            return;

        }


        return true;
    }






    //この先デバッグ用


    public void GridColorSelecting()
    {
        for (int i = 0; i < 10; i++)
        {
            for (int j = 0; j < 10; j++)
            {
                if (fc.eachGridsTiling[i, j] != TilingType.Empty)
                {
                    //eachGrids[i, j].img.color = editParam.unAttachingColor;
                    eachGrids[i, j].img.sprite = editParam.attachingSprite;
                    ResizeImgSize(eachGrids[i, j].img, editParam.attachingUnitImgSize);
                }
                else ResizeImgSize(eachGrids[i, j].img, editParam.attachingUnitImgSize);
            }
        }
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
