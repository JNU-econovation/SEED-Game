using UnityEngine;
using UnityEngine.SceneManagement;

public class StartMenuController : MonoBehaviour
{
    public void StartGame()
    {
        SceneManager.LoadScene("BossScene");
    }

    public void ExitGame()
    {
        Application.Quit();
    }
}