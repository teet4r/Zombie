using Photon.Pun;
using UnityEngine;

// 플레이어 캐릭터를 사용자 입력에 따라 움직이는 스크립트
[RequireComponent(typeof(PlayerInput))]
[RequireComponent(typeof(Rigidbody))]
[RequireComponent(typeof(Animator))]
public class PlayerMovement : MonoBehaviourPun
{
    void Awake()
    {
        instance = this;

        playerInput = GetComponent<PlayerInput>();
        rigid = GetComponent<Rigidbody>();
        animator = GetComponent<Animator>();

        #region Animator.StringToHash()
        moveHash = AnimatorID.Int.MOVE;
        #endregion
    }

    void OnEnable()
    {
        if (currentCamera == null)
            currentCamera = Camera.main;
    }

    // FixedUpdate는 물리 갱신 주기에 맞춰 실행됨
    void FixedUpdate()
    {
        if (!photonView.IsMine) return;

        Move();
        Rotate();

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

    /// <summary>
    /// 마우스 방향으로 회전
    /// </summary>
    void Rotate()
    {
        // 마우스의 위치를 ScreenPointToRay를 이용해 카메라로부터의 스크린의 점을 통해 레이를 반환한다.
        Ray cameraRay = currentCamera.ScreenPointToRay(Input.mousePosition);
        // 월드 좌표로 하늘 방향에 크기가 1인 단위벡터와 원점을 갖는다.
        Plane GroupPlane = new Plane(Vector3.up, Vector3.zero);

        // cameraRay가 평면과 교차했는지 여부를 체크
        if (GroupPlane.Raycast(cameraRay, out float rayLength))
        {
            // rayLenghth거리에 위치값을 반환한다.
            m_lookingPoint = cameraRay.GetPoint(rayLength);
            transform.LookAt(new Vector3(m_lookingPoint.x, transform.position.y, m_lookingPoint.z));
        }
    }

    //void Rotate()
    //{
    //    var turnDistance = playerInput.rotate * rotateSpeed * Time.fixedDeltaTime;
    //    rigid.rotation *= Quaternion.Euler(0f, turnDistance, 0f);
    //}

    public static PlayerMovement instance = null;

    public float moveSpeed = 5f; // 앞뒤 움직임의 속도
    public float rotateSpeed = 180f; // 좌우 회전 속도
    public Camera currentCamera;
    public Vector3 lookingPoint { get { return m_lookingPoint; } }

    PlayerInput playerInput; // 플레이어 입력을 알려주는 컴포넌트
    Rigidbody rigid; // 플레이어 캐릭터의 리지드바디
    Animator animator; // 플레이어 캐릭터의 애니메이터
    Vector3 m_lookingPoint;

    int moveHash;
}