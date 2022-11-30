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
        Rotate();

        animator.SetFloat(moveHash, playerInput.move);
    }

    // 입력값에 따라 캐릭터를 앞뒤로 움직임
    void Move()
    {
        var moveDistance = playerInput.move * transform.forward * moveSpeed * Time.fixedDeltaTime;
        rigid.MovePosition(rigid.position + moveDistance); // 전역 위치로 이동시켜주는 함수
        // transform.position += moveDistance; 로 하지 않은 이유: 물리 처리를 무시하게 됨
    }

    // 입력값에 따라 캐릭터를 좌우로 회전
    void Rotate()
    {
        /*var worldMousePoint = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        Debug.Log(Input.mousePosition);
        Debug.Log(worldMousePoint);
        transform.rotation = Quaternion.Euler(new Vector3(worldMousePoint.x, 1f, worldMousePoint.z));*/
        //transform.LookAt(new Vector3(worldMousePoint.x - Camera.main.transform.rotation.x, worldMousePoint.y - Camera.main.transform.rotation.y, 1f));
        //var turnDistance = playerInput.rotate * rotateSpeed * Time.fixedDeltaTime;
    }

    public float moveSpeed = 5f; // 앞뒤 움직임의 속도
    public float rotateSpeed = 180f; // 좌우 회전 속도

    PlayerInput playerInput; // 플레이어 입력을 알려주는 컴포넌트
    Rigidbody rigid; // 플레이어 캐릭터의 리지드바디
    Animator animator; // 플레이어 캐릭터의 애니메이터

    int moveHash;
}