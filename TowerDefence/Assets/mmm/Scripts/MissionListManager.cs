﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UniRx;
using System.Linq;

public class MissionListManager : MonoBehaviour
{
    public GameObject missionContainer;
    public GameObject circleLoading;

    private int playableStageNum = 1;
    private int stageIndex = 1;
    private GameObject content;

    private List<StageData> list = new List<StageData>();

    private NCMBDatabase database = new NCMBDatabase();

    private void Awake()
    {
        content = GameObject.Find("Content");
    }

    void Start()
    {
        FindMissionItems(PlayerPrefs.GetString("DirectToStageSelect", "FromTitle"));
    }

    private void FindMissionItems(string from)
    {
        if (from.Equals("FromTitle"))
        {
            List<StageData> missionItemList = MasterDataScript.instance.stageDataList;

            playableStageNum = PlayerPrefs.GetInt("playableStageNum", 1);
            Debug.Log("playableStageNum1:" + playableStageNum.ToString());

            if (missionItemList.Count <= playableStageNum)
            {
                playableStageNum = missionItemList.Count;
            }

            for (int i = 0; i < playableStageNum; i++ )
            {
                list.Add(missionItemList[i]);
            }

            InflateItems(list);
        }
        else
        {
            circleLoading.SetActive(true);
            FetchRemoteStageData();
        }
    }

    private void FetchRemoteStageData()
    {
        database.FetchAllStageData();
        database.StageDataObservable.Subscribe(dataList =>
        {
            if (dataList.Count > 0)
            {
                circleLoading.SetActive(false);
                MasterDataScript.instance.onlineStageDataList = dataList;
                InflateItems(dataList);
            }
        });
    }
    private void InflateItems(List<StageData> missionItemList)
    {
        foreach (StageData data in missionItemList)
        {
            var m_Text = missionContainer.transform.Find("EnemyTitle").GetComponent<Text>();
            m_Text.text = data.name;

            missionContainer.GetComponent<StageItemListener>().stageNum = stageIndex;
            stageIndex++;

            Instantiate(missionContainer, content.transform);
        }
    }
}

