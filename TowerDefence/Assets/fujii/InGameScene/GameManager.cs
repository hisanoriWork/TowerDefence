using MyLibrary;
using System;
using System.Linq;
using UniRx;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
public class GameManager : MonoBehaviour
{
    /*****public field*****/
    public bool isPlaying { get; set; } = true;
    public float timeScale { get; set; } = 1.0f;

    /*****private field*****/
    [SerializeField] private UIManager m_option = default;
    [SerializeField] private UIManager m_stop = default, m_play = default, m_X2Play = default, m_X4Play = default;
    [SerializeField] private TimeView m_timeView = default;
    [SerializeField] private InstManager m_player1 = default, m_player2 = default;
    [SerializeField] private Gauge m_player1HP = default, m_player2HP = default;
    [SerializeField] private Text m_victoryCanvas = default;
    [SerializeField] private StageNumManager m_stageNumManager = default;
    private int[,] gird;
    private bool m_isFinished = false;
    /*****Mobehabiour method*****/
    void Awake()
    {
        //オプションを開いたときゲームを停止
        m_option.whenDisplayed.Subscribe(_ =>
        {
            Stop(false);
            SEManager.instance.Pause();
            BGMManager.instance.Pause();
            SEManager.instance.Play("設定");
        });
        m_option.whenHidden.Subscribe(_ =>
        {
            SEManager.instance.Resume();
            BGMManager.instance.Resume();
        });
        //オプションを閉じたとき，もともとゲームを再生していたら再生する
        m_option.whenHidden.Where(_ => isPlaying).Subscribe(_ => Play(timeScale));
        //オプションを閉じたとき，もともとゲームを停止していたら停止する
        m_option.whenHidden.Where(_ => !isPlaying).Subscribe(_ => Stop(true));
        m_stop.whenHidden.Subscribe(_ => { m_play.Display(); Play(1.0f); });
        m_play.whenHidden.Subscribe(_ => { m_X2Play.Display(); Play(2.0f); });
        m_X2Play.whenHidden.Subscribe(_ => { m_X4Play.Display(); Play(4.0f); });
        m_X4Play.whenHidden.Subscribe(_ => { m_stop.Display(); Stop(true); });
    }
    void Start()
    {
        m_player1HP.gaugeMaxValue = m_player2HP.gaugeMaxValue = 1000;
        m_player1HP.maxValue = m_player1HP.value = m_player1.shipHP;
        m_player2HP.maxValue = m_player2HP.value = m_player2.shipHP;
        Play(1.0f);
        BGMManager.instance.Replay("対戦");
    }
    void Update()
    {

        if (isPlaying)
        {
            if (m_player1HP.value != m_player1.shipHP)
                m_player1HP.value = m_player1.shipHP;
            if (m_player2HP.value != m_player2.shipHP)
                m_player2HP.value = m_player2.shipHP;

            int victoryNum = CheckVictory(m_player1.shipHP, m_player2.shipHP);
            if (!m_isFinished && victoryNum > 0)
            {
                m_isFinished = true;
                switch (victoryNum)
                {
                    case 3:
                        m_victoryCanvas.text = "引き分け";
                        BGMManager.instance.Stop();
                        SEManager.instance.Play("引き分け");
                        break;
                    case 2:
                        MasterDataScript.instance.battleStageData.UpdateStageResult(true);
                        m_victoryCanvas.text = "負けた";
                        BGMManager.instance.Stop();
                        SEManager.instance.Play("敗北");
                        break;
                    case 1:
                        if (PlayerPrefs.GetString("DirectToStageSelect", "FromTitle").Equals("FromTitle"))
                        {
                            m_stageNumManager.SetPlayableStageNum(true);
                        }
                        MasterDataScript.instance.battleStageData.UpdateStageResult(false);
                        m_victoryCanvas.text = "勝った";
                        BGMManager.instance.Stop();
                        SEManager.instance.Play("勝利");
                        break;
                }
                Stop(true);
                m_victoryCanvas.transform.parent.gameObject.SetActive(true);
            }
        }
    }
    /*****public method*****/
    public void Play(float timeScale = 1.0f)
    {
        Pauser.Play(timeScale);
        this.timeScale = timeScale;
        isPlaying = true;
        m_timeView.timer.Play();
    }
    public void Stop(bool flag = true)
    {
        Pauser.Pause();
        if (flag)
        {
            this.isPlaying = false;
            this.timeScale = timeScale;
        }
        m_timeView.timer.Stop();
    }
    public void TransitionScene(String sceneName)
    {
        Play(1.0f);
        SEManager.instance.Resume();
        BGMManager.instance.Resume();
        SEManager.instance.Stop();
        BGMManager.instance.Stop();
        SceneManager.LoadScene(sceneName);
    }

    /*****private method*****/
    private int CheckVictory(int shipHP1, int shipHP2)
    {
        /*決まってない：0
         * プレイヤー1の勝利：1
         * プレイヤー2の勝利：2
         * ドロー：3
         */
        if (shipHP1 <= 0 && shipHP2 <= 0) return 3;
        else if (shipHP1 <= 0) return 2;
        else if (shipHP2 <= 0) return 1;

        if (m_timeView.isFinished)
        {
            if (shipHP1 == shipHP2) return 3;
            else if (shipHP1 < shipHP2) return 2;
            else return 1;
        }
        return 0;
    }
}

