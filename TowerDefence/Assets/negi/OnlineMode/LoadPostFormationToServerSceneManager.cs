using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;


public class LoadPostFormationToServerSceneManager : MonoBehaviour
{

    //実際に格納する用(他に格納場所があるならここの部分のものをそれに差し替えて)
    int inputOwnFormationNum;
    string inputDetailContentText;
    int inputDifficultySlider;



    //入力された情報を読み込む用
    public Text detailContentText;

    public Slider difficultySlider;
    public Text difficultySliderShowingText;




    //編成選ぶ用(mmmからコピー)
    public int ownFormationNum = 1;
    public GameObject[] slotBtns;
    public Sprite[] buttonSprites;
    public Sprite[] selectButtonSprites;



    //Dialogと意思確認用のプレハブ
    public DialogManager dialogManager;


    // Start is called before the first frame update
    void Start()
    {
        //BGMManager.instance.SetVolume(1);
        //BGMManager.instance.Play("タイトル");
        BGMManager.instance.Play("タイトル");

        ownFormationNum = int.Parse(PlayerPrefs.GetString("ownFormationNum", "1"));
        if (ownFormationNum <= slotBtns.Length + 1)
        {
            for (int i = 0; i < slotBtns.Length; i++)
            {
                var m_Image = slotBtns[i].GetComponent<Image>();
                m_Image.sprite = buttonSprites[i];
                if (ownFormationNum == i + 1)
                {
                    m_Image.sprite = selectButtonSprites[i];
                }
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }





    public void ChangeSlot(GameObject button)
    {
        for (int i = 0; i < slotBtns.Length; i++)
        {
            var m_Image = slotBtns[i].GetComponent<Image>();
            m_Image.sprite = buttonSprites[i];
            if (slotBtns[i] == button)
            {
                SEManager.instance.Play("セレクト");
                m_Image.sprite = selectButtonSprites[i];
                inputOwnFormationNum = (i + 1);
            }
        }
    }

    public void ChangeDifficultySlider()
    {
        difficultySliderShowingText.text = difficultySlider.value.ToString();
    }



    public void PostData()
    {

        SEManager.instance.Play("決定");

        //選択された編成を取得
        //もうinputOwnFormationNumに入っているはず




        //記入された詳細を取得

        //detailContentTextに格納されている

        inputDetailContentText = detailContentText.text;


        Debug.Log(inputDetailContentText);
        if (inputDetailContentText == "")
        {
            dialogManager.ShowDialog("テキストが空です");
            return;

        }



        //選択された難易度を取得

        inputDifficultySlider = (int)difficultySlider.value;





        //IDを振り当てる？なんかしてユニークなIDを作成






        //通信してサーバーに保存させる

        //失敗したらDialog出す
        //dialogManager.ShowDialog("投稿に失敗しました・・・");

        return;
    }





    public void LoadOnlineEntranceScene()
    {
        SEManager.instance.Play("シーン遷移");
        SceneManager.LoadScene("OnlineEntranceScene");
    }
}
