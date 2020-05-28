using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MissionDetailController : MonoBehaviour
{
    /*****public field*****/
    public MasterDataScript masterData;
    public GameObject misshionDetailWindow;
    public Text selectMissionTV;
    public Image enemyPreViewIV;
    public Text DetailContentTV;
    public Text Difficulty;

    private string from = "FromTitle";
    private bool isLocal = true;

    public SpriteGenerator spriteGenerator;

    //public StageDataManager stageDataManager;

    /*****private field*****/
    private StageData stageData;
    private int stageIndex = 1;

    private void Start()
    {
        PlayerPrefs.SetString("stageNum", stageIndex.ToString());
        if (!from.Equals(PlayerPrefs.GetString("DirectToStageSelect", "FromTitle")))
        {
            isLocal = false;
        }
        //ChangeDetailContent();
        spriteGenerator.GenerateSprite(masterData.stageDataList[0].GetFormation());
    }

    public void ChangeDetailContent()
    {

        List<StageData> stageList;
        if (isLocal)
        {
            PlayerPrefs.SetString("stageNum", stageIndex.ToString());
            stageList = masterData.stageDataList;
        } else
        {
            stageList = masterData.onlineStageDataList;
        }

        if (stageList.Count >= stageIndex)
        {

            stageData = stageList[stageIndex - 1];
            Debug.Log(stageData.ID);
            selectMissionTV.text = stageIndex.ToString(); ;

            spriteGenerator.GenerateSprite(stageData.GetFormation());

            DetailContentTV.text = stageData.detailContent;
            var difficultyStarNum = "";
            for (int i = 0; i < stageData.difficulty; i++)
            {
                difficultyStarNum += "★";
            }
            Difficulty.text = "難易度 : " + difficultyStarNum;
        }
        else
        {
            Debug.Log("不正な値が入力されました.");
            stageIndex = 0;
            ChangeDetailContent();
        }
    }

    public void ChangeSelectStageNum(int selectStageNum)
    {
        if (selectStageNum >= 0)
        {
            stageIndex = selectStageNum;
        }
        else
        {
            Debug.Log("不正なステージが選択されました.");
            stageIndex = 0;
        }
        ChangeDetailContent();
    }
}
