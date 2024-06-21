using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Pool;

public abstract class PoolObject : MonoBehaviour
{
    IObjectPool<PoolObject> _myPool;
    public IObjectPool<PoolObject> MyPool
    {
        get => _myPool;
        set => _myPool = value;
    }


    public PoolObject SetPool(IObjectPool<PoolObject> pool)
    {
        MyPool = pool;
        return this;
    }

    public virtual void RelasePool()
    {
        MyPool.Release(this);
    }
}
