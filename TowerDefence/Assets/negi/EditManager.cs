using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class EditManager : MonoBehaviour
{


    /*****public field*****/
    public MasterDataScript masterData;
    public FormationGridManager formationGridManager;
    public EditParam editParam;

    public List<GameObject> switchableUnitTypes;

    public GameObject selectableUnitOrigin;

    public Sprite testSprite;

    //private UnitType unitType;

    public Dictionary<UnitType, List<GameObject>> selectableUnits = new Dictionary<UnitType, List<GameObject>>();

    public GameObject movingUnitObject;
    public MovingUnit movingUnit;

    public Sprite nullSprite;

    public SelectShipWindow selectShipWindow;

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
        CreateSelectableUnits(UnitType.Pngn, masterData.pngnDataList);

        blockDataList = masterData.blockDataList;
        CreateSelectableUnits(UnitType.Block, blockDataList);

        shipDataList = masterData.shipDataList;
        CreateSelectableUnits(UnitType.Ship, shipDataList);


        //SelectableContentsの実体化(船)
        SwitchUnitType(UnitType.Ship);


        //FormationWindowの内容取得実体化

        //Cursor非表示
        movingUnitObject.SetActive(false);

        editParam.deleteButton.SetActive(false);
    }

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
    }


    private void SetSpriteAndResizeImgSize(Transform transform,float imgSize,Sprite sprite)
    {
        
        var img = transform.GetComponent<Image>();
        var t = transform.GetComponent<RectTransform>();

        img.sprite = sprite;
        img.SetNativeSize();

        var width = t.sizeDelta.x * imgSize;
        var height = t.sizeDelta.y * imgSize;

        t.sizeDelta = new Vector2(width, height);

        return;
    }

    private Vector2 ConvertOffsetValue(Vector2 dataOffset)
    {
        return dataOffset * editParam.movingUnitOffset;
    }

    private void CreateSelectableUnits(UnitType unitType, List<UnitData> unitDataList)
    {
        SelectableUnit sel;

        //Pngn Block用
        selectableUnits.Add(unitType, new List<GameObject>());
        foreach (UnitData data in unitDataList)
        {
            var obj = Instantiate(selectableUnitOrigin, GetUnitType(unitType).transform) as GameObject;
            obj.name = data.ID.ToString();

            var transform = obj.transform.Find("Image");
            SetSpriteAndResizeImgSize(transform, editParam.selectableUnitImgSize, data.sprite);

            obj.transform.Find("UnitName").GetComponent<Text>().text = data.name;
            /*obj.transform.Find("UnitDetails").GetComponent<Text>().text 
                = "Power:" + data.power + "   HP:" + data.HP + "\nCT:" + data.CT + "   cost:" + data.cost;*/
            obj.transform.Find("UnitDetails").GetComponent<Text>().text
                = "HP:" + data.HP + "   Cost:" + data.cost;

            sel = obj.GetComponent<SelectableUnit>();
            sel.selectableUnitID = data.ID;
            sel.selectableUnitType = data.unitType;
            sel.selectableUnitForm = data.form;
            sel.selectableUnitEmptyForm = data.emptyForm;
            sel.selectableUnitCost = data.cost;

            sel.selectableUnitOffset = ConvertOffsetValue(data.offset);

            selectableUnits[unitType].Add(obj);
        }


        return;
    }

    private void CreateSelectableUnits(UnitType unitType, List<ShipData> unitDataList)
    {
        SelectableUnit sel;

        //Ship用
        selectableUnits.Add(unitType, new List<GameObject>());
        foreach (ShipData data in unitDataList)
        {
            GameObject obj = Instantiate(selectableUnitOrigin, GetUnitType(unitType).transform) as GameObject;
            obj.name = data.unitData.ID.ToString();

            var transform = obj.transform.Find("Image");
            SetSpriteAndResizeImgSize(transform, editParam.selesctableShipImgSize, data.unitData.sprite);

            obj.transform.Find("UnitName").GetComponent<Text>().text = data.name;
            obj.transform.Find("UnitDetails").GetComponent<Text>().text
                = "HP:" + data.unitData.HP + "   Cost:" + data.unitData.cost;

            sel = obj.GetComponent<SelectableUnit>();
            sel.selectableUnitID = data.unitData.ID;
            sel.selectableUnitType = data.unitData.unitType;
            sel.selectableUnitForm = data.unitData.form;
            sel.selectableUnitEmptyForm = data.unitData.emptyForm;
            sel.selectableUnitCost = data.unitData.cost;

            sel.selectableUnitOffset = ConvertOffsetValue(data.unitData.offset);

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
        SEManager.instance.Play("セレクト");
        SwitchUnitType(UnitType.Pngn);
    }

    public void SwitchShipType()
    {
        SEManager.instance.Play("セレクト");
        SwitchUnitType(UnitType.Ship);
    }

    public void SwitchBlockType()
    {
        SEManager.instance.Play("セレクト");
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

    public void ClickEachSelectableUnit(GameObject selectableUnit)
    {
        SEManager.instance.Play("セレクト");
        SelectableUnit sel = selectableUnit.GetComponent<SelectableUnit>();

        //Debug.Log(sel.selectableUnitType);
        //Debug.Log(UnitType.Pngn);


        //PngnBlockの場合
        if (sel.selectableUnitType == UnitType.Pngn || sel.selectableUnitType == UnitType.Block)
        {
            //まずそれを選択したときにコストが負にならないか判定する
            if (editParam.formationCost - sel.selectableUnitCost >= 0)
            {
                editParam.deleteButton.SetActive(true);

                //Debug.Log(editParam.formationCost - sel.selectableUnitCost);
                movingUnitObject.SetActive(true);

                movingUnit.movingUnitID = sel.selectableUnitID;
                movingUnit.movingUnitType = sel.selectableUnitType;
                movingUnit.movingUnitForm = sel.selectableUnitForm;
                movingUnit.movingUnitEmptyForm = sel.selectableUnitEmptyForm;
                movingUnit.movingUnitOffset = sel.selectableUnitOffset;
                movingUnit.movingUnitCost = sel.selectableUnitCost;
                movingUnit.beforeAttachingUnitPosition = null;

                var transform = movingUnitObject.transform;
                SetSpriteAndResizeImgSize(transform, editParam.movingUnitImgSize, selectableUnit.transform.Find("Image").GetComponent<Image>().sprite);

                movingUnitObject.transform.position = (Vector2)Input.mousePosition + sel.selectableUnitOffset;

                formationGridManager.DisplayEnableGrid(movingUnit.movingUnitID, movingUnit.movingUnitType, movingUnit.movingUnitForm,movingUnit.movingUnitEmptyForm, movingUnit.movingUnitOffset);


            }

        }
        //Shipの場合
        else if (sel.selectableUnitType == UnitType.Ship)
        {
            selectShipWindow.ShowSelectShipWindow(sel.selectableUnitID);
        }

        

        


        return;
    }

    public void ClickEachAttachingUnit(GameObject attachingUnit)
    {
        
        SelectableUnit att = attachingUnit.GetComponent<SelectableUnit>();

        //Debug.Log(sel.selectableUnitID);


        if (att.selectableUnitID != 0)
        {
            //Debug.Log(sel.selectableUnitType);
            //Debug.Log(UnitType.Pngn);


            //PngnBlockの場合
            if (att.selectableUnitType == UnitType.Pngn || att.selectableUnitType == UnitType.Block)
            {

                //そのユニットを外すことが禁則であるとき、外させない
                if (formationGridManager.CheckingCutVertex(att.selectableUnitForm, att.selectableUnitEmptyForm, att.beforeAttachingPosition))
                //↑非常に危険、見直してから使う
                //if(true)
                {

                    //Debug.Log("aho");
                    //Debug.Log(att.beforeAttachingPosition[0] + ":" + att.beforeAttachingPosition[1]);

                    //ここらへん追加しすぎて乱雑なのでリファクタリング必要

                    SEManager.instance.Play("セレクト");

                    editParam.deleteButton.SetActive(true);

                    Image attachingUnitImage = attachingUnit.GetComponent<Image>();

                    movingUnitObject.SetActive(true);

                    movingUnit.movingUnitID = att.selectableUnitID;
                    movingUnit.movingUnitType = att.selectableUnitType;
                    movingUnit.movingUnitForm = att.selectableUnitForm;
                    movingUnit.movingUnitEmptyForm = att.selectableUnitEmptyForm;
                    movingUnit.movingUnitOffset = att.selectableUnitOffset;
                    movingUnit.movingUnitCost = att.selectableUnitCost;
                    movingUnit.beforeAttachingUnitPosition = att.beforeAttachingPosition;

                    editParam.formationCost = editParam.formationCost + att.selectableUnitCost;
                    formationGridManager.UpdateFormationCostDisplay();

                    var transform = movingUnitObject.transform;
                    SetSpriteAndResizeImgSize(transform, editParam.movingUnitImgSize, attachingUnitImage.sprite);


                    attachingUnitImage.sprite = nullSprite;

                    attachingUnit.transform.Find("Text").GetComponent<Text>().text = "0";


                    //var img = transform.GetComponent<Image>();
                    var t = attachingUnitImage.GetComponent<RectTransform>();

                    attachingUnitImage.SetNativeSize();

                    var width = t.sizeDelta.x * editParam.attachingUnitImgSize;
                    var height = t.sizeDelta.y * editParam.attachingUnitImgSize;

                    t.sizeDelta = new Vector2(width, height);

                    attachingUnitImage.transform.position = (Vector2)attachingUnitImage.transform.position - att.selectableUnitOffset * editParam.canvasScale;
                    //attachingUnitImage.rectTransform.position = (Vector2)attachingUnitImage.rectTransform.position - att.selectableUnitOffset * editParam.attachingUnitImgSize;

                    movingUnitObject.transform.position = (Vector2)Input.mousePosition + att.selectableUnitOffset;

                    att.selectableUnitID = 0;

                    att.selectableUnitForm = null;
                    att.selectableUnitEmptyForm = null;
                    att.selectableUnitOffset = new Vector2();
                    att.selectableUnitCost = 0;

                    formationGridManager.DisplayEnableGrid(movingUnit.movingUnitID, movingUnit.movingUnitType, movingUnit.movingUnitForm, movingUnit.movingUnitEmptyForm, movingUnit.movingUnitOffset);
                }


                
            }

            //Shipの場合


        }


        return;
    }

    public void LoadEditSelectScene()
    {
        SceneManager.LoadScene("EditSelectScene");
    }


}
