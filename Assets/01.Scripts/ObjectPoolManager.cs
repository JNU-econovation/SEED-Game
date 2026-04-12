using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 프리팹 기반 오브젝트 풀 싱글톤.
/// Instantiate/Destroy 대신 Spawn/ReturnToPool을 사용해 GC 스파이크를 방지합니다.
/// </summary>
public class ObjectPoolManager : MonoBehaviour
{
    private static ObjectPoolManager _instance;

    /// <summary>
    /// 씬에 ObjectPoolManager가 없어도 자동으로 생성합니다.
    /// </summary>
    public static ObjectPoolManager Instance
    {
        get
        {
            if (_instance == null)
            {
                var go = new GameObject("ObjectPoolManager");
                _instance = go.AddComponent<ObjectPoolManager>();
                DontDestroyOnLoad(go);
            }
            return _instance;
        }
    }

    // 프리팹 → 비활성 오브젝트 큐
    private readonly Dictionary<GameObject, Queue<GameObject>> pools = new();

    void Awake()
    {
        if (_instance != null && _instance != this)
        {
            Destroy(gameObject);
            return;
        }
        _instance = this;
        DontDestroyOnLoad(gameObject);
    }

    /// <summary>
    /// 풀에서 오브젝트를 꺼냅니다. 풀이 비어 있으면 새로 생성합니다.
    /// </summary>
    /// <param name="prefab">원본 프리팹</param>
    /// <param name="position">배치 위치</param>
    /// <param name="rotation">배치 회전</param>
    /// <param name="parent">부모 트랜스폼 (null이면 루트)</param>
    public GameObject Spawn(GameObject prefab, Vector3 position, Quaternion rotation, Transform parent = null)
    {
        if (!pools.TryGetValue(prefab, out Queue<GameObject> queue))
        {
            queue = new Queue<GameObject>();
            pools[prefab] = queue;
        }

        GameObject obj = queue.Count > 0 ? queue.Dequeue() : Instantiate(prefab);

        obj.transform.SetParent(parent);
        obj.transform.SetPositionAndRotation(position, rotation);
        obj.SetActive(true);
        return obj;
    }

    /// <summary>
    /// 오브젝트를 비활성화해 풀로 반환합니다.
    /// </summary>
    /// <param name="prefab">원본 프리팹 (Spawn 시 사용한 것과 동일)</param>
    /// <param name="obj">반환할 오브젝트</param>
    public void ReturnToPool(GameObject prefab, GameObject obj)
    {
        if (prefab == null || obj == null) return;

        if (!pools.TryGetValue(prefab, out Queue<GameObject> queue))
        {
            queue = new Queue<GameObject>();
            pools[prefab] = queue;
        }

        obj.SetActive(false);
        obj.transform.SetParent(transform); // 풀 매니저 하위로 이동
        queue.Enqueue(obj);
    }
}
