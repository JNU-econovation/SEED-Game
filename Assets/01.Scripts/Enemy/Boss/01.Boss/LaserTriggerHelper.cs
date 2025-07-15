using UnityEngine;

public class LaserTriggerHelper : MonoBehaviour
{
    private BossSkill2 bossSkill;

    public void Init(BossSkill2 skill)
    {
        bossSkill = skill;
    }

    private void OnTriggerStay(Collider other)
    {
        PlayerHealth player = other.GetComponent<PlayerHealth>();
        if (player != null && bossSkill != null)
        {
            bossSkill.DealDamage(player);
            Debug.Log($"🔥 TriggerStay: {player.name}에게 데미지 전달됨!");
        }
    }
}
