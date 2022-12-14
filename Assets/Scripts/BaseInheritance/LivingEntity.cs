using System;
using Photon.Pun;
using UnityEngine;

// 생명체로서 동작할 게임 오브젝트들을 위한 뼈대를 제공
// 체력, 데미지 받아들이기, 사망 기능, 사망 이벤트를 제공
public class LivingEntity : PoolObject, IDamageable
{
    // 생명체가 활성화될때 상태를 리셋
    protected virtual void OnEnable()
    {
        // 사망하지 않은 상태로 시작
        dead = false;
        // 체력을 시작 체력으로 초기화
        health = startingHealth;
    }

    /// <summary>
    /// 호스트->모든 클라이언트 방향으로 체력과 사망 상태를 동기화하는 메서드.
    /// </summary>
    /// <param name="health"></param>
    /// <param name="dead"></param>
    [PunRPC] // 이렇게 선언되면 다른 클라이언트에서 원격 실행 가능해짐
    public void ApplyUpdatedHealth(float health, bool dead)
    {
        this.health = health;
        this.dead = dead;
    }

    /// <summary>
    /// 데미지 처리.
    /// 호스트에서 먼저 단독 실행되고, 호스트를 통해 다른 클라이언트에서 일괄 실행됨.
    /// </summary>
    /// <param name="damage"></param>
    /// <param name="hitPoint"></param>
    /// <param name="hitNormal"></param>
    [PunRPC]
    public virtual void OnDamage(float damage, Vector3 hitPoint, Vector3 hitNormal)
    {
        // 마스터 클라이언트(서버)인 경우 실행
        if (PhotonNetwork.IsMasterClient)
        {
            // 데미지만큼 체력 감소
            health -= damage;

            // 호스트에서 클라이언트로 동기화
            photonView.RPC("ApplyUpdatedHealth", RpcTarget.Others, health, dead);
            // 다른 클라이언트도 OnDamage를 실행하도록 함
            photonView.RPC("OnDamage", RpcTarget.Others, damage, hitPoint, hitNormal);
        }

        // 체력이 0 이하 && 아직 죽지 않았다면 사망 처리 실행
        if (health <= 0 && !dead)
            Die();
    }

    /// <summary>
    /// 체력을 회복하는 기능.
    /// </summary>
    /// <param name="healAmount"></param>
    [PunRPC]
    public virtual void RestoreHealth(float healAmount)
    {
        // 이미 사망한 경우 체력을 회복할 수 없음
        if (dead) return;

        // 호스트만 체력을 직접 갱신 가능
        if (PhotonNetwork.IsMasterClient)
        {
            // 체력 추가
            health += healAmount;
            // 서버에서 클라이언트로 동기화
            photonView.RPC("ApplyUpdatedHealth", RpcTarget.Others, health, dead);
            // 다른 클라이언트도 RestoreHealth를 실행하도록 함
            photonView.RPC("RestoreHealth", RpcTarget.Others, healAmount);
        }
    }

    // 사망 처리
    public virtual void Die()
    {
        // 사망 상태를 참으로 변경
        dead = true;
    }

    public float startingHealth = 100f; // 시작 체력
    public float health { get; protected set; } // 현재 체력
    public bool dead { get; protected set; } // 사망 상태
}