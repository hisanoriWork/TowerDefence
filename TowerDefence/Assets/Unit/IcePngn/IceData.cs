using UnityEngine;

[CreateAssetMenu(menuName = "Scriptable/Weapon/IceData")]
public class IceData : ScriptableObject//飛び道具データ
{
    [Range(0, 90)] public float angle;//角度
    [Range(0, 30)] public float deviation;//偏差
    [Range(0, 10)] public float speed;//速度
    [Range(0, 10)] public float gravity;//重力
}
