using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MissionDetailController : MonoBehaviour
{
    /*****public field*****/
    public GameObject misshionDetailWindow;
    public Text selectMissionTV;
    public Image enemyPreViewIV;
    public Text DetailContentTV;
    public Text Difficulty;

    public StageDataManager stageDataManager;

    /*****private field*****/
    private StageData stageData;

    public void ChangeDetailContent(int selectStageNum)
    {
        if (stageData = stageDataManager.stageDataList[selectStageNum - 1])
        {
            selectMissionTV.text = "ステージ" + selectStageNum;
            enemyPreViewIV.sprite = stageData.preViewSprite;
            DetailContentTV.text = stageData.detailContent;
            var difficultyStarNum = "";
            for (int i = 0; i <= stageData.difficulty; i++)
            {
                difficultyStarNum += "★";
            }
            Difficulty.text = "難易度 : " + difficultyStarNum;
        }
        else
        {
            Debug.Log("不正なステージが選択されました.");
        }
    }
}
