using UnityEngine;

public abstract class Singleton<T> : MonoBehaviour where T : MonoBehaviour
{
    private static T instance;
    public static T Instance
    {
        get
        {
            if (instance == null)
            {
                // Find an existing instance in the scene
                instance = FindObjectOfType<T>();
                if (instance == null)
                {
                    // Create a new instance if none exists
                    GameObject singletonObject = new GameObject(typeof(T).Name);
                    instance = singletonObject.AddComponent<T>();
                }

                // Make the instance persistent if ShouldPersist returns true
                if (instance is Singleton<T> singletonInstance && singletonInstance.ShouldPersist())
                {
                    DontDestroyOnLoad(singletonInstance.gameObject);
                }
            }
            return instance;
        }
    }

    protected virtual bool ShouldPersist()
    {
        return false;
    }

    protected virtual void Awake()
    {
        if (Instance != this)
        {
            Destroy(gameObject);
        }
        else
        {
            OnAwake();
        }
    }

    protected virtual void Start()
    {
        OnStart();
    }

    protected virtual void OnAwake() { }

    protected virtual void OnStart() { }
}
