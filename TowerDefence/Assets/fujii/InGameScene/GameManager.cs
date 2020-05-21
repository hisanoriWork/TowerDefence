using System;
using System.Collections;
using System.Linq;
using UniRx;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;
public class GameManager : MonoBehaviour
{
    /*****public field*****/
    public bool isPlaying { get; set; } = true;
    public float timeScale { get; set; } = 1.0f;
    /*****private field*****/
    [SerializeField] private UIManager m_option = default;
    [SerializeField] private UIManager m_stop, m_play, m_X2Play, m_X4Play;
    [SerializeField] private TimeView m_timeView = default;
    [SerializeField] private InstManager m_player1 = default, m_player2 = default;
    [SerializeField] private Gauge m_player1HP = default, m_player2HP = default;
    [SerializeField] private GameObject m_winCanvas = default, m_loseCanvas = default, m_drawCanvas = default;
    private int[,] gird;
    /*****Mobehabiour method*****/
    void Awake()
    {
        //オプションを開いたときゲームを停止
        m_option.whenDisplayed.Subscribe(_ => Stop(false));
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
        m_player1HP.maxValue = m_player1HP.value = m_player1.shipHP;
        m_player2HP.maxValue = m_player2HP.value = m_player1.shipHP;
    }
    void Update()
    {
        if (isPlaying)
        {
            if (m_player1HP.value != m_player1.shipHP)
                m_player1HP.value = m_player1.shipHP;
            if (m_player2HP.value != m_player2.shipHP)
                m_player2HP.value = m_player2.shipHP;
           
            int victoryNum = CheckVictory(m_player1.shipHP, m_player2.shipHP, m_player1.pngnNum, m_player2.pngnNum);
            switch (victoryNum)
            {
                case 3:
                    m_drawCanvas.SetActive(true);
                    break;
                case 2:
                    m_loseCanvas.SetActive(true);
                    break;
                case 1:
                    m_winCanvas.SetActive(true);
                    break;
                default:
                    break;
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
        SceneManager.LoadScene(sceneName);
    }
    /*****private method*****/
    private int CheckVictory(int shipHP1 ,int shipHP2,int pngnNum1,int pngnNum2)
    {
        /*決まってない：0
         * プレイヤー1の勝利：1
         * プレイヤー2の勝利：2
         * ドロー：3
         */
        if (pngnNum1 == 0 && pngnNum2 == 0) return 3;
        else if (pngnNum1 == 0) return 2;
        else if (pngnNum1 == 0) return 1;

        if (shipHP1 == 0 && shipHP2 == 0) return 3;
        else if (shipHP1 == 0) return 2;
        else if (shipHP2 == 0) return 1;

        if (m_timeView.isFinished)
        {
            if (shipHP1 == shipHP2) return 3;
            else if (shipHP1 < shipHP2) return 2;
            else return 1;
        }
        return 0;
    }
}
/*****public class*****/

