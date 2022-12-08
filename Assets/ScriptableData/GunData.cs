using UnityEngine;

[CreateAssetMenu(fileName = "GunData", menuName = "Scriptable/GunData")]
public class GunData : ScriptableObject
{
    public Sfx sfxShot; // 발사 소리
    public Sfx sfxReload; // 재장전 소리

    public float damage = 25; // 공격력

    public int startAmmoRemain = 100; // 처음에 주어질 전체 탄약
    public int magCapacity = 25; // 탄창 용량

    public float timeBetFire = 0.12f; // 총알 발사 간격
    public float reloadTime = 1.8f; // 재장전 소요 시간
}