﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EditParam : MonoBehaviour
{
    [SerializeField] public float selectableUnitImgSize = 0.6f;
    [SerializeField] public float selesctableShipImgSize = 0.08f;

    [SerializeField] public float movingUnitImgSize = 0.4f;
    [SerializeField] public float movingUnitOffset = 40f;

    [SerializeField] public float attachingUnitImgSize = 0.5f;
    [SerializeField] public float attachingShipImgSize = 0.8f;
    [SerializeField] public Color defaultColor = new Color(255f, 255f, 255f, 255f);
    [SerializeField] public Color unAttachingColor = new Color(255f, 255f, 255f, 255f);
    [SerializeField] public Color AttachingColor = new Color(255f, 255f, 255f, 255f);

    [SerializeField] public Sprite nullSprite;
    [SerializeField] public Sprite attachingSprite;

    [SerializeField] public int formationCostMax= 1000;
    [SerializeField] public int formationCost = 1000;
    public GameObject deleteButton;

    public int ownFormationNum;

    public CanvasScaler canvasScaler;

    public float canvasScale = 1;

    private void Awake()
    {
        ownFormationNum = int.Parse(PlayerPrefs.GetString("ownFormationNum", "1"));
        canvasScale = Screen.height / canvasScaler.referenceResolution.y;
        //Debug.Log(canvasScale);
    }
}
