using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Scriptable/StageData")]
public class StageData : ScriptableObject
{
    public int ID;
    public int password = 1;
    [TextArea] public new string name;
    [TextArea] public string detailContent = "ステージのしょうさい";
    [SmartArray] public int[] gridInfo;
    public int shipInfo;
    public int difficulty;
    [System.NonSerialized] public int winCount;
    [System.NonSerialized] public int loseCount;
    public Formation GetFormation()
    {
        Formation formation = new Formation();
        //マジックナンバーすみません
        for (int i = 0; i < 10; i++) for (int j = 0; j < 10; j++)
            {
                formation.gridinfo[i, j] = gridInfo[10 * i + j];
            }
        formation.shiptype = shipInfo;
        formation.formationDataExists = true;
        return formation;
    }
}