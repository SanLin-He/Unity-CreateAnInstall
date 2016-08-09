using UnityEngine;
using System.Collections;

public class MonoSingleton<T> : MonoBehaviour where T :Component
{

    // Use this for initialization
    private static T instance;
    void Awake()
    {
        instance = this as T;
    }
    public static T Instance
    {
        
        get
        {
            if (instance == null)
            {
                instance = FindObjectOfType(typeof(T)) as T;
            }
            return instance;
        }
    }
	
}
