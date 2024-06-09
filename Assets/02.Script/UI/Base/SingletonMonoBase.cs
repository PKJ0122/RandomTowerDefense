using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class SingletonMonoBase<T> : MonoBehaviour
    where T : SingletonMonoBase<T>
{
    static T s_instance;
    public static T Instance => s_instance;

    protected virtual void Awake()
    {
        if (s_instance == null)
        {
            s_instance = (T)this;
            DontDestroyOnLoad(gameObject);
            return;
        }

        Destroy(gameObject);
    }
}
