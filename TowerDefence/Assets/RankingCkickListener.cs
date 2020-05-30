using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RankingCkickListener : MonoBehaviour
{
    public GameObject messeageWindow;
    public int stageIndex;
    public void OnCkickListener()
    {
        MasterDataScript.instance.battleStageData = MasterDataScript.instance.onlineStageDataList[stageIndex];
        messeageWindow.SetActive(true);
    }
}
