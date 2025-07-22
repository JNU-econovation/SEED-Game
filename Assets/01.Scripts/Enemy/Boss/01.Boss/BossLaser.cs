using UnityEngine;

public class BossLaser : MonoBehaviour
{
    [Header("레이저 설정")]
    public float laserDuration = 1.5f;
    public float damage = 30f;
    public float laserRange = 50f;
    public LayerMask hitLayers;

    [Header("필수 참조")]
    public LineRenderer lineRenderer;
    public Transform firePoint;

    [Header("이펙트 오브젝트들")]
    public GameObject muzzleEffectGroup; // 시작 이펙트 (Spark, Glow 등)
    public GameObject hitEffectGroup;    // 끝 이펙트 (Hit, HitGlow 등)

    private void Awake()
    {
        if (lineRenderer != null)
            lineRenderer.enabled = false;

        if (muzzleEffectGroup != null)
            muzzleEffectGroup.SetActive(false);

        if (hitEffectGroup != null)
            hitEffectGroup.SetActive(false);
    }

    public void StartSkill()
    {
        GameObject player = GameObject.FindGameObjectWithTag("Player");
        if (player == null || firePoint == null)
        {
            Debug.LogWarning("Player 또는 firePoint가 설정되지 않았습니다.");
            return;
        }

        // 방향 계산
        Vector3 dir = (player.transform.position - firePoint.position).normalized;
        dir.y = 0f;

        // 충돌 체크
        Vector3 end = dir * laserRange;
        if (Physics.Raycast(firePoint.position, dir, out RaycastHit hit, laserRange, hitLayers))
        {
            end = dir * hit.distance;

            if (hit.collider.CompareTag("Player"))
            {
                Debug.Log("플레이어 피격");
            }
        }

        // 🔧 위치/회전 맞춤
        transform.position = firePoint.position;
        transform.rotation = Quaternion.LookRotation(dir);

        // 🔧 라인 렌더러 설정 (View + Local Space)
        if (lineRenderer != null)
        {
            lineRenderer.useWorldSpace = false; // ✅ Local 좌표
            // 🔥 Alignment는 반드시 "View"로! (Inspector에서)
            lineRenderer.positionCount = 2;
            lineRenderer.SetPosition(0, Vector3.zero);
            lineRenderer.SetPosition(1, end);
            lineRenderer.enabled = true;
        }

        // 이펙트들
        if (muzzleEffectGroup != null)
            muzzleEffectGroup.SetActive(true);

        if (hitEffectGroup != null)
        {
            hitEffectGroup.transform.localPosition = end;
            hitEffectGroup.SetActive(true);
        }

        Destroy(gameObject, laserDuration);
    }


}
