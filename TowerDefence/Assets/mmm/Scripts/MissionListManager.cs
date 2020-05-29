using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UniRx;
using System.Linq;

public class MissionListManager : MonoBehaviour
{
    public GameObject missionContainer;

    private int playableStageNum = 5;
    private int stageIndex = 1;
    private GameObject content;[SerializeField]
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

            if (missionItemList.Count <= playableStageNum)
            {
                playableStageNum = missionItemList.Count;
            }

            missionItemList.Take(playableStageNum);

            InflateItems(missionItemList);
        }
        else
        {
            FetchRemoteStageData();
        }
    }

    private void FetchRemoteStageData()
    {
        database.tchAllStageData();
        database.ob.Subscribe(fetc =>
        {
            if (fetc != null && fetc.Count > 0)
            {
                MasterDataScript.instance.onlineStageDataList = fetc;
                InflateItems(fetc);
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

