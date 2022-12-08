using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// PoolObject Ÿ���� �����յ��� �����ϴ� ������Ʈ Ǯ
/// PoolObject Ÿ���� ����ϱ⸸ �ϸ� �ȴ�.
/// </summary>
public class ObjectPool : MonoBehaviour
{
    void Awake()
    {
        instance = this;

        Initialize();
    }

    /// <summary>
    /// PoolObjectŸ�Ը� ��ȯ
    /// </summary>
    /// <param name="typeName"></param>
    /// <param name="obj"></param>
    /// <returns></returns>
    public bool GetObject(string typeName, out PoolObject obj)
    {
        if (!dict.ContainsKey(typeName))
        {
            obj = null;
            return false;
        }

        if (pools[typeName].Count == 0)
        {
            var clone = Instantiate(dict[typeName]);
            clone.gameObject.SetActive(false);
            obj = clone;
        }
        else
            obj = pools[typeName].Dequeue();
        return true;
    }

    public bool GetRandomObject(string typeName, out PoolObject obj)
    {
        if (!typePool.ContainsKey(typeName))
        {
            obj = null;
            return false;
        }

        int idx = UnityEngine.Random.Range(0, typePool[typeName].Count);
        if (GetObject(typePool[typeName][idx], out PoolObject _obj))
        {
            obj = _obj;
            return true;
        }
        else
        {
            obj = null;
            return false;
        }
    }

    /// <summary>
    /// PoolObjectŸ�� ��ȯ,
    /// time�� �Ŀ� ��ȯ��.
    /// </summary>
    /// <param name="obj"></param>
    /// <param name="time"></param>
    public void ReturnObject(PoolObject obj, float time = 0f)
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
            SaveAllTypes(prefabs[i], prefabs[i].GetType());
        }
    }

    void SaveAllTypes(PoolObject obj, Type objType)
    {
        var objTypeName = objType.Name;
        if (!typePool.ContainsKey(objTypeName))
            typePool.Add(objTypeName, new List<string>());
        typePool[objTypeName].Add(objTypeName);

        if (objTypeName.CompareTo(poolObject) == 0)
            return;
        
        SaveAllTypes(obj, objType.BaseType);
    }

    public static ObjectPool instance = null;

    [SerializeField]
    PoolObject[] prefabs;

    readonly string poolObject = typeof(PoolObject).Name;

    Dictionary<string, PoolObject> dict = new Dictionary<string, PoolObject>();
    Dictionary<string, Queue<PoolObject>> pools = new Dictionary<string, Queue<PoolObject>>();
    Dictionary<string, List<string>> typePool = new Dictionary<string, List<string>>();
}
