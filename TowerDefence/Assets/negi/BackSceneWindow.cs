using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BackSceneWindow : MonoBehaviour
{
    public GameObject backSceneWindowObject;
    public EditManager editManager;


    // Start is called before the first frame update
    void Start()
    {
        backSceneWindowObject.SetActive(false);
    }


    public void ShowBackSceneWindow()
    {
        SEManager.instance.Play("セレクト");
        backSceneWindowObject.SetActive(true);
        return;
    }


    public void Yes()
    {
        SEManager.instance.Play("シーン遷移");
        editManager.LoadEditSelectScene();
        backSceneWindowObject.SetActive(false);
        return;
    }

    public void No()
    {
        SEManager.instance.Play("キャンセル");
        backSceneWindowObject.SetActive(false);
        return;
    }
}
