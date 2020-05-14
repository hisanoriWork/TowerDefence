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
    public InstManager player1, player2;
    public UIManager option;
    public TimeView timeView;
    public MasterDataScript masterData;
    public GameObject winCanvas, loseCanvas, drawCanvas;
    /*****private field*****/
    public Gauge m_player1HP , m_player2HP;
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
        player1.Init(formation);
        player2.Init(formation);
        
        RegardAsFriend(player1);
        RegardAsOpponent(player2);
        //オプションを開いたときゲームを停止
        option.whenDisplayed.Subscribe(_ => Stop(true));
        //オプションを閉じたとき，もともとゲームを再生していたら再生する
        option.whenHidden.Where(_ => isPlaying).Subscribe(_ => Play());
        //オプションを閉じたとき，もともとゲームを停止していたら停止する
        option.whenHidden.Where(_ => !isPlaying).Subscribe(_ => Stop());
    }
    void Start()
    {
        m_player1HP.maxValue = m_player1HP.value = player1.shipHP;
        m_player2HP.maxValue = m_player2HP.value = player1.shipHP;
    }
    void Update()
    {
        if (isPlaying)
        {
            if (m_player1HP.value != player1.shipHP)
                m_player1HP.value = player1.shipHP;
            if (m_player2HP.value != player2.shipHP)
                m_player2HP.value = player2.shipHP;
           
            int victoryNum = CheckVictory(player1.shipHP, player2.shipHP, player1.pngnNum, player2.pngnNum);
            switch (victoryNum)
            {
                case 3:
                    drawCanvas.SetActive(true);
                    break;
                case 2:
                    loseCanvas.SetActive(true);
                    break;
                case 1:
                    winCanvas.SetActive(true);
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
        player1.Play();
        player2.Play();
        timeView.timer.Play();
    }
    public void Stop(bool isPlaing = false)
    {
        this.isPlaying = isPlaying;
        player1.Stop();
        player2.Stop();
        timeView.timer.Stop();
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

        if (timeView.isFinished)
        {
            if (shipHP1 == shipHP2) return 3;
            else if (shipHP1 < shipHP2) return 2;
            else return 1;
        }
        return 0;
    }
    private void RegardAsFriend(InstManager player)
    {
        player.ChangeLayer();
    }
    private void RegardAsOpponent(InstManager player)
    {
        player.Invert(true);
        player.ChangeLayer();
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
