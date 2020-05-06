using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum UnitType
{
    Pngn,
    Ship,
    Block,
    Other,
}

//下のように書くことでProject Windowのメニュー画面で作成することができます
[CreateAssetMenu(menuName = "Scriptable/UnitData")]
public class UnitData : ScriptableObject
{
    
    public new string name; //Unitの名前
    public int ID = 0; //Unitを識別するためのID
    [Range(0, 2000)] public int power = 10;　//Unitの攻撃力
    [Range(0, 1000)] public int HP = 100; //Unitの体力
    [Range(-1, 101)] public float CT = 5f; //Unitの攻撃間隔
    [Range(0, 1000)] public int cost = 10; //Unitを設置するためのコスト
    public GameObject prefab; //Unitと対応したプレハブ
    public UnitType unitType;
    public bool pngnCanGetOn = false; //上にペンギンは乗れるかどうか
    public bool blockCanGetOn = false; //上に土台系は乗れるかどうか
    public bool indestructible = false; //破壊不能オブジェクトかどうか
    public List<bool> form; //Unitが編成画面で構成されるときのグリッド単位の形
}