using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StageSelectManager : MonoBehaviour
{
    public MissionDetailController missionDetailController;

    void Awake()
    {
    }

    public void ChangeSelectStage(int selectStageNum)
    {
        missionDetailController.ChangeSelectStageNum(selectStageNum);
    }
}
