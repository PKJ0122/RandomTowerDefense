using System.Collections.Generic;
using UnityEngine.LowLevel;
using UnityEngine.Pool;

public class ObjectPoolManager : SingletonMonoBase<ObjectPoolManager>
{
    static Dictionary<string, IObjectPool<PoolObject>> s_pool;

    public static Dictionary<string, IObjectPool<PoolObject>> Pool
    {
        get
        {
            if (s_pool == null)
            {
                s_pool = new Dictionary<string, IObjectPool<PoolObject>>();
            }
            return s_pool;
        }
    }

    /// <summary>
    /// 오브젝트 풀 반환 함수
    /// </summary>
    public IObjectPool<PoolObject> Get(string Key)
    {
        return Pool[Key];
    }

    /// <summary>
    /// string를 키값으로 유닛 프리펩 Pool을 생성 그리고 딕셔너리에 등록
    /// </summary>
    public void CreatePool(string key,PoolObject poolObject)
    {
        IObjectPool<PoolObject> pool = new ObjectPool<PoolObject>(() => Create(key, poolObject),
                                                                OnPoolItem,
                                                                OnReleaseItem,
                                                                OnDestroyItem,
                                                                false,
                                                                10,
                                                                99
                                                                );
        Pool.Add(key, pool);
    }

    PoolObject Create(string key,PoolObject poolObject)
    {
        return Instantiate(poolObject).SetPool(Pool[key]);
    }

    public void OnPoolItem(PoolObject item)
    {
        item.gameObject.SetActive(true);
    }

    public void OnReleaseItem(PoolObject item)
    {
        item.gameObject.SetActive(false);
    }

    public void OnDestroyItem(PoolObject item)
    {
        Destroy(item.gameObject);
    }
}