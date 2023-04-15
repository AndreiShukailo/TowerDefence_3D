using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectPooller
{
    public GameObject Prefab;
    private List<GameObject> _pool;

    public ObjectPooller (GameObject prefab)
    {
        Prefab = prefab;
        _pool = new List<GameObject>();
    }

    private GameObject CreateObject()
    {
        var rez = GameObject.Instantiate(Prefab);
        rez.SetActive(true);
        _pool.Add(rez);
        return rez;
    }

    public GameObject Get()
    {
        GameObject rez = _pool.Find(o => o.activeSelf == false);

        if (rez != null)
        {
            rez.SetActive(true);
        }
        else
        {
            rez = CreateObject();
        }

        return rez;
    }
}
