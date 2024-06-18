using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Pool;

public abstract class PoolObject : MonoBehaviour
{
    IObjectPool<PoolObject> _myPool;
    public IObjectPool<PoolObject> myPool
    {
        get => _myPool;
        set => _myPool = value;
    }


    public PoolObject SetPool(IObjectPool<PoolObject> pool)
    {
        _myPool = pool;
        return this;
    }

    public virtual void RelasePool()
    {
        if (myPool == null)
        {
            Destroy(gameObject);
            return;
        }

        _myPool.Release(this);
    }
}
