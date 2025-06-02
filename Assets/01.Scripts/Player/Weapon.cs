using UnityEngine;

public class Weapon : MonoBehaviour
{
    [SerializeField] private AttackInfos attackInfos;
    [SerializeField] private Collider hitbox;
    
    public Collider Hitbox => hitbox;
    public float Damage => attackInfos.damage;
    // 무기마다 AttackSpeed를 애니메이션 재생 속도로
    public float AttackSpeed => attackInfos.attackSpeed;

    
}

