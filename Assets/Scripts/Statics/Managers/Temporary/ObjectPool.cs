using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectPool : MonoBehaviour
{
    void Awake()
    {
        instance = this;
    }

    #region Zombie
    public Zombie GetZombie()
    {
        if (zombieQ.Count == 0)
        {
            var zombie = Instantiate(zombiePrefab);
            zombie.gameObject.SetActive(false);
            return zombie;
        }
        return zombieQ.Dequeue();
    }

    public void ReturnZombie(Zombie zombie, float time)
    {
        if (zombie == null) return;
        StartCoroutine(_ReturnZombie(zombie, Mathf.Max(time, 0f)));
    }

    IEnumerator _ReturnZombie(Zombie zombie, float time)
    {
        yield return new WaitForSeconds(time);
        zombie.gameObject.SetActive(false);
        zombieQ.Enqueue(zombie);
    }
    #endregion

    public PoolObject GetObject<T>()
    {
        var typeName = typeof(T).Name;
        if (!dict.ContainsKey(typeName))
            return null;
        return null;
    }

    void Initialize()
    {
        for (int i = 0; i < prefabs.Length; i++)
            dict.Add(prefabs[i].GetType().Name, prefabs[i]);
    }

    public static ObjectPool instance = null;

    [Header("Prefabs")]
    [SerializeField]
    Zombie zombiePrefab;
    [SerializeField]
    Coin coinPrefab;
    [SerializeField]
    AmmoPack ammoPackPrefab;
    [SerializeField]
    HealthPack healthPackPrefab;

    [SerializeField]
    PoolObject[] prefabs;

    Dictionary<string, PoolObject> dict = new Dictionary<string, PoolObject>();

    Queue<Zombie> zombieQ = new Queue<Zombie>();
}
