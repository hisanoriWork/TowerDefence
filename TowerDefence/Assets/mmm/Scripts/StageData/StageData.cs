using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Scriptable/StageData")]
public class StageData : ScriptableObject
{
    [Tooltip("ステージ番号")] public int stageNum = 1;
    [Tooltip("対戦相手の画像")] public Sprite preViewSprite;
    [Tooltip("詳細")] [TextArea] public string detailContent = "ステージのしょうさい";
    [Tooltip("難易度")] public int difficulty = 1;
    [Tooltip("編成情報")] [SmartArray] public int[] gridInfo;
    [Tooltip("編成の舟のID")] public int shipType = 10010;

    public Formation GetFormation()
    {
        Formation formation = new Formation();
        //マジックナンバーすみません
        for (int i = 0; i < 10; i++) for (int j = 0; j < 10; j++)
            {
                formation.gridinfo[i, j] = gridInfo[10 * i + j];
            }
        formation.shiptype = shipType;
        formation.formationDataExists = true;
        return formation;
    }
}