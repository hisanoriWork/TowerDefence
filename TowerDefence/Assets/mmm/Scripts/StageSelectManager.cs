using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StageSelectManager : MonoBehaviour
{
    public MissionDetailController missionDetailController;

    void Awake()
    {
        BGMManager.instance.Play("ミッション");
    }

    public void ChangeSelectStage(int selectStageNum)
    {
        missionDetailController.ChangeSelectStageNum(selectStageNum);
    }
}
