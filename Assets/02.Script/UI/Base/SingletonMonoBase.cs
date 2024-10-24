using UnityEngine;

public abstract class SingletonMonoBase<T> : MonoBehaviour
    where T : SingletonMonoBase<T>
{
    static T s_instance;
    public static T Instance
    {
        get
        {
            if (s_instance == null)
            {
                s_instance = (T)FindObjectOfType(typeof(T));
                if (s_instance == null)
                {
                    s_instance = new GameObject(typeof(T).Name).AddComponent<T>();
                }
            }

            return s_instance;
        }
    }

    protected virtual void Awake()
    {
        if (s_instance != null)
        {
            Destroy(gameObject);
            return;
        }
    }
}
