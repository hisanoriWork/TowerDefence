using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SelectableUnit : MonoBehaviour
{
    public int selectableUnitID;
    public UnitType selectableUnitType;
    public GridForm[] selectableUnitForm;
    public Vector2 selectableUnitOffset;

    public int[] beforeAttachingPosition;
}
