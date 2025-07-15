using System.Collections;
using UnityEngine;

public class BossSkill2 : MonoBehaviour
{
    public Transform firePoint;
    public Transform target;
    public GameObject laserPrefab;
    public float laserDuration = 3f;
    public float laserLength = 20f;
    public float damagePerSecond = 50f;
    public LayerMask hitLayerMask;

    private GameObject laser;

    public void StartSkill()
    {
        if (target == null)
        {
            GameObject player = GameObject.FindWithTag("Player");
            if (player != null)
                target = player.transform;
        }

        if (target != null && firePoint != null)
            StartCoroutine(FireLaser());
    }

    private IEnumerator FireLaser()
    {
        Vector3 startPos = firePoint.position;
        Vector3 direction = Quaternion.Euler(0f, -15f, 0f) * (target.position - startPos);

        laser = Instantiate(laserPrefab, startPos, Quaternion.LookRotation(direction));

        // Cylinder 오브젝트 찾아서 TriggerHelper 붙이기
        Transform cylinder = laser.transform.Find("Cylinder");
        if (cylinder != null)
        {
            LaserTriggerHelper helper = cylinder.gameObject.GetComponent<LaserTriggerHelper>();
            if (helper == null)
                helper = cylinder.gameObject.AddComponent<LaserTriggerHelper>();
            helper.Init(this);
        }
        else
        {
            Debug.LogWarning("⚠️ Cylinder 오브젝트를 찾을 수 없습니다!");
        }


        Destroy(laser, laserDuration);
        yield return new WaitForSeconds(laserDuration);
        Destroy(gameObject);
    }

    public void DealDamage(PlayerHealth player)
    {
        if (player != null)
        {
            float damageThisFrame = damagePerSecond * Time.deltaTime;

            player.enableStun = false;
            player.TakeDamage(damageThisFrame);

            // 한 프레임 뒤에 다시 true로 돌림
            StartCoroutine(ResetStun(player));
        }
    }

    private IEnumerator ResetStun(PlayerHealth player)
    {
        yield return null; // 한 프레임 기다림
        player.enableStun = true;
    }


}
