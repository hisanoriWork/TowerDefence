using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StageSelectManager : MonoBehaviour
{
    /*****public field*****/
    public int stageNum = 1;

    public MissionDetailController missionDetailController;

    void Awake()
    {
    }

    public void ChangeSelectStage(int selectStageNum)
    {
        stageNum = selectStageNum;
        Debug.Log("ステージ" +selectStageNum + "が選ばれました");
        //TODO: MissionDetailWindowを変更する
        missionDetailController.ChangeDetailContent(selectStageNum);
    }
}
