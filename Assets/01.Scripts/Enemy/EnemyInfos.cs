using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "EnemyInfos", menuName = "ScriptableObject/EnemyInfos")]
public class EnemyInfos : ScriptableObject
{
    public string name;
    public float maxHealth;
    public float detectionRange;
    public bool isBoss;
    public AttackInfos attackInfo;

    [Header("피격 이펙트 및 효과음")]
    public GameObject hitEffect;
    public AudioClip hitSound;
}
