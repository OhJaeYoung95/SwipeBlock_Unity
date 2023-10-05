using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectPool<T> where T : Component
{
    private List<T> pool = new List<T>();
    private Func<T> create;

    public ObjectPool(Func<T> create, GameObject folder, int initialCount)
    {
        this.create = create;

        for (int i = 0; i < initialCount; i++)
        {
            T obj = this.create();
            obj.transform.parent = folder.transform;
            obj.gameObject.SetActive(false);
            pool.Add(obj);
        }
    }
    public void CreateObjects(Func<T> create, int initSize)
    {
        for (int i = 0; i < initSize; i++)
        {
            T obj = create();
            obj.gameObject.SetActive(false);
            pool.Add(obj);
        }
    }

    public T GetObject()
    {
        T obj = pool.Find(item => !item.gameObject.activeSelf);
        if(obj == null)
        {
            CreateObjects(create, 30);
            obj = pool.Find(item => !item.gameObject.activeSelf);
        }
        obj.gameObject.SetActive(true);
        pool.Remove(obj);
        return obj; 
    }

    public void ReturnObject(T obj)
    {
        obj.gameObject.SetActive(false);
        pool.Add(obj);
    }
}