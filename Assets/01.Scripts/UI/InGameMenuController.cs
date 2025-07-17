using UnityEngine;
using UnityEngine.SceneManagement;

public class InGameMenuController : MonoBehaviour
{
    public KeyCode inGameMenuKey = KeyCode.Escape;

    private GameObject gameMenu;
    
    private void Awake()
    {
        gameMenu = gameObject.transform.GetChild(0).gameObject;
        gameMenu.SetActive(false);
    }

    // Update is called once per frame
    private void Update()
    {
        if (Input.GetKeyDown(inGameMenuKey))
        {
            gameMenu.SetActive(!gameMenu.activeSelf);
            Time.timeScale = gameMenu.activeSelf ? 0f : 1f;
        }
    }

    public void Continue()
    {
        gameMenu.SetActive(!gameMenu.activeSelf);
        Time.timeScale = gameMenu.activeSelf ? 0f : 1f;
    }

    public void Restart()
    {
        SceneManager.LoadScene("MapScene");
        Time.timeScale = 1f;
    }

    public void Quit()
    {
        Application.Quit();
    }
    
    
}
