using UnityEngine;
using UnityEngine.SceneManagement;

public class StartMenuController : MonoBehaviour
{
    public void StartGame()
    {
        SceneManager.LoadScene("MapScene");
    }

    public void ExitGame()
    {
        Application.Quit();
    }
}