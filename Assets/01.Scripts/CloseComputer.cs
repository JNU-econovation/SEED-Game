using TMPro;
using UnityEngine;

public class CloseComputer : MonoBehaviour
{
    public GameObject Computer;
    public GameObject SigninPanel;
    public GameObject ComputerBGPanel;
    public GameObject TextFileContent;

    public bool AlreadySignIn = false;

    public bool IsActive()
    {
        return Computer.activeSelf;
    }
    
    public void Toggle()
    {
        if (Computer != null && Computer.activeSelf && AlreadySignIn)
        {
            Time.timeScale = 1f;
            Computer.SetActive(false);
            SigninPanel.SetActive(false);
            ComputerBGPanel.SetActive(true);
            TextFileContent.SetActive(false);
        }
        else if (Computer != null && Computer.activeSelf && !AlreadySignIn)
        {
            Time.timeScale = 1f;
            Computer.SetActive(false);
            SigninPanel.SetActive(true);
            ComputerBGPanel.SetActive(false);
            TextFileContent.SetActive(false);
        }
    }

    public void CloseComputerX()
    {
        if (Computer != null && Computer.activeSelf && AlreadySignIn)
        {
            Time.timeScale = 1f;
            Computer.SetActive(false);
            SigninPanel.SetActive(false);
            ComputerBGPanel.SetActive(true);
            TextFileContent.SetActive(false);
        }
        else if (Computer != null && Computer.activeSelf && !AlreadySignIn)
        {
            Time.timeScale = 1f;
            Computer.SetActive(false);
            SigninPanel.SetActive(true);
            ComputerBGPanel.SetActive(false);
            TextFileContent.SetActive(false);
        }
    }
}
