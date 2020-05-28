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
        Debug.Log("Win");
        if (victoryFlag)
        {
            int playableStageNum = PlayerPrefs.GetInt("playableStageNum", 1);
            Debug.Log(playableStageNum + "PlayableIn");
            if (playableStageNum == stageNum)
            {
                playableStageNum++;
                PlayerPrefs.SetInt("playableStageNum", playableStageNum);
            }
        }
    }
}
