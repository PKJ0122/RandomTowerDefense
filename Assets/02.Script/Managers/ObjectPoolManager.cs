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
    /// ������Ʈ Ǯ ��ȯ �Լ�
    /// </summary>
    public IObjectPool<PoolObject> Get(string Key)
    {
        return Pool[Key];
    }

    /// <summary>
    /// string�� Ű������ ���� ������ Pool�� ���� �׸��� ��ųʸ��� ���
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