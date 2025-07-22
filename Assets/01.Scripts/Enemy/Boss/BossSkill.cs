using UnityEngine;

public class BossSkill : MonoBehaviour
{
    public GameObject skill1Prefab;
    public GameObject skill2Prefab;

    public Transform firePoint;
    public float skillCooldown = 10f;
    private float lastSkillTime = -Mathf.Infinity;

    private bool isUsingSkill = false;
    public bool bossTriggered = false;

    public bool IsSkillInProgress()
    {
        return isUsingSkill;
    }

    public bool IsCooldownOver()
    {
        return Time.time >= lastSkillTime + skillCooldown;
    }

    public bool CheckSkillChance(float chance)
    {
        return Random.value < chance;
    }

    public void TryCastSkill1()
    {
        if (!bossTriggered) return;

        isUsingSkill = true;
        lastSkillTime = Time.time;

        // Skill1 오브젝트 생성
        Vector3 spawnPos = transform.position + transform.forward * 5f;
        spawnPos.y = -7.7f;

        GameObject obj = Instantiate(skill1Prefab, spawnPos, skill1Prefab.transform.rotation);
        BossSkill1 skill = obj.GetComponent<BossSkill1>();
        if (skill != null)
            skill.StartSkill();

        // AudioManager 통해 사운드 재생
        AudioManager.Instance.PlayBossSkill1();

        Invoke(nameof(EndSkill), 2f);
    }

    public void TryCastSkill2()
    {
        if (!bossTriggered) return;
        
        isUsingSkill = true;
        lastSkillTime = Time.time;

        // 🔥 Skill2 오브젝트 생성
        GameObject obj = Instantiate(skill2Prefab, firePoint.position, Quaternion.identity);
        BossSkill2 skill = obj.GetComponent<BossSkill2>();
        if (skill != null)
        {
            skill.firePoint = firePoint;
            skill.StartSkill();
        }

        // AudioManager 통해 사운드 재생
        AudioManager.Instance.PlayBossSkill2();

        Invoke(nameof(EndSkill), 2f);
    }

    private void EndSkill()
    {
        isUsingSkill = false;
    }

    public void PuzzleSuccess()
    {
        Debug.Log("퍼즐 성공 → 즉사 스킬 취소, 전투 계속!");
    }

    public void PuzzleFailed()
    {
        Debug.Log("퍼즐 실패 → 플레이어 즉사!");
        FindObjectOfType<PlayerHealth>().TakeDamage(100f);
    }
}
