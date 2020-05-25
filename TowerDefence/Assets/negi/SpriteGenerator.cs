using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SpriteGenerator : MonoBehaviour
{
    
    public MasterDataScript masterData;
    
    public Image childImage;


    private Image m_baseImage;

    private float m_width;
    private float m_height;

    private GridLayoutGroup gridLayoutGroup;



    //編成データ読み書き用
    PrefsManager prefs = new PrefsManager();
    //データ
    private Formation formation = new Formation();//マス目部分int[] gridinfo = new int[10,10] ,船部分 int shiptype;

    private void Awake()
    {
        m_baseImage = GetComponent<Image>();
        gridLayoutGroup = GetComponent<GridLayoutGroup>();
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void genButton()
    {
        formation = prefs.GetFormation(1);

        GenerateSprite(formation);

        return;
    }

    public void GenerateSprite(Formation formation)
    {
        m_width = m_baseImage.rectTransform.sizeDelta.x;
        m_height = m_baseImage.rectTransform.sizeDelta.y;
        Debug.Log(m_baseImage.rectTransform.sizeDelta.x);

        gridLayoutGroup.cellSize = new Vector2(m_width/10, m_height/10);


        InstanceGridInfo();

        for (int y = 0; y < 10; y++)
        {
            for (int x = 0; x < 10; x++)
            {
                //Debug.Log(formation.gridinfo[y, x]);

                UnitData unitData = masterData.FindUnitData(formation.gridinfo[y, x]);

            }
        }



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

                //Debug.Log("start:"+eachGrids[i, j].img.transform.position);
            }
        }

        return;
    }























}
