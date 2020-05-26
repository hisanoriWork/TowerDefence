using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;







//ここを読んでください

//SpriteGenerator(これ)のインスペクタに
//・シーンに内にMasterDataを追加し、それをアタッチしてください
//・CanvasScalerに親元のCanvasをアタッチしてください
//・SpriteGeneratorGridの幅と高さを必要なものに変更してください
//　この幅と高さからは船がはみ出ます
//　幅と高さを1対1にすること
//　わかりにくかったらSpriteGeneratorGridの色の透明化をなくせばいいです








public class SpriteGenerator : MonoBehaviour
{
    
    public MasterDataScript masterData;
    //public EditParam editParam;

    public EachGrid[,] eachGrids;

    public Image attachingShip;
    public EachGrid shipEachGrid;

    public Sprite nullSprite;

    public Image childImage;


    public CanvasScaler canvasScaler;
    public float canvasScale = 1;

    public float offsetParam = 1000;
    public float unitSize = 0.01f;
    public float shipSize = 0.01f;


    private Image m_baseImage;

    private float m_width;
    private float m_height;
    private float m_unitImgSize;
    private float m_shipImgSize;

    private GridLayoutGroup gridLayoutGroup;

    private UnitData m_unitData;



    //編成データ読み書き用
    PrefsManager prefs = new PrefsManager();
    //データ
    private Formation formation = new Formation();//マス目部分int[] gridinfo = new int[10,10] ,船部分 int shiptype;

    private void Awake()
    {
        m_baseImage = GetComponent<Image>();
        gridLayoutGroup = GetComponent<GridLayoutGroup>();
        canvasScale = Screen.height / canvasScaler.referenceResolution.x;
        eachGrids = new EachGrid[10, 10];
        shipEachGrid = new EachGrid();



        m_width = m_baseImage.rectTransform.sizeDelta.x;
        m_height = m_baseImage.rectTransform.sizeDelta.y;
        //Debug.Log(m_width);

        m_unitImgSize = m_width * unitSize;
        m_shipImgSize = m_width * shipSize;

        gridLayoutGroup.cellSize = new Vector2(m_width / 10, m_height / 10);

        InstanceGridInfo();

        gridLayoutGroup.CalculateLayoutInputHorizontal();
        gridLayoutGroup.CalculateLayoutInputVertical();
        gridLayoutGroup.SetLayoutHorizontal();
        gridLayoutGroup.SetLayoutVertical();
        gridLayoutGroup.enabled = false;
    }

    // Start is called before the first frame update
    void Start()
    {
       
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void GenerateSprite(Formation importformation)
    {
        //formation = importformation;
        for (int y = 0; y < 10; y++)
        {
            for (int x = 0; x < 10; x++)
            {
                formation.gridinfo[y, x] = importformation.gridinfo[y, x];

            }
        }

        formation.shiptype = importformation.shiptype;


        UpdateGridInfo();

        /*for (int y = 0; y < 10; y++)
        {
            for (int x = 0; x < 10; x++)
            {
                //Debug.Log(formation.gridinfo[y, x]);

                UnitData unitData = masterData.FindUnitData(formation.gridinfo[y, x]);

            }
        }*/



        //baseImage.color = new Color(123, 12, 43);
        //baseImage.sprite = null;

        //Image chileImageTmp = (Image)Instantiate(childImage);
        //chileImageTmp.transform.SetParent(baseImage.transform, false);


        //GameObject gameObject = new GameObject();

        //gameObject.transform.SetParent(baseImage.transform, false);

        //gameObject.


        //GameObject prefab = (GameObject)Instantiate(gameObject);
        //prefab.transform.SetParent(baseImage.transform, false);


        return;
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

                Image img = Instantiate(childImage, m_baseImage.transform);

                EachGrid eachGrid = new EachGrid();
                eachGrid.img = img.GetComponent<Image>();
                eachGrid.txt = img.transform.Find("Text").GetComponent<Text>();
                eachGrid.txt.text = "0";
                eachGrid.sel = img.GetComponent<SelectableUnit>();
                eachGrid.sel.beforeAttachingPosition = attachingPosition;

                //Debug.Log("start:"+eachGrids[i, j].img.transform.position);
                eachGrids[i, j] = eachGrid;
                
            }
        }

        shipEachGrid.img = attachingShip.GetComponent<Image>();
        shipEachGrid.txt = attachingShip.transform.Find("Text").GetComponent<Text>();
        shipEachGrid.txt.text = "0";
        shipEachGrid.sel = attachingShip.GetComponent<SelectableUnit>();

        return;
    }

    public void UpdateGridInfo()
    {

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

                //Debug.Log(eachGrid.sel.selectableUnitOffset);

                if (m_unitData != null)
                {
                    ResetUnitOffsetAndResizeImgSize(eachGrid.img, m_unitImgSize, eachGrid.sel.selectableUnitOffset);
                    eachGrid.UpdateEachGrid(m_unitData.sprite, m_unitData.ID, m_unitData.unitType, m_unitData.form, m_unitData.emptyForm,
                        m_unitData.offset, m_unitData.cost, attachingPosition);
                    //eachGrid.UpdateEachGrid(m_unitData.sprite, m_unitData.ID, m_unitData.unitType, m_unitData.form, ConvertOffsetValue(m_unitData.offset), m_unitData.cost, null);
                    SetUnitOffsetAndResizeImgSize(eachGrid.img, m_unitImgSize, eachGrid.sel.selectableUnitOffset);
                }
                else
                {
                    ResetUnitOffsetAndResizeImgSize(eachGrid.img, m_unitImgSize, eachGrid.sel.selectableUnitOffset);
                    //eachGrid.UpdateEachGrid(nullSprite, 0, UnitType.Pngn, null, Vector2.zero, 0, attachingPosition);
                    eachGrid.UpdateEachGrid(nullSprite, 0, UnitType.Pngn, null, null, Vector2.zero, 0, null);
                    SetUnitOffsetAndResizeImgSize(eachGrid.img, m_unitImgSize, Vector2.zero);
                }


                eachGrid.txt.text = formation.gridinfo[i, j].ToString();

                eachGrids[i, j] = eachGrid;

                //Debug.Log(eachGrids[i, j].sel.beforeAttachingPosition[0] + ":::" + eachGrids[i, j].sel.beforeAttachingPosition[1]);
                //Debug.Log("start:"+eachGrids[i, j].img.transform.position);
            }
        }



        m_unitData = masterData.FindUnitData(formation.shiptype);


        
        if(shipEachGrid.sel.selectableUnitOffset != null)
        {
            ResetUnitOffsetAndResizeImgSize(shipEachGrid.img, m_shipImgSize, shipEachGrid.sel.selectableUnitOffset);
        }


        if (m_unitData != null)
        {
            //ResetUnitOffsetAndResizeImgSize(shipEachGrid.img, editParam.attachingShipImgSize, ConvertOffsetValue(m_unitData.offset));
            

            shipEachGrid.UpdateEachGrid(m_unitData.sprite, m_unitData.ID, m_unitData.unitType, m_unitData.form, m_unitData.emptyForm,
                m_unitData.offset, m_unitData.cost, null);

            SetUnitOffsetAndResizeImgSize(shipEachGrid.img, m_shipImgSize,m_unitData.offset);
        }
        else
        {
            //SetUnitOffsetAndResizeImgSize(shipEachGrid.img, m_shipImgSize, Vector2.zero);
            //ResetUnitOffsetAndResizeImgSize(shipEachGrid.img, editParam.attachingShipImgSize, ConvertOffsetValue(m_unitData.offset));
            SetUnitOffsetAndResizeImgSize(shipEachGrid.img, m_shipImgSize, Vector2.zero);
        }


        return;
    }

    private Vector2 ConvertOffsetValue(Vector2 dataOffset,float imgSize)
    {
        return dataOffset * imgSize;
    }

    private void SetUnitOffsetAndResizeImgSize(Image img, float imgsize, Vector2 selectableUnitOffset)
    {

        //var img = transform.GetComponent<Image>();
        var t = img.GetComponent<RectTransform>();

        img.SetNativeSize();

        var width = t.sizeDelta.x * imgsize ;
        var height = t.sizeDelta.y * imgsize ;

        t.sizeDelta = new Vector2(width, height);

        img.transform.position = (Vector2)img.transform.position + selectableUnitOffset * imgsize * canvasScale * offsetParam;
        //img.rectTransform.position = (Vector2)img.rectTransform.position - selectableUnitOffset * imgSize;

        return;
    }

    private void ResetUnitOffsetAndResizeImgSize(Image img, float imgsize, Vector2 selectableUnitOffset)
    {

        //var img = transform.GetComponent<Image>();
        var t = img.GetComponent<RectTransform>();

        img.SetNativeSize();

        var width = t.sizeDelta.x * imgsize;
        var height = t.sizeDelta.y * imgsize ;

        t.sizeDelta = new Vector2(width, height);

        img.transform.position = (Vector2)img.transform.position - selectableUnitOffset * imgsize * canvasScale * offsetParam;
        //img.rectTransform.position = (Vector2)img.rectTransform.position - selectableUnitOffset * imgSize;

        return;
    }




}
