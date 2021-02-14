using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BaseManager<T> : MonoBehaviour where T : MonoBehaviour
{
    private static bool isShuttingDown = false;
    public static bool IsShuttingDown;

    private static T instance;

    public static T Instance
    {
        get
        {
            if(IsShuttingDown)
            {
                Debug.LogErrorFormat("{0} is shutting down, returning null", typeof(T));
                return null;
            }

            if (instance == null)
                instance = (T)FindObjectOfType(typeof(T));

            if (instance == null)
            {
                Debug.LogErrorFormat("No manager of {0}. Please add to _PersistentScene", typeof(T));
                return null;
            }

            return instance;
        }
    }

    protected virtual void OnApplicationQuit()
    {
        isShuttingDown = true;
    }

    protected virtual void OnDestroy()
    {
        isShuttingDown = true;
    }

    protected virtual void Start()
    {
        isShuttingDown = false;
    }

    protected virtual void OnEnable()
    {
        isShuttingDown = false;
    }

}
