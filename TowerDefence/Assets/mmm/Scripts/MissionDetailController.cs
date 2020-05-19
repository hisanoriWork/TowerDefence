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
    private int stageIndex = 0;

    private void Start()
    {
        ChangeDetailContent();
    }

    public void ChangeDetailContent()
    {
        if (stageDataManager.stageDataList.Count >= stageIndex)
        {
            stageData = stageDataManager.stageDataList[stageIndex];
            selectMissionTV.text = "ステージ" + (stageIndex + 1);
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
            Debug.Log("不正な値が入力されました.");
            stageIndex = 0;
            ChangeDetailContent();
        }
    }

    public void ChangeSelectStageNum(int selectStageNum)
    {
        if (selectStageNum >= 0)
        {
            stageIndex = selectStageNum - 1;
        }
        else
        {
            Debug.Log("不正なステージが選択されました.");
            stageIndex = 0;
        }
        ChangeDetailContent();
    }
}
