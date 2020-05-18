using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovingUnit : MonoBehaviour
{

    public FormationGridManager formationGridManager;
    public int movingUnitID;
    public UnitType movingUnitType;
    public GridForm[] movingUnitForm;
    public Vector2 movingUnitOffset;
    public int movingUnitCost;

    public int[] beforeAttachingUnitPosition;


    // Start is called before the first frame update
    void Start()
    {
        beforeAttachingUnitPosition = null;
    }

    // Update is called once per frame
    void Update()
    {
        this.transform.position = (Vector2)Input.mousePosition + movingUnitOffset;
        if (Input.GetMouseButtonUp(0)) SetUnit();
    }


    public void SetUnit()
    {
        formationGridManager.HideEnableGrid();
        formationGridManager.Attach(movingUnitID, movingUnitType,movingUnitForm,movingUnitOffset, beforeAttachingUnitPosition);
        beforeAttachingUnitPosition = null;
        this.gameObject.SetActive(false);
        return;
    }
}
