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

    private void FetchTopStageData()
    {
        circleLoading.SetActive(true);
        database.FetchRankingData();

        database.TopStageDataObservable.Subscribe(dataList =>
        {
            if (dataList.Count > 0)
            {
                circleLoading.SetActive(false);
                //TODO: 対戦に飛ぶなら必要.
                //MasterDataScript.instance.onlineStageDataList = dataList;
                InflateItems(dataList);
            }
        });
    }
    private void InflateItems(List<StageData> topStageDataList)
    {
        // Inflate
        for (int i = 0; i < topStageDataList.Count; i++  )
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
