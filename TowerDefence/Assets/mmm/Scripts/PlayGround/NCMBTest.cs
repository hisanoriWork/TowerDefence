using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using NCMB;

public class NCMBTest : MonoBehaviour
{
    // NCMBを利用するためのクラス
    private NCMBQuery<NCMBObject> queryPostStage;
    private NCMBQuery<NCMBObject> queryFetchAllStage;

    void Start()
    {
        //TODO: 削除
        PostStageData(1, "お試し", 1);
        FetchAllStageData();
    }

    public void PostStageData(int slotNum, string detailContent, int difficulty)
    {
        PrefsManager prefs = new PrefsManager();
        Formation formation = prefs.GetFormation(slotNum);

        if (formation.formationDataExists)
        {
            queryPostStage = new NCMBQuery<NCMBObject>("OnlineStageData");

            // 保存されているデータ件数を取得
            queryPostStage.CountAsync((int count, NCMBException e) =>
            {
                if (e != null)
                {
                    //件数取得失敗時の処理
                    Debug.Log("件数の取得に失敗しました");
                }
                else
                {
                    NCMBObject stageData = new NCMBObject("OnlineStageData");

                    // オブジェクトに値を設定
                    stageData["ID"] = count + 1;
                    stageData["detailContent"] = detailContent;

                    stageData["gridInfo"] = formation.gridinfo;
                    stageData["shipInfo"] = formation.shiptype;
                    stageData["difficulty"] = difficulty;

                    stageData["winCount"] = 0;
                    stageData["loseCount"] = 0;
                    //TODO: カラムの追加

                    // データストアへの登録
                    stageData.SaveAsync((NCMBException ee) =>
                    {
                        if (ee != null)
                        {
                            //エラー処理
                        }
                        else
                        {
                            //成功時の処理
                        }
                    });
                }
            });
        }
    }

    public List<StageData> FetchAllStageData()
    {
        var stageDatas = new List<StageData>();
        queryFetchAllStage = new NCMBQuery<NCMBObject>("OnlineStageData");
        queryFetchAllStage.FindAsync((List<NCMBObject> fetchList, NCMBException e) =>
        {
            if (e != null)
            {
                //検索失敗時の処理
            }
            else
            {
                //検索成功時の処理
                //TODO: 最大表示数を決める?
                foreach (NCMBObject fetchStage in fetchList)
                {
                    Debug.Log("aa");
                    stageDatas.Add(ParceStageData(fetchStage));
                }
            }
        });

        return stageDatas;
    }

    private StageData ParceStageData(NCMBObject fetchStage)
    {
        StageData stageData = ScriptableObject.CreateInstance<StageData>();
        stageData.ID = System.Convert.ToInt32(fetchStage["ID"]);
        stageData.detailContent = fetchStage["detailContent"].ToString();
        stageData.gridInfo = fetchStage["gridInfo"] as int[];
        stageData.shipInfo = System.Convert.ToInt32(fetchStage["shipInfo"]);
        stageData.difficulty = System.Convert.ToInt32(fetchStage["difficulty"]);

        stageData.winCount = System.Convert.ToInt32(fetchStage["winCount"]);
        stageData.loseCount = System.Convert.ToInt32(fetchStage["loseCount"]);
        return stageData;
    }
}
