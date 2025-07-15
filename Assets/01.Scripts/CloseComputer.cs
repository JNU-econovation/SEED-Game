using UnityEngine;

public class CloseComputer : MonoBehaviour
{
    public GameObject Computer;
    public GameObject SigninPanel;
    public GameObject ComputerBGPanel;
    public GameObject TextFileContent;

    public bool AlreadySignIn = false;

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
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
