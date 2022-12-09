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
    public override void RestoreHealth(float healAmount)
    {
        // LivingEntity의 RestoreHealth() 실행 (체력 증가)
        base.RestoreHealth(healAmount);
        healthSlider.value = health;
    }

    // 데미지 처리
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

        GameManager.instance.EndGame();
    }

    void OnTriggerEnter(Collider other)
    {
        // 아이템과 충돌한 경우 해당 아이템을 사용하는 처리
        if (dead) return;

        if (other.TryGetComponent(out IItem item))
        {
            item.Use(gameObject);
            SoundManager.instance.sfxAudio.Play(Sfx.ItemPickUp);
        }
    }

    public Slider healthSlider; // 체력을 표시할 UI 슬라이더

    Animator animator; // 플레이어의 애니메이터
    PlayerMovement playerMovement; // 플레이어 움직임 컴포넌트
    PlayerShooter playerShooter; // 플레이어 슈터 컴포넌트
}