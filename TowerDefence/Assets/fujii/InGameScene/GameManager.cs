using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using UniRx;
public class GameManager : MonoBehaviour
{
    /*****public field*****/

    /*****private field*****/
    [SerializeField] private UIManager m_option;
    [SerializeField] private TimeView m_timeView;
    [SerializeField] private InstManager m_player1, m_player2;
    [SerializeField] private Gauge m_player1HP, m_player2HP;
    [SerializeField] private GameObject m_winCanvas, m_loseCanvas, m_drawCanvas;
    private int[,] gird;
    public bool isPlaying { get; set; } = true;
    /*****Mobehabiour method*****/
    void Awake()
    {
        //PrefsManager prefs = new PrefsManager();
        //Formation formation = prefs.getFormation();
        //gird = formation.girdinfo;
        //下はデバッグ用
        Formation formation = new Formation();
        formation.formationDataExists = true;
        formation.gridinfo = new int[10, 10]
        {
            {  0,  0,  0,  0,  0,  0,  0,  0,  0,  0},
            {  0,  0,  0,  0,  0, 14,100,100, 11, 10},
            {  0,  0,  0,100, 10,  0,100,100,  0,  0},
            { 12,  0, 10,100,  0,  0,100,  0,  0,  0},
            {  0,  0,  0, 11,  0,  0,  0,  0,  0,  0},
            {  0,  0,  0,  0,  0,  0,  0,  0,  0,  0},
            {  0,  0,  0,  0,  0,  0,  0,  0,  0,  0},
            {  0,  0,  0,  0,  0,  0,  0,  0,  0,  0},
            {  0,  0,  0,  0,  0,  0,  0,  0,  0,  0},
            {  0,  0,  0,  0,  0,  0,  0,  0,  0,  0},
        };
        formation.shiptype = 10010;
        m_player1.Init(formation);
        m_player2.Init(formation);
        
        RegardAsFriend(m_player1);
        RegardAsOpponent(m_player2);
        //オプションを開いたときゲームを停止
        m_option.whenDisplayed.Subscribe(_ => Stop(true));
        //オプションを閉じたとき，もともとゲームを再生していたら再生する
        m_option.whenHidden.Where(_ => isPlaying).Subscribe(_ => Play());
        //オプションを閉じたとき，もともとゲームを停止していたら停止する
        m_option.whenHidden.Where(_ => !isPlaying).Subscribe(_ => Stop());
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
    public void Play()
    {
        isPlaying = true;
        m_player1.Play();
        m_player2.Play();
        m_timeView.timer.Play();
    }
    public void Stop(bool isPlaing = false)
    {
        this.isPlaying = isPlaying;
        m_player1.Stop();
        m_player2.Stop();
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
    private void RegardAsFriend(InstManager m_player)
    {
        m_player.ChangeLayer();
    }
    private void RegardAsOpponent(InstManager m_player)
    {
        m_player.Invert(true);
        m_player.ChangeLayer();
    }
}
/*****public class*****/
public class InfoToWeapon
{
    public Vector3 pos;
    public string layer;
    public int power;
    public InfoToWeapon(Vector3 pos,string layer,int power)
    {
        this.pos = pos;
        this.layer = layer;
        this.power = power;
    }

}
public class Utility
{
    public static void SetLayerRecursively(GameObject self, string layerName)
    {
        int layer = LayerMask.NameToLayer(layerName);
        SetLayerRecursively(self, layer);
    }

    public static void SetLayerRecursively(GameObject self, int layer)
    {
        self.layer = layer;

        foreach (Transform n in self.transform)
        {
            SetLayerRecursively(n.gameObject, layer);
        }
    }

    public static IEnumerator WaitForSecond(float time, UnityEvent voidEvent)
    {
        yield return new WaitForSeconds(time);
        voidEvent.Invoke();
    }
}
