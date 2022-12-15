using ExitGames.Client.Photon;
using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// 좀비 게임 오브젝트를 주기적으로 생성
public class ZombieSpawner : MonoBehaviourPun, IPunObservable
{
    void Awake()
    {
        instance = this;

        PhotonPeer.RegisterType(typeof(Color), 128, ColorSerialization.SerializeColor, ColorSerialization.DeserializeColor);
    }

    void OnEnable()
    {
        wave = 0;
        zombieCount = 0;
    }

    void Update()
    {
        if (PhotonNetwork.IsMasterClient)
        {
            // 게임 오버 상태일때는 생성하지 않음
            if (GameManager.instance != null && GameManager.instance.isGameover)
                return;

            if (zombieCount <= 0)
                SpawnWave();
        }

        // UI 갱신
        UpdateUI();
    }

    public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
        if (stream.IsWriting)
        {
            stream.SendNext(zombieCount);
            stream.SendNext(wave);
        }
        else
        {
            _zombieCount = (int)stream.ReceiveNext();
            wave = (int)stream.ReceiveNext();
        }
    }

    // 웨이브 정보를 UI로 표시
    void UpdateUI()
    {
        if (PhotonNetwork.IsMasterClient)
            // 현재 웨이브와 남은 적 수 표시
            UIManager.instance.UpdateWaveText(wave, zombieCount);
        else
            UIManager.instance.UpdateWaveText(wave, _zombieCount);
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
        var zombieData = zombieDatas[Random.Range(0, zombieDatas.Length)];
        var spawnPoint = spawnPoints[Random.Range(0, spawnPoints.Length)];
        var zombie = PhotonNetwork.Instantiate(zombiePrefab.gameObject.name, spawnPoint.position, spawnPoint.rotation);
        var zombieComponent = zombie.GetComponent<Zombie>();

        zombieComponent.photonView.RPC("Setup", RpcTarget.All, zombieData.health, zombieData.damage, zombieData.speed, zombieData.skinColor);
        ++zombieCount;
    }

    public static ZombieSpawner instance = null;

    public Zombie zombiePrefab;
    public ZombieData[] zombieDatas; // 사용할 좀비 셋업 데이터들
    public Transform[] spawnPoints; // 좀비 AI를 소환할 위치들
    public float spawnTime = 20f;
    public int zombieCount;

    int wave; // 현재 웨이브
    int _zombieCount;
}