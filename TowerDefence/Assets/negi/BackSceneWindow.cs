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
        backSceneWindowObject.SetActive(true);
        return;
    }


    public void Yes()
    {
        editManager.LoadEditSelectScene();
        backSceneWindowObject.SetActive(false);
        return;
    }

    public void No()
    {
        backSceneWindowObject.SetActive(false);
        return;
    }
}
