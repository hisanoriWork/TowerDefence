﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MissionListManager : MonoBehaviour
{
    public MasterDataScript masterData;
    //public List<MissionItemData> missionItemList;
    public GameObject missionContainer;

    private int playableStageNum = 1;
    private int stageIndex = 1;
    private GameObject content;

    private void Awake()
    {
        content = GameObject.Find("Content");
    }

    void Start()
    {
        List<MissionItemData> missionItemList = masterData.missionItemDataList;
      
        playableStageNum = PlayerPrefs.GetInt("playableStageNum", 1);

        if (missionItemList.Count <= playableStageNum)
        {
            playableStageNum = missionItemList.Count;
        }
        for (int i = 0; i < playableStageNum; i++)
        {
            var m_Image = missionContainer.transform.Find("EnemyIcon").GetComponent<Image>();
            m_Image.sprite = missionItemList[i].stageIcon;

            var m_Text = missionContainer.transform.Find("EnemyTitle").GetComponent<Text>();
            m_Text.text = missionItemList[i].title;

            missionContainer.GetComponent<StageItemListener>().stageNum = stageIndex;
            stageIndex++;

            Instantiate(missionContainer, content.transform);
        }
    }
}
