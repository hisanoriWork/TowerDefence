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

    public SpriteGenerator spriteGenerator;

    //編成選ぶ用(mmmからコピー)
    public int ownFormationNum = 1;
    public GameObject[] slotBtns;
    public Sprite[] buttonSprites;
    public Sprite[] selectButtonSprites;

    [SerializeField] private NCMBDatabase db = default;

    //Dialogと意思確認用のプレハブ
    public DialogManager dialogManager;

    // Start is called before the first frame update
    void Start()
    {
        //BGMManager.instance.SetVolume(1);
        BGMManager.instance.Play("タイトル");

        ownFormationNum = int.Parse(PlayerPrefs.GetString("ownFormationNum", "1"));
        formation = prefs.GetFormation(ownFormationNum);
        spriteGenerator.GenerateSprite(formation);

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

                ownFormationNum = i+ 1;
                formation = prefs.GetFormation(ownFormationNum);
                spriteGenerator.GenerateSprite(formation);

                PlayerPrefs.SetString("ownFormationNum", ownFormationNum.ToString());
            }
        }
    }



    public void PostData()
    {
        SEManager.instance.Play("決定");
        inputOwnFormationNum = int.Parse(PlayerPrefs.GetString("ownFormationNum", "1"));
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

        db.PostStageData(inputOwnFormationNum, inputNameText, inputDetailContentText, dialogManager);

        return;
    }

    public void LoadOnlineEntranceScene()
    {
        SEManager.instance.Play("シーン遷移");
        SceneManager.LoadScene("OnlineEntranceScene");
    }
}
