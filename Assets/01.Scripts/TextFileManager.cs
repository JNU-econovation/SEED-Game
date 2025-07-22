using UnityEngine;

public class TextFileManager : MonoBehaviour
{
    public GameObject TextFileContent;

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (TextFileContent != null && TextFileContent.activeSelf)
            {
                TextFileContent.SetActive(false);
            }
        }
    }

    public void ClickTextFile()
    {
        TextFileContent.SetActive(true);
    }

    public void ClickTextFileContentX()
    {
        if (TextFileContent != null && TextFileContent.activeSelf)
        {
            TextFileContent.SetActive(false);
        }
    }
}
