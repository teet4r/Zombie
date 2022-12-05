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

    /*public Zombie GetZombie()
    {
        if (zombieQ.Count == 0)
        {
            var zombie = Instantiate(zombiePrefab);
            zombie.gameObject.SetActive(false);
            return zombie;
        }
        return zombieQ.Dequeue();
    }*/

    public void ReturnZombie(Zombie zombie, float time)
    {
        if (zombie == null) return;
        StartCoroutine(_ReturnZombie(zombie, time));
    }

    IEnumerator _ReturnZombie(Zombie zombie, float time)
    {
        yield return new WaitForSeconds(time);
        zombie.gameObject.SetActive(false);
        zombieQ.Enqueue(zombie);
    }

    public LivingEntity GetLivingEntity<T>()
    {
        var type = typeof(T);
        if (type != typeof(LivingEntity))
            return null;
        if (!dictionary.ContainsKey(type))
            dictionary[type] = new Queue<LivingEntity>();
        if (dictionary[type].Count == 0)
        {
            var obj = Instantiate(prefabs[0]);
            obj.gameObject.SetActive(false);
            return obj;
        }
        return dictionary[type].Dequeue();
    }

    public void ReturnLivingEntity(LivingEntity livingEntity)
    {
        if (livingEntity == null) return;
        livingEntity.gameObject.SetActive(false);
        dictionary[typeof(LivingEntity)].Enqueue(livingEntity);
    }

    public static ObjectPool instance = null;

    [Header("Prefabs")]
    [SerializeField]
    LivingEntity[] prefabs;

    Queue<Zombie> zombieQ = new Queue<Zombie>();
    Dictionary<Type, Queue<LivingEntity>> dictionary = new Dictionary<Type, Queue<LivingEntity>>();
}
