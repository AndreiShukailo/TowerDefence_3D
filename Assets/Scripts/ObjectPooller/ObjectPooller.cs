using System.Collections.Generic;
using UnityEngine;

public class ObjectPooller<T> where T : MonoBehaviour
{
    public GameObject Prefab;
    public T Component;
    private List<T> _pool;

    public ObjectPooller(T component)
    {
        Component = component;
        Prefab = component.gameObject;
        _pool = new List<T>();
    }

    private T CreateObject(T component)
    {
        var rez = GameObject.Instantiate(component);
        _pool.Add(rez);
        return rez;
    }

    public T Get(T component)
    {
        T rez = _pool.Find(o => o.gameObject.activeSelf == false);

        if (rez != null)
        {
            rez.gameObject.SetActive(true);
        }
        else
        {
            rez = CreateObject(component);
        }

        return rez;
    }
}
