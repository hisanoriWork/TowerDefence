using UnityEngine;
using UnityEngine.SceneManagement;
public class StageSelectSceneController : MonoBehaviour
{
    public void ToTitleFromStageSelect()
    {
        SEManager.instance.Play("シーン遷移");
        if (PlayerPrefs.GetString("DirectToStageSelect", "FromTitle").Equals("FromTitle"))
        {
            SceneManager.LoadScene("TitleScene");
        }
        else
        {
            SceneManager.LoadScene("OnlineEntranceScene");
        }
    }

    public void ToGameFromStageSelect()
    {
        SEManager.instance.Play("シーン遷移");
        SceneManager.LoadScene("GameScene");
    }
}
