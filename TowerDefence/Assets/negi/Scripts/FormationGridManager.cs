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

    public RectTransform contentRectTransform;
    public Image image;


    private void Start()
    {
        formation = prefs.GetFormation();


        for (int i = 0; i < 100; i++)
        {
            var obj = Instantiate(image, contentRectTransform);
            obj.GetComponentInChildren<Text>().text = formation.gridinfo[i].ToString();
        }
    }


}
