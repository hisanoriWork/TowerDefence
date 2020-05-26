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

    //public StageDataManager stageDataManager;

    /*****private field*****/
    private StageData stageData;
    private int stageIndex = 1;

    private void Start()
    {
        PlayerPrefs.SetString("stageNum", stageIndex.ToString());
        ChangeDetailContent();
    }

    public void ChangeDetailContent()
    {
        if (masterData.stageDataList.Count >= stageIndex)
        {
            PlayerPrefs.SetString("stageNum", stageIndex.ToString());

            stageData = masterData.stageDataList[stageIndex - 1];
            selectMissionTV.text = "ステージ" + (stageIndex);
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
