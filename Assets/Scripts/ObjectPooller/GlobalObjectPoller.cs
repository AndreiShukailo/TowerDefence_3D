using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Object = System.Object;

public class GlobalObjectPoller : MonoBehaviour
{
    public static GlobalObjectPoller instance;
    [SerializeField] private List<ObjectPooller<MonoBehaviour>> _pollers = new List<ObjectPooller<MonoBehaviour>>();

    private void Awake()
    {
        instance = this;
    }

    private T GetFromPools<T>(T component) where T : MonoBehaviour
    {
        var pool = _pollers.Find(pooler => pooler.Prefab == component.gameObject);
        var mono = component as MonoBehaviour;
        T rez;

        if (pool == null)
        {
            var newPool = new ObjectPooller<MonoBehaviour>(mono);
            _pollers.Add(newPool);
             rez = newPool.Get(mono) as T;
        }
        else
        {
            rez = pool.Get(mono) as T;
        }

        return rez;
    }

    public T Get<T>(T component) where T: MonoBehaviour
    {
        return GetFromPools(component);
    }
}
