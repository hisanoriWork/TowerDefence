using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BackFormationWindow: MonoBehaviour
{
    public GameObject backFormationWindowObject;
    public FormationGridManager formationGridManager;


    // Start is called before the first frame update
    void Start()
    {
        backFormationWindowObject.SetActive(false);
    }


    public void ShowBackFormationWindow()
    {
        SEManager.instance.Play("セレクト"); 
        backFormationWindowObject.SetActive(true);

        return;
    }


    public void Yes()
    {
        formationGridManager.BackToSaveFormation();
        backFormationWindowObject.SetActive(false);
        return;
    }

    public void No()
    {
        SEManager.instance.Play("キャンセル");
        backFormationWindowObject.SetActive(false);
        return;
    }
}
