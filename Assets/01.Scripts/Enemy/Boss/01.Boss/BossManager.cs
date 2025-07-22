using UnityEngine;
using SojaExiles;

public class BossManager : MonoBehaviour
{
    public static BossManager Instance;
    public Transform boss;
    public Vector3 initialBossPosition;
    private Quaternion initialBossRotation;
    public float initialHealth = 500f;

    [SerializeField] private GameObject door1;
    [SerializeField] private GameObject door2;
    [SerializeField] private BossEntranceTrigger entranceTrigger;

    private Animator door1Animator;
    private Animator door2Animator;


    private void Awake()
    {
        if (Instance == null)
            Instance = this;
        else
            Destroy(gameObject);
    }
    private void Start()
    {
        initialBossPosition = boss.position;
        initialBossRotation = boss.rotation;
        door1Animator = door1.GetComponent<Animator>();
        door2Animator = door2.GetComponent<Animator>();
    }

    public void ResetBoss()
    {
        boss.position = initialBossPosition;
        boss.rotation = initialBossRotation;

        boss.GetComponent<EnemyHealth>().ResetHealth();
        boss.GetComponent<EnemyAI>().ResetState();

        CloseDoor();
        entranceTrigger.ResetTrigger();
    }

    public void CloseDoor()
    {
        door1.GetComponent<opencloseDoor1>()?.ForceClose();
        door2.GetComponent<opencloseDoor>()?.ForceClose();
    }
}
