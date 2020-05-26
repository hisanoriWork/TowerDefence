using System.Collections.Generic;
using UnityEngine;

public class InstDataScript : MonoBehaviour
{
    public List<UnitInst> unitList = new List<UnitInst>();
    public List<UnitInst> pngnList = new List<UnitInst>();
    public ShipInst ship = new ShipInst();
    public PlayerNum playerNum;
    public Transform place;
    public Transform missilePlace;
}

public class UnitInst
{
    public GameObject obj;
    public UnitScript script;
}
public class ShipInst
{
    public GameObject obj;
    public UnitScript unitScript;
    public ShipScript shipScript;
}

public enum PlayerNum
{
    Player1,
    Player2,
}
