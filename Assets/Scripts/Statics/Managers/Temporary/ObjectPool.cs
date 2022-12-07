using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectPool : MonoBehaviour
{
    void Awake()
    {
        instance = this;

        Initialize();
    }

    public PoolObject GetObject<T>()
    {
        var typeName = typeof(T).Name;
        if (!dict.ContainsKey(typeName))
            return null;

        if (pools[typeName].Count == 0)
        {
            var clone = Instantiate(dict[typeName]);
            clone.gameObject.SetActive(false);
            return clone;
        }
        return pools[typeName].Dequeue();
    }

    public PoolObject GetObject(PoolType type)
    {
        var typeName = poolTypeNames[(int)type];
        if (pools[typeName].Count == 0)
        {
            var clone = Instantiate(dict[typeName]);
            clone.gameObject.SetActive(false);
            return clone;
        }
        return pools[typeName].Dequeue();
    }

    public void ReturnObject(PoolObject obj, float time)
    {
        if (obj == null)
            return;
        var typeName = obj.GetType().Name;
        if (!dict.ContainsKey(typeName))
            return;
        StartCoroutine(_ReturnObject(obj, time, typeName));
    }

    IEnumerator _ReturnObject(PoolObject obj, float time, string typeName)
    {
        yield return new WaitForSeconds(time);
        obj.gameObject.SetActive(false);
        pools[typeName].Enqueue(obj);
    }

    void Initialize()
    {
        for (int i = 0; i < prefabs.Length; i++)
        {
            var typeName = prefabs[i].GetType().Name;
            dict.Add(typeName, prefabs[i]);
            pools.Add(typeName, new Queue<PoolObject>());
        }
    }

    public static ObjectPool instance = null;

    [Header("Prefabs")]
    [SerializeField]
    PoolObject[] prefabs;

    /// <summary>
    /// PoolType과 순서 및 이름 동기화.
    /// 인스펙터와는 달라도 됨
    /// </summary>
    string[] poolTypeNames =
    {
        "Zombie"
    };
    Dictionary<string, PoolObject> dict = new Dictionary<string, PoolObject>();
    Dictionary<string, Queue<PoolObject>> pools = new Dictionary<string, Queue<PoolObject>>();
}

public enum PoolType
{
    Zombie
}