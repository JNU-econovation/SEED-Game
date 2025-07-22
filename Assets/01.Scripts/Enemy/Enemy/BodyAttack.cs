using UnityEngine;

public class BodyAttack : MonoBehaviour
{
    [SerializeField] private BoxCollider enemy; 
    [SerializeField] private BoxCollider weapon; 

    void Update()
    {
        if (enemy == null || weapon == null) return;

        // 위치, 회전, 크기 동기화
        weapon.transform.position = enemy.transform.position;
        weapon.transform.rotation = enemy.transform.rotation;
        weapon.transform.localScale = enemy.transform.localScale;
        
        // BoxCollider 설정값 복사
        weapon.center = enemy.center;
        weapon.size = enemy.size;
    }
}
