using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class TitleSceneManager : MonoBehaviour
{
    //編成データ読み書き用
    PrefsManager prefs = new PrefsManager();
    //データ
    private Formation formation = new Formation();//マス目部分int[] gridinfo = new int[10,10] ,船部分 int shiptype;

    // Start is called before the first frame update
    void Start()
    {
        CheckFormationPrefIsSet();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void LoadStageSelectScene()
    {
        SceneManager.LoadScene("StageSelectScene");
    }
    public void LoadEditSelectScene()
    {
        SceneManager.LoadScene("EditSelectScene");
    }

    public void CheckFormationPrefIsSet()
    {
        string checkFormaionStr;

        for(int i = 1; i <= 3; i++)
        {
            checkFormaionStr = PlayerPrefs.GetString("formation" + i.ToString(), "NoData");
            if (checkFormaionStr == "NoData")
            {
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
                prefs.SetFormation(formation.gridinfo, formation.shiptype, i);
            }
        }

        return;
    }

}
