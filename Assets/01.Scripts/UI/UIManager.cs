using UnityEngine;

public class UIManager : MonoBehaviour
{
    [SerializeField] private InGameMenuController Menu;
    [SerializeField] private ChangeUICanvas UICanvas;
    [SerializeField] private CloseComputer Computer;
    
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (!Computer.IsActive() && !UICanvas.IsActive())
            {
                Menu.Toggle();
            }
            if (Computer.IsActive())
            {
                Computer.Toggle();    
            }

            if (UICanvas.IsActive())
            {
                UICanvas.Toggle();
            }

        }
        if (!Menu.IsActive() && Input.GetKeyDown(KeyCode.Q))
        {
            UICanvas.Toggle();
        }
    }
}
