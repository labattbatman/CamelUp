using UnityEngine;
using System.Collections;

public abstract class MonoSingleton<T> : MonoBehaviour where T : MonoSingleton<T>
{
    private static T _instance;

    public static T Instance
    {
        get
        {
            if (_instance == null)
            {
                GameObject go = new GameObject("Game Manager");
                _instance = go.AddComponent<T>();
            }

            return _instance;
        }
    }

    public void Awake()
    {
        _instance = GameObject.FindObjectOfType<T>();
    }
}
