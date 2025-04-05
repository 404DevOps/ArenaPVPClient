using UnityEngine;

public class Singleton<T> : MonoBehaviour where T : MonoBehaviour
{
    private static T instance;
    private static bool isShuttingDown = false;

    protected bool KeepOnSceneLoad = true;

    public static T Instance
    {
        get
        {
            if (isShuttingDown)
            {
                Debug.LogWarning($"[Singleton] Instance of '{typeof(T)}' is being destroyed. Returning null.");
                return null;
            }

            return instance;
        }
    }

    protected virtual void Awake()
    {
        if (instance != null && instance != this)
        {
            Debug.LogWarning($"[Singleton] Duplicate instance of '{typeof(T)}' detected. Destroying the new one.");
            Destroy(gameObject); // Destroy duplicate
            return;
        }

        instance = this as T;
        DontDestroyOnLoad(gameObject); // Keep the singleton across scene loads
    }

    protected virtual void OnApplicationQuit()
    {
        isShuttingDown = true;
    }

    protected virtual void OnDestroy()
    {
        if (instance == this)
        {
            instance = null;
            isShuttingDown = true;
        }
    }
}

