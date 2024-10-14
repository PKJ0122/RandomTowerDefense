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

    protected virtual void Awake()
    {
        DontDestroyOnLoad(gameObject);
    }

    protected virtual void OnEnable()
    {
        UIManager.Instance.Get<GameEndUI>().OnReStartButtonClick += RelasePool;
        UIManager.Instance.Get<GameEndUI>().OnLobbyButtonClick += RelasePool;
    }

    public PoolObject SetPool(IObjectPool<PoolObject> pool)
    {
        MyPool = pool;
        return this;
    }

    public virtual void RelasePool()
    {
        UIManager.Instance.Get<GameEndUI>().OnReStartButtonClick -= RelasePool;
        UIManager.Instance.Get<GameEndUI>().OnLobbyButtonClick -= RelasePool;
        MyPool.Release(this);
    }
}
