using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FormationGridManager : MonoBehaviour
{

    //編成データ読み書き用
    PrefsManager prefs = new PrefsManager();
    //データ
    Formation formation = new Formation();//マス目部分int[] gridinfo = new int[120] ,船部分 int shiptype;

    public MasterDataScript masterData;

    public RectTransform contentRectTransform;
    public Image image;
    public Sprite sanple;
    public UnitData unitData;



    private void Start()
    {
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

        for (int i = 0; i < 10; i++)
        {
            for (int j = 0; j < 10; j++)
            {
                var obj = Instantiate(image, contentRectTransform);
                obj.transform.Find("Text").GetComponent<Text>().text = formation.gridinfo[i,j].ToString();
                //obj.GetComponentInChildren<Text>().text = i.ToString() + j.ToString();
                //obj.sprite = masterData.FindUnitData(formation.gridinfo[i, j]).sprite;
                unitData = masterData.FindUnitData(formation.gridinfo[i, j]);
                if(unitData!=null)obj.transform.Find("Image").GetComponent<Image>().sprite = unitData.sprite;
            }
        }
    }



    public void regenerate()
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
                var obj = Instantiate(image, contentRectTransform);
                obj.transform.Find("Text").GetComponent<Text>().text = formation.gridinfo[i, j].ToString();
                //obj.GetComponentInChildren<Text>().text = i.ToString() + j.ToString();
                //obj.sprite = masterData.FindUnitData(formation.gridinfo[i, j]).sprite;
                unitData = masterData.FindUnitData(formation.gridinfo[i, j]);
                if (unitData != null) obj.transform.Find("Image").GetComponent<Image>().sprite = unitData.sprite;
            }
        }
    }





    public void Attach(int movingUnitID,Vector3 movingUnitPos)
    {
        for (int i = 0; i < 10; i++)
        {
            for (int j = 0; j < 10; j++)
            {
                formation.gridinfo[i, j] = 10;
            }
        }


        regenerate();

        return;
    }


}
