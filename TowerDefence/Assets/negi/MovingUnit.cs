using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovingUnit : MonoBehaviour
{
    //*****************スプライトにアタッチしたまま！！！！！！！！！！************


    public FormationGridManager formationGridManager;
    public int movingUnitID;
    public UnitType movingUnitType;


    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        this.transform.position = Input.mousePosition;
        if (Input.GetMouseButtonUp(0)) setUnit();
    }


    public void setUnit()
    {
        formationGridManager.Attach(movingUnitID,movingUnitType);
        this.gameObject.SetActive(false);
        return;
    }
}
