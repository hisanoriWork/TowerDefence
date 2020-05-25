using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class EditSelectSceneManager : MonoBehaviour
{
    private int m_ownFormationNum;


    // Start is called before the first frame update
    void Start()
    {
        m_ownFormationNum = 1;
        //BGMManager.instance.Play("エディット");
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

    public void SwitchFormation(int import_ownFormationNum)
    {
        SEManager.instance.Play("セレクト");
        m_ownFormationNum = import_ownFormationNum;
        return;
    }

    public void LoadEditScene()
    {
        SEManager.instance.Play("決定");
        PlayerPrefs.SetString("ownFormationNum", m_ownFormationNum.ToString());
        SceneManager.LoadScene("EditScene");
    }
}
