﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class OnlineEntranceSceneManager : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        //BGMManager.instance.SetVolume(1);
        //BGMManager.instance.Play("タイトル");
        BGMManager.instance.Play("タイトル");
        //PlayerPrefs.SetInt("playableStageNum", 1);
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void LoadTitleScene()
    {
        SEManager.instance.Play("シーン遷移");
        SceneManager.LoadScene("TitleScene");
    }

    public void LoadPostFormationToServerScene()
    {
        SEManager.instance.Play("シーン遷移");
        SceneManager.LoadScene("LoadPostFormationToServerScene");
    }

    public void LoadOnlineRankingScene()
    {
        SEManager.instance.Play("シーン遷移");
        SceneManager.LoadScene("OnlineRankingScene");
    }


    public void LoadOnlineStageSelectScene()
    {
        //本当はオンラインモードでの対戦シーンに遷移する、今の遷移先は仮

        SEManager.instance.Play("シーン遷移");
        PlayerPrefs.SetString("DirectToStageSelect", "FromOnline");
        SceneManager.LoadScene("StageSelectScene");
    }

}
