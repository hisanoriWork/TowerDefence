using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PostButtonMessageWindow : MonoBehaviour
{
    public LoadPostFormationToServerSceneManager loadPostFormationToServerSceneManager;


    public GameObject postButtonMessageWindowObject;


    // Start is called before the first frame update
    void Start()
    {
        postButtonMessageWindowObject.SetActive(false);
    }


    public void ShowPostButtonMessageWindow()
    {
        SEManager.instance.Play("セレクト");
        postButtonMessageWindowObject.SetActive(true);
        return;
    }


    public void Yes()
    {
        SEManager.instance.Play("決定");
        loadPostFormationToServerSceneManager.PostData();
        postButtonMessageWindowObject.SetActive(false);
        return;
    }

    public void No()
    {
        SEManager.instance.Play("キャンセル");
        postButtonMessageWindowObject.SetActive(false);
        return;
    }
}
