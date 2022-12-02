using UnityEngine;

// 플레이어 캐릭터를 사용자 입력에 따라 움직이는 스크립트
[RequireComponent(typeof(PlayerInput))]
[RequireComponent(typeof(Rigidbody))]
[RequireComponent(typeof(Animator))]
public class PlayerMovement : MonoBehaviour
{
    void Awake()
    {
        playerInput = GetComponent<PlayerInput>();
        rigid = GetComponent<Rigidbody>();
        animator = GetComponent<Animator>();

        #region Animator.StringToHash()
        moveHash = AnimatorID.Int.MOVE;
        #endregion
    }

    // FixedUpdate는 물리 갱신 주기에 맞춰 실행됨
    void FixedUpdate()
    {
        Move();
        //Rotate();

        if (playerInput.fbMove == 0f)
            animator.SetFloat(moveHash, playerInput.lrMove);
        else
            animator.SetFloat(moveHash, playerInput.fbMove);
    }

    // 입력값에 따라 캐릭터를 앞뒤로 움직임
    void Move()
    {
        var moveDistance = (playerInput.fbMove * Vector3.forward + playerInput.lrMove * Vector3.right).normalized * moveSpeed * Time.fixedDeltaTime;
        rigid.MovePosition(rigid.position + moveDistance); // 전역 위치로 이동시켜주는 함수
        // transform.position += moveDistance; 로 하지 않은 이유: 물리 처리를 무시하게 됨
    }

    //void Rotate()
    //{
    //    var turnDistance = playerInput.rotate * rotateSpeed * Time.fixedDeltaTime;
    //    rigid.rotation *= Quaternion.Euler(0f, turnDistance, 0f);
    //}

    public float moveSpeed = 5f; // 앞뒤 움직임의 속도
    public float rotateSpeed = 180f; // 좌우 회전 속도

    PlayerInput playerInput; // 플레이어 입력을 알려주는 컴포넌트
    Rigidbody rigid; // 플레이어 캐릭터의 리지드바디
    Animator animator; // 플레이어 캐릭터의 애니메이터

    int moveHash;
}