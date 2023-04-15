using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Object = System.Object;

public class GlobalObjectPoller : MonoBehaviour
{
    public static GlobalObjectPoller instance;
    [SerializeField] private List<ObjectPooller> _pollers = new List<ObjectPooller>();

    private void Awake()
    {
        instance = this;
    }

    private GameObject GetFromPools (GameObject pref)
    {
        var pool = _pollers.Find(pooler => pooler.Prefab == pref);

        if (pool == null)
        {
            var newPool = new ObjectPooller(pref);
            _pollers.Add(newPool);
            return newPool.Get();
        }
        else
        {
            return pool.Get();
        }
    }

    public GameObject Get(GameObject pref)
    {
        return GetFromPools(pref);
    }

    //private void RemoveFromPool(GameObject pref)
    //{
    //    var pool = _pollers.Find(pooler => pooler.Prefab == pref);

    //    if (pool == null)
    //    {
    //        var newPool = new ObjectPooller(pref);
    //        _pollers.Add(newPool);
    //        newPool.Remove(pref);
    //    }
    //    else
    //    {
    //        pool.Remove(pref);
    //    }
    //}

    //public void Remove(GameObject obj)
    //{
    //    RemoveFromPool(obj);
    //}
}
