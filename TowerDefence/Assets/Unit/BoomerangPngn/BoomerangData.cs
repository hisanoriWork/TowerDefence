using UnityEngine;

[CreateAssetMenu(menuName = "Scriptable/Weapon/BoomerangData")]
public class BoomerangData : ScriptableObject//飛び道具データ
{
    public AnimationCurve xCurve, yCurve;
    public float time;
    public float dx,dy;
}

