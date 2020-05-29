using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MissionDetailController : MonoBehaviour
{
    /*****public field*****/
    public GameObject misshionDetailWindow;
    public GameObject DetailBox;
    public Text selectMissionTV;
    public Image enemyPreViewIV;
    public Text DetailContentTV;
    public Text Difficulty;

    private string from = "FromTitle";
    private bool isLocal = true;

    public SpriteGenerator spriteGenerator;

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
    }

    public void ChangeDetailContent()
    {

        List<StageData> stageList;
        if (isLocal)
        {
            PlayerPrefs.SetString("stageNum", stageIndex.ToString());
            stageList = MasterDataScript.instance.stageDataList;
        }
        else
        {
            stageList = MasterDataScript.instance.onlineStageDataList;
        }

        if (stageList.Count >= stageIndex)
        {
            DetailBox.SetActive(true);
            stageData = stageList[stageIndex - 1];
            MasterDataScript.instance.battleStageData = stageData;
            selectMissionTV.text = stageData.name.ToString(); ;
            spriteGenerator.GenerateSprite(stageData.GetFormation());
            PlayerPrefs.SetString("StageDataUuid", stageData.uuid);
            DetailContentTV.text = stageData.detailContent;

            // TODO: uuidに値があるならdifficultyStarNumを計算.
            var difficultyStarNum = "";

            if ( !stageData.uuid.Equals("") )
            {
                stageData.difficulty = stageData.CalDifficulty();
            }
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
