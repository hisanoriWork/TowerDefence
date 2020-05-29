using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class MissionListManager : MonoBehaviour
{
    public GameObject missionContainer;

    private int playableStageNum = 5;
    private int stageIndex = 1;
    private GameObject content;
    private NCMBDatabase database = new NCMBDatabase();

    private void Awake()
    {
        content = GameObject.Find("Content");
    }

    void Start()
    {
        ViewMissons(PlayerPrefs.GetString("DirectToStageSelect", "FromTitle"));
    }

    private void ViewMissons(string from)
    {
        if (from.Equals("FromTitle"))
        {
            List<StageData> missionItemList = MasterDataScript.instance.stageDataList;

            playableStageNum = PlayerPrefs.GetInt("playableStageNum", 1);
            Debug.Log(playableStageNum + "Playable");
            if (missionItemList.Count <= playableStageNum)
            {
                playableStageNum = missionItemList.Count;
            }
            for (int i = 0; i < playableStageNum; i++)
            {
                var m_Text = missionContainer.transform.Find("EnemyTitle").GetComponent<Text>();
                m_Text.text = missionItemList[i].name;

                missionContainer.GetComponent<StageItemListener>().stageNum = stageIndex;
                stageIndex++;

                Instantiate(missionContainer, content.transform);
            }
        }
        else
        {
            database.FetchAllStageData(this);
        }
    }

    public void ヤバい(List<StageData> missionItemList)
    {
        MasterDataScript.instance.onlineStageDataList = missionItemList;
        for (int i = 0; i < database.fetchStageDataList.Count; i++)
        {
            var m_Text = missionContainer.transform.Find("EnemyTitle").GetComponent<Text>();
            m_Text.text = database.fetchStageDataList[i].name;

            missionContainer.GetComponent<StageItemListener>().stageNum = stageIndex;
            stageIndex++;

            Instantiate(missionContainer, content.transform);
        }
    }
}
