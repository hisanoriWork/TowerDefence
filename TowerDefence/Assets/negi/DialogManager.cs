using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DialogManager : MonoBehaviour
{
    [SerializeField] public Text dialogText;
    [SerializeField] public Image dialogImage;
    [SerializeField] public GameObject dialogObject;

    [SerializeField] private float displayTime = 10f;
    [SerializeField] private float fadeTime = 5f;

    private float currentRemainTime;

    // Use this for initialization
    void Start()
    {
        currentRemainTime = displayTime;
        dialogObject.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        // 残り時間を更新
        currentRemainTime -= Time.deltaTime;

        if (currentRemainTime <= 0f)
        {
            // 残り時間が無くなったら自分自身を消滅
            dialogObject.SetActive(false);
            return;
        }

        if(currentRemainTime <= fadeTime)
        {
            // フェードアウト
            float alpha = currentRemainTime / fadeTime;
            var color = dialogImage.color;
            color.a = alpha;
            dialogImage.color = color;
        }

    }

    public void ShowDialog(string message)
    {
        dialogObject.SetActive(true);


        dialogText.text = message;

        float alpha = fadeTime / fadeTime;
        var color = dialogImage.color;
        color.a = alpha;
        dialogImage.color = color;

        currentRemainTime = displayTime;

        return;
    }
}
