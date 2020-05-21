﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StageItemListener : MonoBehaviour
{
    public int stageNum = 0;
    private StageSelectManager stageSelectManager;

    void Start()
    {
        stageSelectManager = GameObject.Find("StageSelectManager").GetComponent<StageSelectManager>();
    }
    public void ClickOnListener()
    {
        if (stageSelectManager != null)
        {
            stageSelectManager.ChangeSelectStage(stageNum);
            Debug.Log("ステージ番号:" + stageNum);
        }
        else
        {
            Debug.Log("stageSelectManagerのインスタンスが存在しません");
        }
    }
}
