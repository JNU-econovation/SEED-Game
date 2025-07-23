using System;
using UnityEngine;
using System.Collections;

public class PlayerHealth : MonoBehaviour
{
    [SerializeField] private float maxHealth = 100f;
    [SerializeField] private float currentHealth;
    [SerializeField] private HealthBar_ES playerHealthUI;

    [SerializeField] private Animator animator;

    private ClueBox clueBox;
    private PlayerStun stun;
    public bool enableStun = true;
    private bool isDead = false;
    public bool IsDead => isDead;
    private float GotoClubTime = 3f;


    private void Awake()
    {
        stun = GetComponent<PlayerStun>();
        currentHealth = maxHealth;
        clueBox = GetComponentInChildren<ClueBox>();
    }

    private void OnTriggerEnter(Collider other)
    {
        Weapon weapon = other.GetComponentInParent<Weapon>();
        if (weapon == null) return;
        if (weapon.tag != "EnemyWeapon") return;

        float damage = weapon.Damage;
        TakeDamage(damage);
    }

    public void TakeDamage(float damage)
    {
        if (isDead) return;

        Debug.Log("플레이어 피격 데미지: " + damage);
        currentHealth -= damage;
        currentHealth = Mathf.Clamp(currentHealth, 0f, maxHealth);

        if (enableStun)
            stun.ApplyStun(.5f);

        AudioManager.Instance.PlayPlayerHit();

        playerHealthUI.SetHealth(currentHealth);

        if (currentHealth <= 0.0001f)
        {
            currentHealth = 0f; // 확실히 0으로 고정
            Die();
        }
    }

    private void Die()
    {
        if (isDead) return; // 이미 죽었으면 다시 실행 X

        isDead = true;
        Debug.Log("플레이어 사망");

        animator.SetTrigger("IsDead");

        clueBox.LoseClue();

        var moveScript = GetComponent<PlayerMovement>();
        moveScript.isDead = true;

        moveScript.enabled = false;
        GetComponent<Rigidbody>().isKinematic = true;

        StartCoroutine(gotoClub(GotoClubTime));

    }
    IEnumerator gotoClub(float GotoClubTime)
    {
        yield return new WaitForSeconds(GotoClubTime);
        currentHealth = 100f;
        playerHealthUI.SetHealth(currentHealth);
        transform.position = new Vector3(6f, 0f, 12f);
        isDead = false;
        animator.SetTrigger("Idle");

        var moveScript = GetComponent<PlayerMovement>();
        moveScript.isDead = false;
        moveScript.enabled = true;
        GetComponent<Rigidbody>().isKinematic = false;
        AudioManager.Instance.PlayBGM();
        BossManager.Instance.ResetEverything();
    }


    public void Heal(float amount)
    {
        currentHealth += amount;
        currentHealth = Mathf.Clamp(currentHealth, 0f, maxHealth);
        playerHealthUI.SetHealth(currentHealth);
    }

    void Start()
    {
        playerHealthUI.Init(maxHealth);
    }

    void Update()
    {
        
    }
}
