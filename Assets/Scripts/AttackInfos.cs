using UnityEngine;

/// <summary>
/// 무기나 적의 공격 수치
/// </summary>
public enum AttackType
{
    Normal,
    Skill,
    Special
}

[CreateAssetMenu(fileName = "AttackInfos", menuName = "ScriptableObject/AttackInfos")]
public class AttackInfos : ScriptableObject
{
    public AttackType attackType;
    
    public float damage;
    public float attackSpeed;
    public float attackRange;
    
    [Header("공격 이펙트 및 효과음")]
    public GameObject attackEffect;
    public AudioClip attackSound;
    
    
}
