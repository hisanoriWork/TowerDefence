using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StageNumManager : MonoBehaviour
{
    public int stageNum
    {
        get
        {
            if (m_stageNum < 0)
                m_stageNum = int.Parse(PlayerPrefs.GetString("stageNum", "1"));
            return m_stageNum;
        }
    }
    private int m_stageNum = -1;

    public void SetPlayableStageNum(bool victoryFlag)
    {
        if (victoryFlag)
        {
            int playableStageNum = int.Parse(PlayerPrefs.GetString("playableStageNum", "1"));
            if (playableStageNum == stageNum)
            {
                playableStageNum++;
                PlayerPrefs.SetString("playableStageNum", playableStageNum.ToString());
            }
        }
    }
}
