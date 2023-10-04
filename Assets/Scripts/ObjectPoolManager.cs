using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectPoolManager : MonoBehaviour
{
    public static ObjectPoolManager Instance { get; private set; }
    public Dictionary<string, ObjectPool<Component>> objectPools = new Dictionary<string, ObjectPool<Component>>();

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(this);
        }
    }

    public void CreateObjectPool<T>(string poolName, T prefab, int initialCount) where T : Component
    {
        ObjectPool<Component> objectPool = new ObjectPool<Component>(
            () => Instantiate(prefab),
            initialCount
        );
        objectPools.Add(poolName, objectPool);
    }

    public T GetObjectPool<T>(string poolName) where T : Component
    {
        if(objectPools.ContainsKey(poolName))
        { 
        }
        return (T)objectPools[poolName].GetObject();
    }

    public void ReturnObjectPool<T>(string poolName,T obj) where T : Component
    {
        objectPools[poolName].ReturnObject(obj);
    }
}
