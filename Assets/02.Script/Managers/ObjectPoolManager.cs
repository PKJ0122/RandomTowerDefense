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
    /// 오브젝트 풀 반환 함수
    /// </summary>
    public IObjectPool<PoolObject> Get(UnitKind unitKind)
    {
        if (!Pool.ContainsKey(unitKind))
            CreatePool(unitKind);

        return Pool[unitKind];
    }

    /// <summary>
    /// UnitKind를 키값으로 유닛 프리펩 Pool을 생성 그리고 딕셔너리에 등록
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