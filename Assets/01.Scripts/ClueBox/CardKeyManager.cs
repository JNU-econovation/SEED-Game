using UnityEngine;

public class CardKeyManager : MonoBehaviour
{
    public static CardKeyManager Instance;

    public bool hasCardKey = false;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }
}