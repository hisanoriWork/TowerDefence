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
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void LoadTitleScene()
    {
        SceneManager.LoadScene("TitleScene");
    }

    public void SwitchFormation(int import_ownFormationNum)
    {
        m_ownFormationNum = import_ownFormationNum;
        return;
    }

    public void LoadEditScene()
    {
        PlayerPrefs.SetString("ownFormationNum", m_ownFormationNum.ToString());
        SceneManager.LoadScene("EditScene");
    }
}
