using Photon.Pun;
using UnityEngine;

// 주어진 Gun 오브젝트를 쏘거나 재장전
// 알맞은 애니메이션을 재생하고 IK를 사용해 캐릭터 양손이 총에 위치하도록 조정
public class PlayerShooter : MonoBehaviourPun
{
    void Awake()
    {
        // 사용할 컴포넌트들을 가져오기
        playerInput = GetComponent<PlayerInput>();
        animator = GetComponent<Animator>();
    }

    void OnEnable()
    {
        // 슈터가 활성화될 때 총도 함께 활성화
        gun.gameObject.SetActive(true);
    }

    void OnDisable()
    {
        // 슈터가 비활성화될 때 총도 함께 비활성화
        gun.gameObject.SetActive(false);
    }

    void Update()
    {
        if (!photonView.IsMine) return;

        // 입력을 감지하고 총 발사하거나 재장전
        if (playerInput.fire)
            gun.Fire();
        else if (playerInput.reload)
            // 장전에 성공했을때만
            if (gun.Reload())
                // 애니메이션 실행
                animator.SetTrigger(AnimatorID.Trigger.RELOAD);

        UpdateAmmoUI();
    }

    // 탄약 UI 갱신
    void UpdateAmmoUI()
    {
        if (gun != null && UIManager.instance != null)
            // UI 매니저의 탄약 텍스트에 탄창의 탄약과 남은 전체 탄약을 표시
            UIManager.instance.UpdateAmmoText(gun.magAmmo, gun.ammoRemain);
    }

    // 애니메이터의 IK 갱신
    // 1. 총을 상체와 함께 흔들기
    // 2. 캐릭터의 양손을 총의 양쪽 손잡이에 위치시키기
    void OnAnimatorIK(int layerIndex)
    {
        // 총의 기준점을 3D모델의 오른쪽 팔꿈치 위치로 이동
        // AvatarIKHint: IK 대상 중 특정 부위를 표현하는 타입
        gunPivot.position = animator.GetIKHintPosition(AvatarIKHint.RightElbow);

        // IK를 사용하여 왼손의 위치와 회전을 총의 왼쪽 손잡이에 맞춤
        animator.SetIKPositionWeight(AvatarIKGoal.LeftHand, 1f);
        animator.SetIKRotationWeight(AvatarIKGoal.LeftHand, 1f);

        animator.SetIKPosition(AvatarIKGoal.LeftHand, leftHandMount.position);
        animator.SetIKRotation(AvatarIKGoal.LeftHand, leftHandMount.rotation);

        // IK를 사용하여 오른손의 위치와 회전을 총의 오른쪽 손잡이에 맞춤
        animator.SetIKPositionWeight(AvatarIKGoal.RightHand, 1f);
        animator.SetIKRotationWeight(AvatarIKGoal.RightHand, 1f);

        animator.SetIKPosition(AvatarIKGoal.RightHand, rightHandMount.position);
        animator.SetIKRotation(AvatarIKGoal.RightHand, rightHandMount.rotation);
    }

    public Gun gun; // 사용할 총
    public Transform gunPivot; // 총 배치의 기준점
    public Transform leftHandMount; // 총의 왼쪽 손잡이, 왼손이 위치할 지점
    public Transform rightHandMount; // 총의 오른쪽 손잡이, 오른손이 위치할 지점

    PlayerInput playerInput; // 플레이어의 입력
    Animator animator; // 애니메이터 컴포넌트
}