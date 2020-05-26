using UnityEngine;

[CreateAssetMenu(menuName = "Scriptable/Weapon/BombData")]
public class BombData : ScriptableObject//飛び道具データ
{
    [Range(0, 90)] public float angle;//角度
    [Range(0, 30)] public float deviation;//偏差
    [Range(0, 10)] public float speed;//速度
    [Range(0, 10)] public float gravity;//重力
    [Range(0, 10)] public float explosionTime;
    [Range(0, 1000)] public float explosionInitialSize;
    [Range(0, 1000)] public float explosionFinalSize;
}
