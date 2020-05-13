using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovingUnit : MonoBehaviour
{
    //*****************スプライトにアタッチしたまま！！！！！！！！！！************


    public FormationGridManager formationGridManager;
    public int movingUnitID;
    public UnitType movingUnitType;

    public int[] beforeAttachingUnitPosition;


    // Start is called before the first frame update
    void Start()
    {
        beforeAttachingUnitPosition = null;
    }

    // Update is called once per frame
    void Update()
    {
        this.transform.position = Input.mousePosition;
        if (Input.GetMouseButtonUp(0)) SetUnit();
    }


    public void SetUnit()
    {
        formationGridManager.Attach(movingUnitID, movingUnitType, beforeAttachingUnitPosition);
        beforeAttachingUnitPosition = null;
        this.gameObject.SetActive(false);
        return;
    }
}
