using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using NCMB;

[CreateAssetMenu(menuName = "Scriptable/StageData")]
public class StageData : ScriptableObject
{
    public int ID;
    public string uuid = "";
    public string userId = "";
    public int password = 1;
    [TextArea] public new string name;
    [TextArea] public string detailContent = "";
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
    public void Init(int ID, string name, string detailContent, int[] gridInfo, int shipInfo, int password = 1)
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
        Init(ID, name, detailContent, grid, formation.shiptype, password);
    }
    public int CalDifficulty()
    {
        int difficulty = 1;

        if ( winCount == 0)
        {
            return 1 ;
        } else if ( winCount < 3)
        {
            if ( winCount + loseCount < 3)
            {
                difficulty = 2;
            } else
            {
                return 2;
            }
        } else if ( winCount < 6)
        {
            difficulty = 3;
        } else
        {
            difficulty = 4;
        }

        int winPercentage = 100 * (winCount) / (winCount + loseCount);
        if ( winPercentage < 30 )
        {
            return difficulty;
        } else if ( winPercentage < 50 )
        {
            difficulty += 1;
        } else if ( winPercentage < 70 )
        {
            difficulty += 2;
        } else
        {
            difficulty += 3;
        }

        return difficulty;
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

    public void UpdateStageResult(bool isEnemyWin)
    {
        Debug.Log("OOO:" + this.uuid);

        if (!this.uuid.Equals(""))
        {
            NCMBObject data = new NCMBObject(NCMBDatabase.ONLINE_STAGE_DATA)
            {
                ObjectId = this.uuid
            };
            Debug.Log("UUU");

            data.FetchAsync((NCMBException e) =>
            {
                Debug.Log("aa");
                if (e != null)
                {
                    Debug.Log("Update Error!");
                }
                else
                {
                    if (isEnemyWin )
                    {
                        data.Increment("winCount");
                    } else
                    {
                        data.Increment("loseCount");
                    }

                    int winCount = System.Convert.ToInt32(data["winCount"]);
                    int loseCount = System.Convert.ToInt32(data["loseCount"]);
                    data["winPercentage"] = System.Convert.ToInt32( 100 * winCount / (winCount + loseCount));
                    data.SaveAsync();
                }
            });
        }
    }
}