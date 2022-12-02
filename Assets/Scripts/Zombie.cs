using System.Collections;
using UnityEngine;
using UnityEngine.AI; // AI, 내비게이션 시스템 관련 코드 가져오기

// 좀비 AI 구현
public class Zombie : LivingEntity
{
    void Awake()
    {
        // 초기화
    }

    void Start()
    {
        // 게임 오브젝트 활성화와 동시에 AI의 추적 루틴 시작
        StartCoroutine(UpdatePath());
    }

    void Update()
    {
        // 추적 대상의 존재 여부에 따라 다른 애니메이션 재생
        zombieAnimator.SetBool("HasTarget", hasTarget);
    }

    void OnTriggerStay(Collider other)
    {
        // 트리거 충돌한 상대방 게임 오브젝트가 추적 대상이라면 공격 실행
    }

    // 좀비 AI의 초기 스펙을 결정하는 셋업 메서드
    public void Setup(ZombieData zombieData)
    {

    }

    // 데미지를 입었을 때 실행할 처리
    public override void OnDamage(float damage, Vector3 hitPoint, Vector3 hitNormal)
    {
        // LivingEntity의 OnDamage()를 실행하여 데미지 적용
        base.OnDamage(damage, hitPoint, hitNormal);
    }

    // 사망 처리
    public override void Die()
    {
        // LivingEntity의 Die()를 실행하여 기본 사망 처리 실행
        base.Die();
    }

    // 주기적으로 추적할 대상의 위치를 찾아 경로 갱신
    IEnumerator UpdatePath()
    {
        // 살아 있는 동안 무한 루프
        while (!dead)
        {
            // 0.25초 주기로 처리 반복
            yield return new WaitForSeconds(0.25f);
        }
    }

    public float damage = 20f; // 공격력
    public float timeBetAttack = 0.5f; // 공격 간격

    public LayerMask whatIsTarget; // 추적 대상 레이어
    public ParticleSystem hitEffect; // 피격 시 재생할 파티클 효과
    public AudioClip deathSound; // 사망 시 재생할 소리
    public AudioClip hitSound; // 피격 시 재생할 소리

    LivingEntity targetEntity; // 추적 대상
    NavMeshAgent navMeshAgent; // 경로 계산 AI 에이전트
    Animator zombieAnimator; // 애니메이터 컴포넌트
    AudioSource zombieAudioPlayer; // 오디오 소스 컴포넌트
    Renderer zombieRenderer; // 렌더러 컴포넌트

    float lastAttackTime; // 마지막 공격 시점
    // 추적할 대상이 존재하는지 알려주는 프로퍼티
    bool hasTarget
    {
        get
        {
            // 추적할 대상이 존재하고, 대상이 사망하지 않았다면 true
            return targetEntity != null && !targetEntity.dead;
        }
    }
}