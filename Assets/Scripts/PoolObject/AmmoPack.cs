using Photon.Pun;
using UnityEngine;

// 총알을 충전하는 아이템
public class AmmoPack : ItemObject
{
    public override void Use(GameObject target)
    {
        if (target.TryGetComponent(out PlayerShooter playerShooter) && playerShooter.gun != null)
            playerShooter.gun.photonView.RPC("AddAmmo", RpcTarget.All, ammo + (int)(ammo * (Random.Range(-errorMaxRate, errorMaxRate) / 100)));
        PhotonNetwork.Destroy(gameObject);
    }

    public int ammo = 50; // 충전할 총알 수
    [Tooltip("ammo 최대 오차율(%)")]
    [Range(0f, 100f)]
    public float errorMaxRate = 20f;
}