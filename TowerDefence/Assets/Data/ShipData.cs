using System.Collections;
using System.Collections.Generic;
using UnityEngine;


//下のように書くことでProject Windowのメニュー画面で作成することができます
[CreateAssetMenu(menuName = "Scriptable/ShipData")]
public class ShipData : ScriptableObject
{
    public UnitData unitData;
}