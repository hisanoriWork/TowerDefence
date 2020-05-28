using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Scriptable/StageData")]
public class StageData : ScriptableObject
{
    public int ID;
    public string uuid ="";
    public int password = 1;
    [TextArea] public new string name;
    [TextArea] public string detailContent = "ステージのしょうさい";
    [SmartArray] public int[] gridInfo;
    public int shipInfo = 10010;
    public int difficulty = 1;
    [System.NonSerialized] public int winCount = 0;
    [System.NonSerialized] public int loseCount = 0;
    private int x = 10, y = 10;

    /***** method*****/
    public static StageData CreateInstance()
    {
        return ScriptableObject.CreateInstance<StageData>();
    }
    public void Init(int ID, string name, string detailContent, int[] gridInfo, int shipInfo,int password = 1)
    {
        this.ID = ID;
        this.name = name;
        this.detailContent = detailContent;
        this.gridInfo = gridInfo;
        this.shipInfo = shipInfo;
        this.password = password;
    }
    public void Init(int ID, string name, string detailContent, Formation formation, int password = 1)
    {
        int[] grid = new int[100];
        for (int i = 0; i < x; i++) for (int j = 0; j < y; j++)
            {
                grid[x * i + j] = formation.gridinfo[i, j];
            }
        Init(ID, name, detailContent,grid, formation.shiptype, password);
    }
    public void SetDifficulty()
    {
        if (winCount == 0 && loseCount == 0)
            difficulty = 1;
        else if (loseCount == 0)
            difficulty = winCount;
        else
            difficulty = winCount / loseCount;
        difficulty = difficulty < 10 ? difficulty : 9;
    }
    public Formation GetFormation()
    {
        Formation formation = new Formation();
        for (int i = 0; i < x; i++) for (int j = 0; j < y; j++)
            {
                formation.gridinfo[i, j] = gridInfo[x * i + j];
            }
        formation.shiptype = shipInfo;
        formation.formationDataExists = true;
        return formation;
    }
}