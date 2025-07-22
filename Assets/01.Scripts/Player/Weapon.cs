using UnityEngine;
using System.Collections.Generic;

public class Weapon : MonoBehaviour
{
    [SerializeField] public List<AttackInfos> attackInfosList;
    [SerializeField] private Collider hitbox;

    public AttackInfos currentAttackInfo; // 현재 사용중인 공격 정보

    public Collider Hitbox => hitbox;

    public float Damage => currentAttackInfo?.damage ?? 0f;
    public float AttackSpeed => currentAttackInfo?.attackSpeed ?? 0f;

    private void Start()
    {
    }

    // 외부에서 선택한 AttackInfos를 설정
    public void SetAttackInfo(AttackInfos info)
    {
        currentAttackInfo = info;
    }

    public AttackInfos GetAttackInfo(int index)
    {
        if (index >= 0 && index < attackInfosList.Count)
            return attackInfosList[index];
        else
            return null;
    }
}