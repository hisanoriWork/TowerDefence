using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EachGrid
{
    public Image img;
    public Text txt;

    public void UpdateEachGrid(Sprite sprite, string text)
    {
        this.img.sprite = sprite;
        this.txt.text = text;
    }
}

public class FormationChanger
{
    //編成情報
    public Formation formation;

    //移動体
    public GameObject movingUnit;
    public Vector2 movingUnitCoordinate;

    //左側グリッド
    public EachGrid[,] eachGrids;
    public Vector2[,] eachGridsCoordinate;

    public bool[,] isEnableToSetting;

    RectTransform contentRectTransform;

    //public List<>


    /*****public method*****/
    public FormationChanger(Formation formation, EachGrid[,] eachGrids, GameObject movingUnit,RectTransform contentRectTransform)
    {
        this.formation = formation;
        this.movingUnit = movingUnit;
        this.movingUnitCoordinate = movingUnit.transform.position;

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
        movingUnitCoordinate = movingUnit.transform.position;
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

    public int[] SearchNearSquare()
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
            
            Debug.Log("oyoyoyoyo");
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

            //Debug.Log(minUnitPosition[0] + ""+minUnitPosition[1]) ;

            return minUnitPosition;
        }

        return null;
    }

}

public class FormationGridManager : MonoBehaviour
{
    /*****public field*****/
    public MasterDataScript masterData;


    //編成データ読み書き用
    PrefsManager prefs = new PrefsManager();
    //データ
    private Formation formation = new Formation();//マス目部分int[] gridinfo = new int[120] ,船部分 int shiptype;

    public EachGrid[,] eachGrids = new EachGrid[10,10];

    FormationChanger fc;

    public RectTransform contentRectTransform;
    public Image eachGridImage;

    public GameObject movingUnit;



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
            { 10, 10,100,100,  0,  0,100,  0,  0,  0},
            {  0,  0,  0, 10,  0,  0,  0,  0,  0,  0},
            {  0,  0,  0,  0,  0,  0,  0,  0,  0,  0},
            {  0,  0,  0,  0,  0,  0,  0,  0,  0,  0},
            {  0,  0,  0,  0,  0,  0,  0,  0,  0,  0},
            {  0,  0,  0,  0,  0,  0,  0,  0,  0,  0},
            {  0,  0,  0,  0,  0,  0,  0,  0,  0,  0},
        };
        formation.shiptype = 10010;

        prefs.SetFormation(formation.gridinfo,formation.shiptype);

        //グリッドを生成、初期化
        GenerateGridInfo();
        fc = new FormationChanger(formation, eachGrids, movingUnit,contentRectTransform);




        //masterData.FindUnitData(10).

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

                if (m_unitData != null) eachGrid.img.sprite = m_unitData.sprite;
                eachGrid.txt.text = formation.gridinfo[i, j].ToString();

                eachGrids[i, j] = eachGrid;
                //Debug.Log("start:"+eachGrids[i, j].img.transform.position);
            }
        }
        return;
    }

    public void UpdateGridInfo()
    {
        foreach (RectTransform child in contentRectTransform)
        {
            //削除する
            Destroy(child.gameObject);
        }
        for (int i = 0; i < 10; i++)
        {
            for (int j = 0; j < 10; j++)
            {

                Image img = Instantiate(eachGridImage, contentRectTransform);

                m_unitData = masterData.FindUnitData(formation.gridinfo[i, j]);

                EachGrid eachGrid = new EachGrid();
                eachGrid.img = img.GetComponent<Image>();
                eachGrid.txt = img.transform.Find("Text").GetComponent<Text>();

                if (m_unitData != null) eachGrid.img.sprite = m_unitData.sprite;
                eachGrid.txt.text = formation.gridinfo[i, j].ToString();

                eachGrids[i, j] = eachGrid;
            }
        }
        return;
    }

    public void Attach(int movingUnitID, UnitType movinUnitType)
    {

        int[] attachingUnitPosition = fc.SearchNearSquare();

        m_unitData = masterData.FindUnitData(movingUnitID);

        if (attachingUnitPosition != null)
        {
            //Debug.Log("attach"+movingUnitID);
            fc.formation.gridinfo[attachingUnitPosition[0], attachingUnitPosition[1]] = movingUnitID;
            if (m_unitData != null) fc.eachGrids[attachingUnitPosition[0], attachingUnitPosition[1]].UpdateEachGrid(m_unitData.sprite, movingUnitID.ToString());
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
