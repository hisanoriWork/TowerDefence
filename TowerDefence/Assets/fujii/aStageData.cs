using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Scriptable/StageData")]
public class aStageData : ScriptableObject
{
    [Tooltip("ステージ番号")] public int stageNum;
    [Tooltip("ステージアイコン")] public Sprite stageIcon;
    [Tooltip("タイトル")] [TextArea] public string title;
    [Tooltip("対戦相手の画像")] public Sprite preViewSprite;
    [Tooltip("対戦相手の編成名")] [TextArea] public new string name;
    [Tooltip("詳細")] [TextArea] public string detailContent;
    [Tooltip("難易度")] public int difficulty = 1;
    [Tooltip("編成情報")][SmartArray] public int[] gridInfo;
    [Tooltip("編成の舟のID")] public int shipType;
}