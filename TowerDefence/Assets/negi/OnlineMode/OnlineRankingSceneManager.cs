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
        database.FetchRankingData();

        database.TopStageDataObservable.Subscribe(dataList =>
        {
            if (dataList != null && dataList.Count > 0)
            {
                //TODO: もしかしたらいるかも?どうするか..
                //MasterDataScript.instance.onlineStageDataList = dataList;
                InflateItems(dataList);
            }
        });
    }
    private void InflateItems(List<StageData> topStageDataList)
    {
        // Inflate
        foreach (StageData data in topStageDataList)
        {
            // TODO: データを反映させる

            Instantiate(rankingLayout, rankingContainer.transform);
        }
    }
}
