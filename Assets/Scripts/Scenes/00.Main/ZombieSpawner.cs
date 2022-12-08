﻿using System.Collections.Generic;
using UnityEngine;

// 좀비 게임 오브젝트를 주기적으로 생성
public class ZombieSpawner : MonoBehaviour
{
    void Awake()
    {
        instance = this;
    }

    void OnEnable()
    {
        wave = 0;
    }

    void Update()
    {
        // 게임 오버 상태일때는 생성하지 않음
        if (GameManager.instance != null && GameManager.instance.isGameover)
            return;

        // 좀비를 모두 물리친 경우 다음 스폰 실행
        if (zombies.Count <= 0)
            SpawnWave();

        // UI 갱신
        UpdateUI();
    }

    // 웨이브 정보를 UI로 표시
    void UpdateUI()
    {
        // 현재 웨이브와 남은 적 수 표시
        UIManager.instance.UpdateWaveText(wave, zombies.Count);
    }

    // 현재 웨이브에 맞춰 좀비들을 생성
    void SpawnWave()
    {
        wave++;

        int spawnCount = Mathf.RoundToInt(wave * 1.5f);
        for (int i = 0; i < spawnCount; i++)
            CreateZombie();
    }

    // 좀비를 생성하고 생성한 좀비에게 추적할 대상을 할당
    void CreateZombie()
    {
        var spawnPoint = spawnPoints[Random.Range(0, spawnPoints.Length)];
        if (ObjectPool.instance.GetObject(typeof(Zombie).Name, out PoolObject obj))
        {
            var zombie = obj as Zombie;
            zombie.transform.position = spawnPoint.position;
            zombie.transform.rotation = spawnPoint.rotation;
            zombie.Setup(zombieDatas[Random.Range(0, zombieDatas.Length)]);
            zombie.gameObject.SetActive(true);
            zombies.Add(zombie);

            zombie.onDeath += () => zombies.Remove(zombie);
            zombie.onDeath += () => ObjectPool.instance.ReturnObject(zombie, 10f);
            zombie.onDeath += () => GameManager.instance.AddScore(100);
        }
    }

    public static ZombieSpawner instance = null;

    public ZombieData[] zombieDatas; // 사용할 좀비 셋업 데이터들
    public Transform[] spawnPoints; // 좀비 AI를 소환할 위치들

    HashSet<Zombie> zombies = new HashSet<Zombie>(); // 생성된 좀비들을 담는 리스트
    int wave; // 현재 웨이브
}