using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.SceneManagement;

public class InGameMenuController : MonoBehaviour
{
    private GameObject gameMenu;

    private void Awake()
    {
        gameMenu = gameObject.transform.GetChild(0).gameObject;
        gameMenu.SetActive(false);
    }

    public bool IsActive()
    {
        return gameMenu.activeSelf;
    }
    
    public void Toggle()
    {
        gameMenu.SetActive(!gameMenu.activeSelf);
        Time.timeScale = gameMenu.activeSelf ? 0f : 1f;
    }

    public void Continue()
    {
        gameMenu.SetActive(!gameMenu.activeSelf);
        Time.timeScale = 1f;
    }

    public void Restart()
    {
        SceneManager.LoadScene("BossScene");
        AudioManager.Instance.RestoreSFXVolumes();
        AudioManager.Instance.PlayBGM();
        Time.timeScale = 1f;
    }

    public void Quit()
    {
        Application.Quit();
    }
    
    
}
