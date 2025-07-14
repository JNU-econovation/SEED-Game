using System.Collections;
using UnityEngine;

public class PlayerAttack_MK : MonoBehaviour
{
    [SerializeField] private Animator animator;
    [SerializeField] private GameObject weapon;

    private Weapon weaponComponent;
    public bool isAttacking = false;
    private float attackCooldown;

    private PlayerMovement movement;

    private GameObject spawnedModel;

    // pencilcase 관련
    public float speed = 10f;
    public float lifeTime = 1f;
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
    }

    void Start()
    {
        movement = GetComponent<PlayerMovement>();
    }

    void Update()
    {
        if (movement != null && movement.isRolling) return;

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
            AudioManager.Instance.PlayPlayerAttack(1);
            StartCoroutine(ThrowAttack(5f, 1f));
        }

        // 노트북 공격
        if (Input.GetMouseButtonDown(0) && !isSmashing && weaponComponent.currentAttackInfo == weaponComponent.attackInfosList[2])
        {
            SmashWeapon();

            AudioManager.Instance.PlayPlayerAttack(2);

            StartCoroutine(SmashAttack());
        }

        // 마우스 찌르기
        if (Input.GetMouseButtonDown(0) && !isThrusting && weaponComponent.currentAttackInfo == weaponComponent.attackInfosList[3])
        {
            ThrustWeapon();

            AudioManager.Instance.PlayPlayerAttack(3);

            StartCoroutine(ThrustAttack(2f, 0.5f));
        }

        // 빔프로젝터 공격
        if (Input.GetMouseButtonDown(0) && !isCoding && weaponComponent.currentAttackInfo == weaponComponent.attackInfosList[4])
        {
            CodingWeapon();

            AudioManager.Instance.PlayPlayerAttack(4);

            StartCoroutine(iscodingAttack(5f, 1f));
        }
    }

    #region 맨손 공격

    IEnumerator Attack()
    {
        EnableAttackHitbox();
        yield return new WaitForSeconds(0.1f);
        DisableAttackHitbox();
        // 만약 애니메이션이 
        yield return new WaitForSeconds(attackCooldown - 0.1f);
        isAttacking = false;
    }

    #endregion

    #region 연필 던지기

    IEnumerator ThrowAttack(float distance, float duration)
    {
        EnableAttackHitbox();

        Vector3 startPos = weapon.transform.position;
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

        GameObject modelPrefab = weaponComponent.currentAttackInfo.attackModel;
        if (modelPrefab != null)
        {
            spawnedModel = Instantiate(modelPrefab, weapon.transform);
        }

        weapon.transform.SetParent(null);
    }

    #endregion

    #region 노트북 공격

    IEnumerator SmashAttack()
    {
        Vector3 originalPos = weapon.transform.position + weapon.transform.forward * 1f;

        weapon.transform.position += weapon.transform.up * 1.3f + weapon.transform.forward * 1f;
        yield return new WaitForSeconds(0.2f);

        float duration = 0.1f;
        float elapsed = 0f;
        Vector3 startPos = weapon.transform.position;
        Vector3 targetPos = originalPos;

        while (elapsed < duration)
        {
            weapon.transform.position = Vector3.Lerp(startPos, targetPos, elapsed / duration);
            elapsed += Time.deltaTime;
            yield return null;
        }
        weapon.transform.position = targetPos;

        EnableAttackHitbox();
        yield return new WaitForSeconds(0.1f);
        DisableAttackHitbox();

        yield return new WaitForSeconds(0.3f);

        ResetWeapon();
        isSmashing = false;
    }

    private void SmashWeapon()
    {
        isSmashing = true;

        GameObject modelPrefab = weaponComponent.currentAttackInfo.attackModel;
        if (modelPrefab != null)
        {
            spawnedModel = Instantiate(modelPrefab, weapon.transform);
        }

        weapon.transform.SetParent(null);
    }

    #endregion

    #region 마우스 공격

    IEnumerator ThrustAttack(float distance, float duration)
    {
        EnableAttackHitbox();

        Vector3 originalPos = weapon.transform.position;
        Vector3 targetPos = originalPos + weapon.transform.forward * distance;

        float elapsed = 0f;

        while (elapsed < duration / 2f)
        {
            weapon.transform.position = Vector3.Lerp(originalPos, targetPos, elapsed / (duration / 2f));
            elapsed += Time.deltaTime;
            yield return null;
        }
        weapon.transform.position = targetPos;

        elapsed = 0f;
        while (elapsed < duration / 2f)
        {
            weapon.transform.position = Vector3.Lerp(targetPos, originalPos, elapsed / (duration / 2f));
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

        GameObject modelPrefab = weaponComponent.currentAttackInfo.attackModel;
        if (modelPrefab != null)
        {
            spawnedModel = Instantiate(modelPrefab, weapon.transform);
        }

        weapon.transform.SetParent(null);
    }

    #endregion

    #region 빔프로젝터 공격

    IEnumerator iscodingAttack(float distance, float duration)
    {
        EnableAttackHitbox();

        Vector3 startPos = weapon.transform.position;
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

        GameObject modelPrefab = weaponComponent.currentAttackInfo.attackModel;
        if (modelPrefab != null)
        {
            spawnedModel = Instantiate(modelPrefab, weapon.transform);
        }

        weapon.transform.SetParent(null);
    }

    #endregion

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
        weapon.transform.SetParent(transform);
        weapon.transform.localPosition = Vector3.zero;
        weapon.transform.localRotation = Quaternion.identity;

        throwTimer = 0f;

        if (spawnedModel != null)
        {
            Destroy(spawnedModel);
            spawnedModel = null;
        }

        weapon.SetActive(true);
    }
}
