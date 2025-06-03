using UnityEngine;

public class EnemyAttack_ES : MonoBehaviour
{
    private HealthBar_ES playerHealth;

    private bool isInvincible = false;   // 몬스터가 무적 상태인지를 관리하는 플래그
    private bool isWaitingForDamage = false;  // 데미지 대기 상태

    private float invincibleTime = 5f;   // 무적 시간 (5초)
    private float istriggerTime = 0f;    // 트리거 충돌 후 시간

    void Start()
    {
        playerHealth = FindObjectOfType<HealthBar_ES>();  // 씬 내 HealthBar_ES 컴포넌트 찾아서 저장
    }

    // 무적 상태를 5초 동안 유지하는 함수
    private void StartInvincibleCooldown()
    {
        isInvincible = true;
        Invoke("EndInvincibleCooldown", invincibleTime);  // 5초 후 무적 해제
    }

    // 무적 상태 해제
    private void EndInvincibleCooldown()
    {
        isInvincible = false;
    }

    // OnTriggerEnter (충돌 시작)
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player") && !isInvincible)
        {
            // 충돌이 시작되면 1초 대기 시작
            if (!isWaitingForDamage)
            {
                isWaitingForDamage = true;
                istriggerTime = Time.time;  // 트리거 시간 기록
            }
        }
    }

    // OnTriggerStay (충돌 중 계속 확인)
    private void OnTriggerStay(Collider other)
    {
        if (other.CompareTag("Player") && isWaitingForDamage && !isInvincible)
        {
            // 충돌 후 1초가 지나면 데미지 주기
            if (Time.time - istriggerTime >= 1f)
            {
                // 1초 동안 충돌이 계속되면 데미지 주기
                if (playerHealth != null)
                {
                    playerHealth.TakeDamage(10f);
                }

                // 데미지 후 무적 상태 시작
                StartInvincibleCooldown();
                isWaitingForDamage = false;  // 대기 상태 해제
            }
        }

        // 무적 상태가 끝난 후 충돌이 계속되면 다시 데미지 처리
        if (isInvincible == false && isWaitingForDamage == false)
        {
            // 충돌 상태에서 무적이 끝난 후 1초가 지나면 다시 데미지 주기
            if (Time.time - istriggerTime >= 1f)
            {
                if (playerHealth != null)
                {
                    playerHealth.TakeDamage(10f);
                    StartInvincibleCooldown(); // 데미지 후 무적 상태 시작
                }
            }
        }
    }

    // OnTriggerExit (충돌 벗어났을 때)
    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            // 플레이어가 충돌을 벗어나면 대기 상태 초기화
            isWaitingForDamage = false;

            // 무적 상태 무시하고 2초 대기 시간 다시 시작
            if (isInvincible)
            {
                // 무적 상태가 끝난 후 다시 2초 대기 시작
                isInvincible = false;
            }
        }
    }
}