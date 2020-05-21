using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MissionListManager : MonoBehaviour
{
    public List<MissionItemData> missionItemList;
    public GameObject missionContainer;

    private int playableStageNum = 5;
    private int stageIndex = 1;
    private GameObject content;

    private void Awake()
    {
        content = GameObject.Find("Content");
    }

    void Start()
    {
        // TODO: playableStageNumを変更
        // stage番号を持たせないと.
        if (missionItemList.Count <= playableStageNum )
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
