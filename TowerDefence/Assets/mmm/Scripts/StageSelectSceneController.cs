using UnityEngine;
using UnityEngine.SceneManagement;
public class StageSelectSceneController : MonoBehaviour
{
    public void ToTitleFromStageSelect()
    {
        SEManager.instance.Play("シーン遷移");
        SceneManager.LoadScene("TitleScene");
    }

    public void ToGameFromStageSelect()
    {
        SEManager.instance.Play("シーン遷移");
        SceneManager.LoadScene("GameScene");
    }
}
