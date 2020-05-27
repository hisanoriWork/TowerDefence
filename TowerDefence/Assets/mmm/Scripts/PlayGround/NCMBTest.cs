using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using NCMB;

public class NCMBTest : MonoBehaviour
{
    // NCMBを利用するためのクラス
    private NCMBQuery<NCMBObject> _query;

    void Start()
    {
        //TODO: 削除
        PostStageData(1, "お試し", 1);
    }

    public void PostStageData(int slotNum, string detailContent, int difficulty)
    {
        PrefsManager prefs = new PrefsManager();
        Formation formation = prefs.GetFormation(slotNum);
        if (formation.formationDataExists)
        {
            NCMBObject stageData = new NCMBObject("OnlineStageData");

            // オブジェクトに値を設定
            stageData["detailContent"] = detailContent;

            stageData["gridinfo"] = formation.gridinfo;
            stageData["shipInfo"] = formation.shiptype;
            stageData["difficulty"] = difficulty;

            stageData["winCount"] = 0;
            stageData["loseCount"] = 0;
            //TODO: カラムの追加

            // データストアへの登録
            stageData.SaveAsync((NCMBException e) =>
            {
                if (e != null)
                {
                    //エラー処理
                }
                else
                {
                    //成功時の処理
                }
            });
        }
    }
}
