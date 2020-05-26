using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class EditSelectSceneManager : MonoBehaviour
{

    public SpriteGenerator[] spriteGenerator = new SpriteGenerator[3];


    //編成データ読み書き用
    PrefsManager prefs = new PrefsManager();
    //データ
    private Formation[] formation = new Formation[3];

    private Formation formTmp;


    private int m_ownFormationNum;


    // Start is called before the first frame update
    void Start()
    {
        BGMManager.instance.Play("エディット");

        m_ownFormationNum = 1;


        for (int i = 0; i < 3; i++)
        {
            formation[i] = new Formation();
            formation[i] = prefs.GetFormation((int)i + 1);
            spriteGenerator[i].GenerateSprite(formation[i]);
        }

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
