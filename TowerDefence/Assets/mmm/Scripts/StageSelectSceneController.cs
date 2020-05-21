using UnityEngine;
using UnityEngine.SceneManagement;
public class StageSelectSceneController : MonoBehaviour
{
    public void ToTitleFromStageSelect()
    {
        SceneManager.LoadScene("TitleScene");
    }

    public void ToGameFromStageSelect()
    {
        SceneManager.LoadScene("GameScene");
    }
}
