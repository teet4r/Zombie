using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// DerivedType, BaseType�� ���� �� �̸� ����ȭ
/// </summary>
public partial class ObjectPool : MonoBehaviour
{
    readonly string[] derivedNames =
    {
        "Zombie", "AmmoPack", "HealthPack", "Coin"
    };
    readonly string[] baseNames =
    {
        "PoolObject", "ItemObject", "LivingEntity"
    };
}

public enum DerivedType
{
    Zombie, AmmoPack, HealthPack, Coin
}

public enum BaseType
{
    PoolObject, ItemObject, LivingEntity
}


/// <summary>
/// PoolObject Ÿ���� �����յ��� �����ϴ� ������Ʈ Ǯ
/// PoolObject Ÿ���� ����ϱ⸸ �ϸ� �ȴ�.
/// </summary>
public partial class ObjectPool : MonoBehaviour
{
    void Awake()
    {
        instance = this;

        Initialize();
    }

    public PoolObject GetObject(DerivedType type)
    {
        var typeName = derivedNames[(int)type];
        if (!pools.ContainsKey(typeName))
            throw new Exception("This type doesn't exist!");

        if (pools[typeName].Count == 0)
        {
            var clone = Instantiate(dict[typeName]);
            clone.gameObject.SetActive(false);
            return clone;
        }
        return pools[typeName].Dequeue();
    }

    public PoolObject GetRandomObject(BaseType type)
    {
        var typeName = baseNames[(int)type];
        if (!typePools.ContainsKey(typeName))
            throw new Exception("This type doesn't exist!");

        int idx = UnityEngine.Random.Range(0, typePools[typeName].Count);
        return GetObject(typePools[typeName][idx]);
    }

    /// <summary>
    /// PoolObjectŸ�� ��ȯ,
    /// time�� �Ŀ� ��ȯ��.
    /// </summary>
    /// <param name="obj"></param>
    /// <param name="time"></param>
    public void ReturnObject(PoolObject obj, float time = 0f)
    {
        StartCoroutine(_ReturnObject(obj, time));
    }

    IEnumerator _ReturnObject(PoolObject obj, float time)
    {
        yield return new WaitForSeconds(time);
        if (obj == null)
            yield break;
        var typeName = obj.GetType().Name;
        if (!dict.ContainsKey(typeName))
            yield break;
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

    PoolObject GetObject(string typeName)
    {
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

    void SaveAllTypes(PoolObject obj, Type objType)
    {
        var objTypeName = objType.Name;
        if (!typePools.ContainsKey(objTypeName))
            typePools.Add(objTypeName, new List<string>());
        typePools[objTypeName].Add(obj.GetType().Name);

        if (objTypeName.CompareTo(poolObject) == 0)
            return;
        
        SaveAllTypes(obj, objType.BaseType);
    }

    public static ObjectPool instance = null;

    [SerializeField]
    PoolObject[] prefabs;

    Dictionary<string, PoolObject> dict = new Dictionary<string, PoolObject>();
    Dictionary<string, Queue<PoolObject>> pools = new Dictionary<string, Queue<PoolObject>>();
    Dictionary<string, List<string>> typePools = new Dictionary<string, List<string>>();

    readonly string poolObject = typeof(PoolObject).Name;
}

/*
 *  �ֻ��� Ÿ��                          PoolObject
 *  ���� Ÿ��        ItemObject                    LivingEntity
 *  ���� Ÿ��        AmmoPack HealthPack Coin      Zombie
 */