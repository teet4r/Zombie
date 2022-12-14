using Photon.Pun;
using UnityEngine;
using UnityEngine.UI; // UI 관련 코드

// 플레이어 캐릭터의 생명체로서의 동작을 담당
public class PlayerHealth : LivingEntity
{
    void Awake()
    {
        animator = GetComponent<Animator>();
        playerMovement = GetComponent<PlayerMovement>();
        playerShooter = GetComponent<PlayerShooter>();
    }

    protected override void OnEnable()
    {
        // LivingEntity의 OnEnable() 실행 (상태 초기화)
        base.OnEnable();

        #region Initialize HealthSlider
        healthSlider.gameObject.SetActive(true);
        healthSlider.maxValue = startingHealth;
        healthSlider.value = health;
        #endregion
        #region Initialize PlayerMovement, PlayerShooter
        playerMovement.enabled = true;
        playerShooter.enabled = true;
        #endregion
    }

    // 체력 회복
    [PunRPC]
    public override void RestoreHealth(float healAmount)
    {
        // LivingEntity의 RestoreHealth() 실행 (체력 증가)
        base.RestoreHealth(healAmount);
        healthSlider.value = health;
    }

    // 데미지 처리
    [PunRPC]
    public override void OnDamage(float damage, Vector3 hitPoint, Vector3 hitDirection)
    {
        if (dead) return;

        SoundManager.instance.sfxAudio.Play(Sfx.PlayerDamaged);
        // LivingEntity의 OnDamage() 실행(데미지 적용)
        base.OnDamage(damage, hitPoint, hitDirection);
        healthSlider.value = health;
    }

    // 사망 처리
    public override void Die()
    {
        // LivingEntity의 Die() 실행(사망 적용)
        base.Die();

        healthSlider.gameObject.SetActive(false);
        SoundManager.instance.sfxAudio.Play(Sfx.PlayerDie);
        animator.SetTrigger(AnimatorID.Trigger.DIE);

        playerMovement.enabled = false;
        playerShooter.enabled = false;

        //GameManager.instance.EndGame();
        // 5초 뒤에 리스폰
        Invoke("Respawn", 5f);
    }

    /// <summary>
    /// 부활 처리
    /// </summary>
    public void Respawn()
    {
        // 로컬인 경우에만 실행
        if (photonView.IsMine)
        {
            // 원점에서 반경 5유닛 내부의 랜덤 위치 지정
            Vector3 randomSpawnPos = Random.insideUnitSphere * 5f;
            randomSpawnPos.y = 0f;

            transform.position = randomSpawnPos;
        }

        // 컴포넌트 리셋을 위해 게임 오브젝트를 잠시 껐다가 다시 켜기
        // 컴포넌트의 OnEnable(), OnDisable() 메서드가 실행됨
        gameObject.SetActive(false);
        gameObject.SetActive(true);
    }

    void OnTriggerEnter(Collider other)
    {
        // 아이템과 충돌한 경우 해당 아이템을 사용하는 처리
        if (dead) return;

        if (other.TryGetComponent(out IItem item))
        {
            // 호스트만 아이템 직접 사용 가능
            // 호스트에서는 아이템 사용 후 사용된 아이템의 효과를 모든 클라이언트에 동기화시킴
            if (PhotonNetwork.IsMasterClient)
                item.Use(gameObject);
            SoundManager.instance.sfxAudio.Play(Sfx.ItemPickUp);
        }
    }

    public Slider healthSlider; // 체력을 표시할 UI 슬라이더

    Animator animator; // 플레이어의 애니메이터
    PlayerMovement playerMovement; // 플레이어 움직임 컴포넌트
    PlayerShooter playerShooter; // 플레이어 슈터 컴포넌트
}