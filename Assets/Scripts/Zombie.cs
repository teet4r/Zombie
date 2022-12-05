﻿using System.Collections;
using UnityEngine;
using UnityEngine.AI; // AI, 내비게이션 시스템 관련 코드 가져오기

// 좀비 AI 구현
public class Zombie : LivingEntity
{
    protected override void Awake()
    {
        base.Awake();

        // 초기화
        navMeshAgent = GetComponent<NavMeshAgent>();
        animator = GetComponent<Animator>();
    }

    protected override void OnEnable()
    {
        base.OnEnable();

        navMeshAgent.enabled = true;

        // 게임 오브젝트 활성화와 동시에 AI의 추적 루틴 시작
        StartCoroutine(UpdatePath());
    }

    void Update()
    {
        // 추적 대상의 존재 여부에 따라 다른 애니메이션 재생
        animator.SetBool(AnimatorID.Bool.HAS_TARGET, hasTarget);
    }

    void OnTriggerStay(Collider other)
    {
        // 트리거 충돌한 상대방 게임 오브젝트가 추적 대상이라면 공격 실행
        if (!dead && Time.time >= lastAttackTime + timeBetAttack)
        {
            if (other.TryGetComponent(out livingEntity2))
                if (livingEntity2 == targetEntity)
                {
                    lastAttackTime = Time.time;

                    // 상대방의 피격 위치와 피격 방향을 근삿값으로 계산
                    livingEntity2.OnDamage(
                        damage,
                        other.ClosestPoint(transform.position),
                        transform.position - other.transform.position
                    );
                }
        }
    }

    // 좀비 AI의 초기 스펙을 결정하는 셋업 메서드
    public void Setup(ZombieData zombieData)
    {
        startingHealth = zombieData.health;
        health = startingHealth;
        damage = zombieData.damage;
        navMeshAgent.speed = zombieData.speed;
        m_renderer.material.color = zombieData.skinColor;
    }

    // 데미지를 입었을 때 실행할 처리
    public override void OnDamage(float damage, Vector3 hitPoint, Vector3 hitNormal)
    {
        if (dead) return;

        // 공격받은 지점과 방향으로 파티클 효과 재생
        hitEffect.transform.position = hitPoint;
        hitEffect.transform.rotation = Quaternion.LookRotation(hitNormal); // 피격 방향으로 향하도록 회전
        hitEffect.Play();

        SoundManager.instance.sfxAudio.Play(Sfx.ZombieDamaged);

        // LivingEntity의 OnDamage()를 실행하여 데미지 적용
        base.OnDamage(damage, hitPoint, hitNormal);
    }

    // 사망 처리
    public override void Die()
    {
        // LivingEntity의 Die()를 실행하여 기본 사망 처리 실행
        base.Die();

        // 다른 AI를 방해하지 않도록 자신의 모든 콜라이더를 비활성화
        EnableColliders(false);

        // AI 추적을 중지하고 navMesh 컴포넌트 비활성화
        navMeshAgent.isStopped = true;
        navMeshAgent.enabled = false;

        animator.SetTrigger(AnimatorID.Trigger.DIE);
        SoundManager.instance.sfxAudio.Play(Sfx.ZombieDie);
    }

    // 주기적으로 추적할 대상의 위치를 찾아 경로 갱신
    IEnumerator UpdatePath()
    {
        WaitForSeconds wfs = new WaitForSeconds(updatePathRate);
        // 살아 있는 동안 무한 루프
        while (!dead)
        {
            if (hasTarget)
            {
                navMeshAgent.isStopped = false; // AI의 이동을 계속 진행
                navMeshAgent.SetDestination(targetEntity.transform.position);
            }
            else
            {
                navMeshAgent.isStopped = true; // 추적 대상이 없으면 AI 이동 중지
                // 20유닛의 반지름을 가진 가상의 구를 그렸을 때 구와 겹치는 모든 콜라이더를 가져옴
                // 단, whatIsTarget 레이어를 가진 콜라이더만 가져오도록 필터링
                var colliders = Physics.OverlapSphere(transform.position, 20f, whatIsTarget);
                for (int i = 0; i < colliders.Length; i++)
                    if (colliders[i].TryGetComponent(out livingEntity1))
                        if (!livingEntity1.dead)
                        {
                            targetEntity = livingEntity1;
                            break;
                        }
            }

            // 0.25초 주기로 처리 반복
            yield return wfs;
        }
    }

    public float damage = 20f; // 공격력
    public float timeBetAttack = 0.5f; // 공격 간격

    public LayerMask whatIsTarget; // 추적 대상 레이어
    public ParticleSystem hitEffect; // 피격 시 재생할 파티클 효과

    [SerializeField]
    Renderer m_renderer; // 렌더러 컴포넌트
    [SerializeField]
    float updatePathRate = 0.25f;

    LivingEntity targetEntity; // 추적 대상
    LivingEntity livingEntity1; // tryGetcomponent에서 쓰일 임시 변수
    LivingEntity livingEntity2; // tryGetcomponent에서 쓰일 임시 변수
    NavMeshAgent navMeshAgent; // 경로 계산 AI 에이전트
    Animator animator; // 애니메이터 컴포넌트

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