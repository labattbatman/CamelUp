using UnityEngine;
using System.Collections;

public class Application : MonoBehaviour
{
    [SerializeField]
    private GameObject[] managers;

    private void Awake()
    {
        foreach(var manager in managers)
        {
            //MonoSingleton<T> singleTon = GetComponent<MonoSingleton<T>>();
        }
    }
	
}
