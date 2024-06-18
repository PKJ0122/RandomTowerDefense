using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Pool;

public class ObjectPoolManager : SingletonMonoBase<ObjectPoolManager>
{
    static Dictionary<UnitKind, IObjectPool<PoolObject>> s_pool;

    public static Dictionary<UnitKind, IObjectPool<PoolObject>> Pool
    {
        get
        {
            if (s_pool == null)
            {
                s_pool = new Dictionary<UnitKind, IObjectPool<PoolObject>>();
            }
            return s_pool;
        }
    }

    /// <summary>
    /// ������Ʈ Ǯ ��ȯ �Լ�
    /// </summary>
    public IObjectPool<PoolObject> Get(UnitKind unitKind)
    {
        if (!Pool.ContainsKey(unitKind))
            CreatePool(unitKind);

        return Pool[unitKind];
    }

    /// <summary>
    /// UnitKind�� Ű������ ���� ������ Pool�� ���� �׸��� ��ųʸ��� ���
    /// </summary>
    public void CreatePool(UnitKind unit)
    {
        IObjectPool<PoolObject> pool = new ObjectPool<PoolObject>(() => Create(unit),
                                                                OnPoolItem,
                                                                OnRelaseItem,
                                                                OnDestroyItem,
                                                                true,
                                                                10,
                                                                30
                                                                );
        Pool.Add(unit, pool);
    }

    PoolObject Create(UnitKind unitKind)
    {
        PoolObject poolitem = UnitRepository.UnitKindDatas[unitKind].unitObject.GetComponent<PoolObject>();
        poolitem.SetPool(Pool[unitKind]);
        return Instantiate(poolitem);
    }

    public void OnPoolItem(PoolObject item)
    {
        item.gameObject.SetActive(true);
    }

    public void OnRelaseItem(PoolObject item)
    {
        item.gameObject.SetActive(false);
    }

    public void OnDestroyItem(PoolObject item)
    {
       Destroy(item.gameObject);
    }
}