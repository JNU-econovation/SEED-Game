using System.Collections;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UIElements;
using static UnityEditor.ShaderGraph.Internal.KeywordDependentCollection;

public class PlayerAttack_MK : MonoBehaviour
{
    [SerializeField] private Animator animator;
    [SerializeField] private GameObject weapon;

    private Weapon weaponComponent;
    public bool isAttacking = false;
    private float attackCooldown; // 애니메이션 길이만큼
    
    private PlayerMovement movement;

    private GameObject spawnedModel;

    //pencilcase 관련
    public float speed = 10f;
    public float lifeTime = 1f;
    private bool isThrown = false;
    private float throwTimer = 0f;

    //laptopweapon 관련
    public bool isSmashing = false;

    //mouse 관련
    public bool isThrusting = false;

    //beamProjector 관련
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
        // 구르기 중엔 공격 허용 X
        if (movement != null && movement.isRolling) return;
        // 공격 입력
        if (Input.GetMouseButtonDown(0) && !isAttacking && weaponComponent.currentAttackInfo == weaponComponent.attackInfosList[0])
        {
            isAttacking = true;
            animator.SetTrigger("Attack");
            StartCoroutine(Attack());
        }

        // 던지기
        if (Input.GetMouseButtonDown(0) && !isThrown && weaponComponent.currentAttackInfo == weaponComponent.attackInfosList[1])
        {
            ThrowWeapon();
            StartCoroutine(ThrowAttack(5f, 1f)); // 거리, 총 시간
        }

        if (Input.GetMouseButtonDown(0) && !isSmashing && weaponComponent.currentAttackInfo == weaponComponent.attackInfosList[2])
        {
            SmashWeapon();
            StartCoroutine(SmashAttack());
        }

        if (Input.GetMouseButtonDown(0) && !isThrusting && weaponComponent.currentAttackInfo == weaponComponent.attackInfosList[3])
        {
            ThrustWeapon();
            StartCoroutine(ThrustAttack(2f, 0.5f)); // 거리, 총 시간
        }

        if (Input.GetMouseButtonDown(0) && !isCoding && weaponComponent.currentAttackInfo == weaponComponent.attackInfosList[4])
        {
            CodingWeapon();
            StartCoroutine(iscodingAttack(5f, 1f));  // 거리, 총 시간
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

    #region 연필 공격

    IEnumerator ThrowAttack(float distance, float duration)
    {
        EnableAttackHitbox();

        Vector3 startPos = weapon.transform.position;
        Vector3 targetPos = startPos + weapon.transform.forward * distance;

        float elapsed = 0f;

        // Step 1: 앞으로 이동
        while (elapsed < duration)
        {
            weapon.transform.position = Vector3.Lerp(startPos, targetPos, elapsed / duration);
            elapsed += Time.deltaTime;
            yield return null;
        }

        // Step 2: 바로 리셋
        DisableAttackHitbox();
        ResetWeapon();
    }

    private void ThrowWeapon()
    {
        isThrown = true;
        throwTimer = 0f;

        // attackModel 프리팹을 동적으로 생성
        GameObject modelPrefab = weaponComponent.currentAttackInfo.attackModel;
        if (modelPrefab != null)
        {
            spawnedModel = Instantiate(modelPrefab, weapon.transform);
        }

        weapon.transform.SetParent(null); //부모에서 분리
    }

    #endregion

    #region 노트북 공격

    IEnumerator SmashAttack()
    {
        Vector3 originalPos = weapon.transform.position + weapon.transform.forward * 1f;
        Quaternion originalRot = weapon.transform.rotation;

        // Step 1: 위로 들어올림 (앞으로 + 위로)
        weapon.transform.position += weapon.transform.up * 1.3f + weapon.transform.forward * 1f;
        yield return new WaitForSeconds(0.2f);

        // Step 2: 아래로 빠르게 내리침
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

        // Step 3: 히트박스 활성화
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

        // attackModel 프리팹을 동적으로 생성
        GameObject modelPrefab = weaponComponent.currentAttackInfo.attackModel;
        if (modelPrefab != null)
        {
            spawnedModel = Instantiate(modelPrefab, weapon.transform);
        }

        weapon.transform.SetParent(null); //부모에서 분리
    }

    #endregion

    #region 마우스 공격

    IEnumerator ThrustAttack(float distance, float duration)
    {
        EnableAttackHitbox();

        Vector3 originalPos = weapon.transform.position;
        Vector3 targetPos = originalPos + weapon.transform.forward * distance;

        float elapsed = 0f;

        // Step 1: 앞으로 이동
        while (elapsed < duration / 2f)
        {
            weapon.transform.position = Vector3.Lerp(originalPos, targetPos, elapsed / (duration / 2f));
            elapsed += Time.deltaTime;
            yield return null;
        }
        weapon.transform.position = targetPos;

        elapsed = 0f;

        // Step 2: 다시 돌아오기
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

        // attackModel 프리팹을 동적으로 생성
        GameObject modelPrefab = weaponComponent.currentAttackInfo.attackModel;
        if (modelPrefab != null)
        {
            spawnedModel = Instantiate(modelPrefab, weapon.transform);
        }

        weapon.transform.SetParent(null); //부모에서 분리
    }

    #endregion

    #region 빔프로젝터 공격

    IEnumerator iscodingAttack(float distance, float duration)
    {
        EnableAttackHitbox();

        Vector3 startPos = weapon.transform.position;
        Vector3 targetPos = startPos + weapon.transform.forward * distance;

        float elapsed = 0f;

        // Step 1: 앞으로 이동
        while (elapsed < duration)
        {
            weapon.transform.position = Vector3.Lerp(startPos, targetPos, elapsed / duration);
            elapsed += Time.deltaTime;
            yield return null;
        }

        // Step 2: 바로 리셋
        DisableAttackHitbox();
        ResetWeapon();
        isCoding = false;
    }

    private void CodingWeapon()
    {
        isCoding = true;

        // attackModel 프리팹을 동적으로 생성
        GameObject modelPrefab = weaponComponent.currentAttackInfo.attackModel;
        if (modelPrefab != null)
        {
            spawnedModel = Instantiate(modelPrefab, weapon.transform);
        }

        weapon.transform.SetParent(null); //부모에서 분리
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
        // 위치와 회전 초기화
        weapon.transform.SetParent(transform);
        weapon.transform.localPosition = new Vector3(0, 0, 0);
        weapon.transform.localRotation = Quaternion.identity;

        // 타이머 리셋
        throwTimer = 0f;

        // 생성된 모델 제거
        if (spawnedModel != null)
        {
            Destroy(spawnedModel);
            spawnedModel = null;
        }

        // 다시 활성화
        weapon.SetActive(true);
    }
}