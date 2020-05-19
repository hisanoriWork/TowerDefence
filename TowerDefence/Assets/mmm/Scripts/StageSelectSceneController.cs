using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
public class StageSelectSceneController : MonoBehaviour
{
    // TODO: 遷移先変更
    public void ToTitleFromStageSelect()
    {
        SceneManager.LoadScene("TitleScene");
    }

    // TODO: 遷移先変更&引数渡し
    public void ToGameFromStageSelect()
    {
        SceneManager.LoadScene("GameScene");
    }
}
