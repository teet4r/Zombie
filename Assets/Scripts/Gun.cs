using System.Collections;
using UnityEngine;

// 총을 구현
public class Gun : MonoBehaviour
{
    void Awake()
    {
        #region Initialize BulletLineRenderer
        bulletLineRenderer = GetComponent<LineRenderer>();

        bulletLineRenderer.positionCount = 2;
        bulletLineRenderer.enabled = false;
        #endregion
    }

    void OnEnable()
    {
        ammoRemain = gunData.startAmmoRemain;
        magAmmo = gunData.magCapacity;
        state = State.Ready;
        lastFireTime = 0f;
    }

    // 발사 시도
    public void Fire()
    {
        if (state == State.Ready && Time.time >= lastFireTime + gunData.timeBetFire)
        {
            lastFireTime = Time.time;
            Shot();
        }
    }

    // 재장전 시도
    public bool Reload()
    {
        if (state == State.Reloading || ammoRemain <= 0 || magAmmo >= gunData.magCapacity)
            return false;

        StartCoroutine(ReloadRoutine());
        return true;
    }

    // 실제 발사 처리
    void Shot()
    {
        // 레이캐스트에 의한 충돌 정보를 저장하는 컨테이너
        RaycastHit hit;
        // 탄알이 맞은 곳을 저장할 변수
        Vector3 hitPosition = Vector3.zero;

        // Raycast(시작 지점, 방향, 충돌 정보 컨테이너, 사정거리)
        // 레이가 어떤 물체와 충돌한 경우
        if (Physics.Raycast(fireTransform.position, fireTransform.forward, out hit, fireDistance))
        {
            // 충돌한 상대방으로부터 IDamageable 오브젝트 가져오기 시도
            if (hit.collider.TryGetComponent(out IDamageable target))
                target.OnDamage(gunData.damage, hit.point, hit.normal);

            hitPosition = hit.point;
        }
        // 레이가 다른 물체와 충돌하지 않았다면
        // 탄알이 최대 사정거리까지 날아갔을 때의 위치를 충돌 위치로 사용
        else
            hitPosition = fireTransform.position + fireTransform.forward * fireDistance;

        StartCoroutine(ShotEffect(hitPosition));
        magAmmo--;
        if (magAmmo <= 0)
            state = State.Empty;
    }

    // 발사 이펙트와 소리를 재생하고 탄알 궤적을 그림
    IEnumerator ShotEffect(Vector3 hitPosition)
    {
        muzzleFlashEffect.Play();
        shellEjectEffect.Play();

        SoundManager.instance.sfxAudio.Play(gunData.sfxShot);

        // 선의 시작점은 총구의 위치
        bulletLineRenderer.SetPosition(0, fireTransform.position);
        // 선의 끝점은 입력으로 들어온 충돌 위치
        bulletLineRenderer.SetPosition(1, hitPosition);

        // 라인 렌더러를 활성화하여 탄알 궤적을 그림
        bulletLineRenderer.enabled = true;
        // 0.03초 동안 잠시 처리를 대기
        yield return new WaitForSeconds(0.03f);
        // 라인 렌더러를 비활성화하여 탄알 궤적을 지움
        bulletLineRenderer.enabled = false;
    }

    // 실제 재장전 처리를 진행
    IEnumerator ReloadRoutine()
    {
        // 현재 상태를 재장전 중 상태로 전환
        state = State.Reloading;
        SoundManager.instance.sfxAudio.Play(gunData.sfxReload);

        // 재장전 소요 시간 만큼 처리 쉬기
        yield return new WaitForSeconds(gunData.reloadTime);

        int ammoToFill = gunData.magCapacity - magAmmo;
        // 탄창에 채워야 할 탄알이 남은 탄알보다 많다면
        // 채워야 할 탄알 수를 남은 탄알 수에 맞춰 줄임
        if (ammoRemain < ammoToFill)
            ammoToFill = ammoRemain;
        magAmmo += ammoToFill; // 탄창을 채움
        ammoRemain -= ammoToFill;

        // 총의 현재 상태를 발사 준비된 상태로 변경
        state = State.Ready;
    }

    // 총의 상태를 표현하는 데 사용할 타입을 선언
    public enum State
    {
        Ready, // 발사 준비됨
        Empty, // 탄알집이 빔
        Reloading // 재장전 중
    }

    public State state { get; private set; } // 현재 총의 상태
    public Transform fireTransform; // 탄알이 발사될 위치
    public GunData gunData; // 총의 현재 데이터
    public ParticleSystem muzzleFlashEffect;
    public ParticleSystem shellEjectEffect;
    
    public int ammoRemain = 100; // 남은 전체 탄알
    public int magAmmo; // 현재 탄알집에 남아 있는 탄알

    LineRenderer bulletLineRenderer; // 탄알 궤적을 그리기 위한 렌더러

    float fireDistance = 50f; // 사정거리
    float lastFireTime; // 총을 마지막으로 발사한 시점
}