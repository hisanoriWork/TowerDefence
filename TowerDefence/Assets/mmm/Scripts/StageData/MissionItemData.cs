using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Scriptable/MissionItemData")]
public class MissionItemData : ScriptableObject
{
    public int stageNum = 1;
    public Sprite stageIcon;
    [TextArea] public string title = "ミッションのタイトル";
}
