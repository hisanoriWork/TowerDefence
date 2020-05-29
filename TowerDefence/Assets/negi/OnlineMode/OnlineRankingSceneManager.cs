using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using UniRx;

public class OnlineRankingSceneManager : MonoBehaviour
{
    public GameObject rankingContainer;
    public GameObject rankingLayout;
    public GameObject circleLoading;
    public GameObject changeButton;

    public bool isWinCountPriority = true;

    public List<StageData> winCountStageData = new List<StageData>();
    public List<StageData> winPercentageData = new List<StageData>();

    private NCMBDatabase database = new NCMBDatabase();
    void Start()
    {
        //BGMManager.instance.SetVolume(1);
        //BGMManager.instance.Play("タイトル");
        BGMManager.instance.Play("タイトル");

        // TODO: 上位ランキングのデータFetch
        FetchTopStageData();
    }
    public void LoadOnlineEntranceScene()
    {
        SEManager.instance.Play("シーン遷移");
        SceneManager.LoadScene("OnlineEntranceScene");
    }
    public void ChangeList()
    {
        isWinCountPriority = !isWinCountPriority;
        FetchTopStageData();
    }
    private void FetchTopStageData()
    {
        circleLoading.SetActive(true);
        if (isWinCountPriority)
        {
            Debug.Log("AAAA");
            if (winCountStageData.Count < 1)
            {
                database.FetchRankingData();
                database.TopStageDataObservable.Subscribe(dataList =>
                {
                    if (dataList.Count > 0)
                    {
                        winCountStageData = dataList;
                        circleLoading.SetActive(false);
                        InflateItems(dataList);
                    }
                });
            }
            else
            {
                Debug.Log("CCCC");
                circleLoading.SetActive(false);
                InflateItems(winCountStageData);
            }
        }
        else
        {
            Debug.Log("BBBB");
            if (winPercentageData.Count < 1)
            {
                database.FetchRankingData2();
                database.TopStageDataObservable2.Subscribe(dataList =>
                {
                    if (dataList.Count > 0)
                    {
                        winPercentageData = dataList;
                        circleLoading.SetActive(false);
                        InflateItems(dataList);
                    }
                });
            }
            else
            {
                Debug.Log("DDDD");
                circleLoading.SetActive(false);
                InflateItems(winPercentageData);
            }
        }
    }

    private void InflateItems(List<StageData> topStageDataList)
    {
        foreach (Transform n in rankingContainer.transform)
        {
            GameObject.Destroy(n.gameObject);
        }
        // Inflate
        for (int i = 0; i < topStageDataList.Count; i++)
        {
            // TODO: SpriteGenのデータを反映させる
            var rankingTV = rankingLayout.transform.Find("Ranking").GetComponent<Text>();
            rankingTV.text = (i + 1).ToString();

            var stageNameTV = rankingLayout.transform.Find("Name").GetComponent<Text>();
            stageNameTV.text = topStageDataList[i].name;

            var stageDetailTV = rankingLayout.transform.Find("Detail").GetComponent<Text>();
            stageDetailTV.text = topStageDataList[i].detailContent;

            Instantiate(rankingLayout, rankingContainer.transform);
        }
    }
}
