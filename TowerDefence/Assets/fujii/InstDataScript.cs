using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InstDataScript : MonoBehaviour
{
    public List<UnitInst> unitList;
    public List<UnitInst> pngnList;
    public ShipInst ship;
    public string pngnLayer;
    public string shipLayer;
    public string weaponLayer;
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
