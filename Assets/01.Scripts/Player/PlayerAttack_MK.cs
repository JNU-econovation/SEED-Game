using System.Collections;
using UnityEngine;

public class PlayerAttack_MK : MonoBehaviour
{
    [SerializeField] private Animator animator;
    [SerializeField] private GameObject weapon;
    [SerializeField] private Transform weaponResetPoint;

    private Weapon weaponComponent;
    private float attackCooldown;
    private PlayerMovement movement;
    private PlayerHealth health;

    private GameObject spawnedModel;
    private GameObject spawnedModelPrefab; // 풀 반환 시 원본 프리팹 참조 보관

    // pencilcase 관련
    public float speed = 10f;
    public float lifeTime = 1f;
    public bool isAttacking = false;
    private bool isThrown = false;
    private float throwTimer = 0f;

    // laptop 관련
    public bool isSmashing = false;

    // mouse 관련
    public bool isThrusting = false;

    // beamProjector 관련
    public bool isCoding = false;

    private void Awake()
    {
        weaponComponent = weapon.GetComponent<Weapon>();
        attackCooldown = weaponComponent.AttackSpeed;
        health = GetComponent<PlayerHealth>();
    }

    void Start()
    {
        movement = GetComponent<PlayerMovement>();
        ResetWeapon();
    }

    void Update()
    {
        if (health.IsDead) return;
        if (movement != null && movement.isRolling) return;
        if (Time.timeScale == 0f) return;

        // 맨손 공격
        if (Input.GetMouseButtonDown(0) && !isAttacking && weaponComponent.currentAttackInfo == weaponComponent.attackInfosList[0])
        {
            isAttacking = true;
            animator.SetTrigger("Attack");
            AudioManager.Instance.PlayPlayerAttack(0);
            StartCoroutine(Attack());
        }

        // 연필 던지기
        if (Input.GetMouseButtonDown(0) && !isThrown && weaponComponent.currentAttackInfo == weaponComponent.attackInfosList[1])
        {
            ThrowWeapon();
            animator.SetTrigger("Throw");
            AudioManager.Instance.PlayPlayerAttack(1);
            StartCoroutine(ThrowAttack(3f, 1f));
        }

        // 노트북 공격
        if (Input.GetMouseButtonDown(0) && !isSmashing && weaponComponent.currentAttackInfo == weaponComponent.attackInfosList[2])
        {
            SmashWeapon();
            animator.SetTrigger("Smash");
            StartCoroutine(SmashAudio(0.8f));
            StartCoroutine(SmashAttack());
        }

        // 마우스 찌르기
        if (Input.GetMouseButtonDown(0) && !isThrusting && weaponComponent.currentAttackInfo == weaponComponent.attackInfosList[3])
        {
            ThrustWeapon();
            animator.SetTrigger("Thrust");
            AudioManager.Instance.PlayPlayerAttack(3);
            StartCoroutine(ThrustAttack(1f, 0.5f));
        }

        // 빔프로젝터 공격
        if (Input.GetMouseButtonDown(0) && !isCoding && weaponComponent.currentAttackInfo == weaponComponent.attackInfosList[4])
        {
            CodingWeapon();
            animator.SetTrigger("Whip");
            AudioManager.Instance.PlayPlayerAttack(4);
            StartCoroutine(iscodingAttack(3f, 1f));
        }
    }

    #region 맨손 공격

    IEnumerator Attack()
    {
        EnableAttackHitbox();
        yield return new WaitForSeconds(0.1f);
        DisableAttackHitbox();
        yield return new WaitForSeconds(attackCooldown - 0.1f);
        isAttacking = false;
    }

    #endregion

    #region 연필 던지기

    IEnumerator ThrowAttack(float distance, float duration)
    {
        EnableAttackHitbox();

        Vector3 startPos  = weapon.transform.position;
        Vector3 targetPos = startPos + weapon.transform.forward * distance;

        float elapsed = 0f;
        while (elapsed < duration)
        {
            weapon.transform.position = Vector3.Lerp(startPos, targetPos, elapsed / duration);
            elapsed += Time.deltaTime;
            yield return null;
        }

        DisableAttackHitbox();
        ResetWeapon();
    }

    private void ThrowWeapon()
    {
        isThrown = true;
        throwTimer = 0f;
        SpawnWeaponModel();
        weapon.transform.SetParent(null);
    }

    #endregion

    #region 노트북 공격

    IEnumerator SmashAttack()
    {
        EnableAttackHitbox();
        yield return new WaitForSeconds(0.1f);
        DisableAttackHitbox();
        ResetWeapon();
        yield return new WaitForSeconds(attackCooldown - 0.1f);
        isSmashing = false;
    }

    IEnumerator SmashAudio(float duration)
    {
        yield return new WaitForSeconds(duration);
        AudioManager.Instance.PlayPlayerAttack(2);
    }

    private void SmashWeapon()
    {
        isSmashing = true;
        SpawnWeaponModel();
        weapon.transform.SetParent(null);
    }

    #endregion

    #region 마우스 공격

    IEnumerator ThrustAttack(float distance, float duration)
    {
        EnableAttackHitbox();

        Vector3 originalPos = weapon.transform.position;
        Vector3 targetPos   = originalPos + weapon.transform.forward * distance;
        float halfDur = duration * 0.5f;
        float elapsed = 0f;

        while (elapsed < halfDur)
        {
            weapon.transform.position = Vector3.Lerp(originalPos, targetPos, elapsed / halfDur);
            elapsed += Time.deltaTime;
            yield return null;
        }
        weapon.transform.position = targetPos;

        elapsed = 0f;
        while (elapsed < halfDur)
        {
            weapon.transform.position = Vector3.Lerp(targetPos, originalPos, elapsed / halfDur);
            elapsed += Time.deltaTime;
            yield return null;
        }
        weapon.transform.position = originalPos;

        DisableAttackHitbox();
        ResetWeapon();
        isThrusting = false;
    }

    private void ThrustWeapon()
    {
        isThrusting = true;
        SpawnWeaponModel();
        weapon.transform.SetParent(null);
    }

    #endregion

    #region 빔프로젝터 공격

    IEnumerator iscodingAttack(float distance, float duration)
    {
        EnableAttackHitbox();

        Vector3 startPos  = weapon.transform.position;
        Vector3 targetPos = startPos + weapon.transform.forward * distance;

        float elapsed = 0f;
        while (elapsed < duration)
        {
            weapon.transform.position = Vector3.Lerp(startPos, targetPos, elapsed / duration);
            elapsed += Time.deltaTime;
            yield return null;
        }

        DisableAttackHitbox();
        ResetWeapon();
        isCoding = false;
    }

    private void CodingWeapon()
    {
        isCoding = true;
        SpawnWeaponModel();
        weapon.transform.SetParent(null);
    }

    #endregion

    // ── 공용 헬퍼 ─────────────────────────────────────────────────────────────

    /// <summary>
    /// 현재 무기의 공격 모델을 풀에서 꺼냅니다.
    /// Instantiate 대신 ObjectPoolManager를 사용해 GC 스파이크를 방지합니다.
    /// </summary>
    private void SpawnWeaponModel()
    {
        GameObject modelPrefab = weaponComponent.currentAttackInfo.attackModel;
        if (modelPrefab == null) return;

        spawnedModelPrefab = modelPrefab;
        spawnedModel = ObjectPoolManager.Instance.Spawn(
            modelPrefab,
            weapon.transform.position,
            weapon.transform.rotation,
            weapon.transform   // weapon 하위로 배치
        );
    }

    private void EnableAttackHitbox()
    {
        weaponComponent.Hitbox.enabled = true;
    }

    private void DisableAttackHitbox()
    {
        weaponComponent.Hitbox.enabled = false;
        isThrown = false;
    }

    private void ResetWeapon()
    {
        weapon.transform.SetParent(weaponResetPoint);
        weapon.transform.localPosition = Vector3.zero;
        weapon.transform.localRotation = Quaternion.identity;
        throwTimer = 0f;

        // Destroy 대신 풀로 반환
        if (spawnedModel != null)
        {
            ObjectPoolManager.Instance.ReturnToPool(spawnedModelPrefab, spawnedModel);
            spawnedModel       = null;
            spawnedModelPrefab = null;
        }

        weapon.SetActive(true);
    }

    public bool IsAttackingOrBusy()
    {
        return isAttacking || isThrown || isSmashing || isThrusting || isCoding;
    }
}
