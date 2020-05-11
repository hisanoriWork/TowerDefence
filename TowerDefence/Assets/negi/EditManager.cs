using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class EditManager : MonoBehaviour
{


    /*****public field*****/
    public MasterDataScript masterData;

    public List<GameObject> switchableUnitTypes;

    public GameObject selectableUnitOrigin;

    public Sprite testSprite;

    //private UnitType unitType;

    public Dictionary<UnitType, List<GameObject>> selectableUnits = new Dictionary<UnitType, List<GameObject>>();

    public GameObject movingUnitObject;
    public MovingUnit movingUnit;

    /*****private field*****/
    private Dictionary<UnitType, string> type = new Dictionary<UnitType, string>()
    {
        {UnitType.Pngn, "PngnUnitType"},
        {UnitType.Ship, "ShipUnitType"},
        {UnitType.Block,"BlockUnitType" }
    };

    private List<UnitData> pngnDataList;
    private List<ShipData> shipDataList;
    private List<UnitData> blockDataList;


    void Awake()
    {

        //SelectableContentsの取得]
        pngnDataList = masterData.pngnDataList;
        createSelectableUnits(UnitType.Pngn, masterData.pngnDataList);

        blockDataList = masterData.blockDataList;
        createSelectableUnits(UnitType.Block, blockDataList);

        shipDataList = masterData.shipDataList;
        createSelectableUnits(UnitType.Ship, shipDataList);


        //SelectableContentsの実体化(船)
        SwitchUnitType(UnitType.Ship);


        //FormationWindowの内容取得実体化

        //Cursor非表示
        movingUnitObject.SetActive(false);
    }

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
    }

    private void createSelectableUnits(UnitType unitType, List<UnitData> unitDataList)
    {
        //Pngn Block用
        selectableUnits.Add(unitType, new List<GameObject>());
        foreach (UnitData data in unitDataList)
        {
            var obj = Instantiate(selectableUnitOrigin, GetUnitType(unitType).transform) as GameObject;
            obj.name = data.ID.ToString();
            obj.transform.Find("Image").GetComponent<Image>().sprite = data.sprite;
            obj.transform.Find("UnitName").GetComponent<Text>().text = data.name;
            obj.transform.Find("UnitDetails").GetComponent<Text>().text = "Power:" + data.power + "   HP:" + data.HP + "\nCT:" + data.CT + "   cost:" + data.cost;
            selectableUnits[unitType].Add(obj);
        }


        return;
    }

    private void createSelectableUnits(UnitType unitType, List<ShipData> unitDataList)
    {
        //Ship用
        selectableUnits.Add(unitType, new List<GameObject>());
        foreach (ShipData data in unitDataList)
        {
            var obj = Instantiate(selectableUnitOrigin, GetUnitType(unitType).transform) as GameObject;
            obj.name = data.unitData.ID.ToString();
            obj.transform.Find("Image").GetComponent<Image>().sprite = data.unitData.sprite;
            obj.transform.Find("UnitName").GetComponent<Text>().text = data.name;
            obj.transform.Find("UnitDetails").GetComponent<Text>().text 
                = "Power:" + data.unitData.power + "   HP:" + data.unitData.HP + "\nCT:" + data.unitData.CT + "   cost:" + data.unitData.cost;
            selectableUnits[unitType].Add(obj);
        }

        return;
    }

    public GameObject GetUnitType(UnitType unitType)
    {
        string unitTypeName = type[unitType];
        foreach (GameObject switchableUnitType in switchableUnitTypes)
        {
            if (switchableUnitType.name == unitTypeName) return switchableUnitType;
        }
        Debug.Log("名前が" + unitTypeName + "であるPngnは存在しません");
        return null;
    }

    public void SwitchPngnType()
    {
        SwitchUnitType(UnitType.Pngn);
    }

    public void SwitchShipType()
    {
        SwitchUnitType(UnitType.Ship);
    }

    public void SwitchBlockType()
    {
        SwitchUnitType(UnitType.Block);
    }

    public void SwitchUnitType(UnitType unitType)
    {
        string unitTypeName = type[unitType];
        foreach (GameObject switchableUnitType in switchableUnitTypes)
        {
            if (switchableUnitType.name.ToString() == unitTypeName)
            {            
                switchableUnitType.SetActive(true);
            }
            else
            {
                switchableUnitType.SetActive(false);
            }
        }
        return;
    }

    public void CreateInstance(GameObject gameObject)
    {
        movingUnitObject.SetActive(true);
        movingUnit.movingUnitID = int.Parse(gameObject.name);
        movingUnitObject.GetComponent<Image>().sprite = gameObject.transform.Find("Image").GetComponent<Image>().sprite;
        movingUnitObject.transform.position = Input.mousePosition;

        return;
    }


}
