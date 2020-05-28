using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;


public class LoadPostFormationToServerSceneManager : MonoBehaviour
{
    //実際に格納する用(他に格納場所があるならここの部分のものをそれに差し替えて)
    int[] inputGridinfo = new int[100];
    int inputShiptype;

    int inputOwnFormationNum;
    string inputNameText;
    string inputDetailContentText;



    //入力された情報を読み込む用

    PrefsManager prefs = new PrefsManager();
    private Formation formation = new Formation();//マス目部分int[] gridinfo = new int[10,10] ,船部分 int shiptype;

    public Text nameText;
    public Text detailContentText;


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



    public void PostData()
    {




        SEManager.instance.Play("決定");

        //選択された編成を取得
        //もうinputOwnFormationNumに入っているはず

        formation = prefs.GetFormation(inputOwnFormationNum);

        for (int y = 0; y < 10; y++)
        {
            for (int x = 0; x < 10; x++)
            {
                inputGridinfo[10 * y + x] = formation.gridinfo[y, x];
            }
        }

        inputShiptype = formation.shiptype;



        //記入された編成名を取得

        inputNameText = nameText.text;

        if (inputNameText == "")
        {
            dialogManager.ShowDialog("編成名が空です！");
            return;

        }

        if (inputNameText.Length > 15)
        {
            dialogManager.ShowDialog("編成名が長すぎます！");
            return;

        }





        //記入された詳細を取得

        //detailContentTextに格納されている

        inputDetailContentText = detailContentText.text;


        //Debug.Log(inputDetailContentText);
        if (inputDetailContentText == "")
        {
            dialogManager.ShowDialog("説明文が空です");
            return;

        }

        if (inputDetailContentText.Length > 50)
        {
            dialogManager.ShowDialog("説明文が長すぎます！");
            return;

        }



        //IDを振り当てる？なんかしてユニークなIDを作成
        NCMBDatabase db = new NCMBDatabase();

        db.PostStageData(inputOwnFormationNum, inputNameText, inputDetailContentText);





        //ここまででreturnされていなかったら投稿できる環境がそろっている
        //以下でサーバーに投稿してもらって大丈夫です

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
