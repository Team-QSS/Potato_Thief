using System.Collections;
using System.Collections.Generic;
using Photon.Pun;
using UnityEngine;

public class SingletonPhotonCallbacks<T> : MonoBehaviourPunCallbacks where T : SingletonPhotonCallbacks<T>
{
    public bool dontDestroyOnLoad;

    private static volatile T _instance;
    private static object _syncRoot = new System.Object();

    public static T instance
    {
        get
        {
            Initialize();
            return _instance;
        }
    }

    public static bool isInitialized
    {
        get { return _instance != null; }
    }

    public static void Initialize()
    {
        if (_instance != null)
        {
            return;
        }

        lock (_syncRoot)
        {
            _instance = GameObject.FindObjectOfType<T>();

            if (_instance == null)
            {
                var go = new GameObject(typeof(T).FullName);
                _instance = go.AddComponent<T>();
            }
        }
    }

    protected virtual void Awake()
    {
        if (_instance != null)
        {
            Debug.LogError(GetType().Name + " Singleton class is already created.");
        }

        if (dontDestroyOnLoad)
        {
            DontDestroyOnLoad(this);
        }

        OnAwake();
    }

    protected virtual void OnDestroy()
    {
        if (_instance == this)
        {
            _instance = null;
        }
    }

    protected virtual void OnAwake()
    {
    }
}
