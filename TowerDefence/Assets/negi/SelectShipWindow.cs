using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SelectShipWindow : MonoBehaviour
{

    public int selectableUnitID = 0;
    public FormationGridManager formationGridManager;
    public GameObject selectShipWindowObject;


    // Start is called before the first frame update
    void Start()
    {
        selectShipWindowObject.SetActive(false);
    }

    public void ShowSelectShipWindow(int selectableUnitID)
    {
        this.selectableUnitID = selectableUnitID;
        selectShipWindowObject.SetActive(true);
        return;
    }


    public  void Yes()
    {
        formationGridManager.SelectEachShip(selectableUnitID);
        selectShipWindowObject.SetActive(false);
    }

    public void No()
    {
        selectShipWindowObject.SetActive(false);
    }
}
