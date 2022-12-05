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
        StartCoroutine(_ReturnZombie(zombie, time));
    }

    IEnumerator _ReturnZombie(Zombie zombie, float time)
    {
        yield return new WaitForSeconds(time);
        zombie.gameObject.SetActive(false);
        zombieQ.Enqueue(zombie);
    }
    #endregion

    public static ObjectPool instance = null;

    [Header("Prefabs")]
    [SerializeField]
    Zombie zombiePrefab;

    Queue<Zombie> zombieQ = new Queue<Zombie>();
}
