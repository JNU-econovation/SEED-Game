using System;
using UnityEngine;

public class MouseLock : MonoBehaviour
{
    private void Update()
    {
        if (Time.timeScale == 0)
        {
            Cursor.lockState = CursorLockMode.None; 
        }
        else
        {
            Cursor.lockState = CursorLockMode.Locked; 
        }
    }
}
