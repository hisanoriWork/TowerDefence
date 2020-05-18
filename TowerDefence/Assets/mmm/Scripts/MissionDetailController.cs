using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MissionDetailController : MonoBehaviour
{
    /*****public field*****/
    public GameObject misshionDetailWindow;
    public GameObject missionDetailTV;
    public GameObject enemyPreViewIV;
    public Text DetailContentTV;

    public void ChangeDetailContent(int selectStageNum)
    {
        DetailContentTV.text = "ステージ" + selectStageNum + "が選択されました.";
    }
}
