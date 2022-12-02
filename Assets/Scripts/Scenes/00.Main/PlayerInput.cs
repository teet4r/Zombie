using UnityEngine;

// 플레이어 캐릭터를 조작하기 위한 사용자 입력을 감지
// 감지된 입력값을 다른 컴포넌트들이 사용할 수 있도록 제공
public class PlayerInput : MonoBehaviour
{
    // 매프레임 사용자 입력을 감지
    void Update()
    {
        if (GameManager.instance == null) return;
        // 게임오버 상태에서는 사용자 입력을 감지하지 않는다
        if (GameManager.instance.isGameover)
        {
            fbMove = 0f;
            lrMove = 0f;
//            rotate = 0;
            fire = false;
            reload = false;
            return;
        }

        // move에 관한 입력 감지
        fbMove = Input.GetAxis(fbMoveAxisName);
        lrMove = Input.GetAxis(lrMoveAxisName);
        // rotate에 관한 입력 감지
//        rotate = Input.GetAxis(rotateAxisName);
        // fire에 관한 입력 감지
        fire = Input.GetButton(fireButtonName);
        // reload에 관한 입력 감지
        reload = Input.GetButtonDown(reloadButtonName);
    }
    
    string fbMoveAxisName = InputManager.VERTICAL; // 앞뒤 움직임을 위한 입력축 이름
    string lrMoveAxisName = InputManager.HORIZONTAL; // 좌우 움직임을 위한 입력축 이름
//    string rotateAxisName = InputManager.HORIZONTAL; // 좌우 회전을 위한 입력축 이름
    string fireButtonName = InputManager.FIRE1; // 발사를 위한 입력 버튼 이름
    string reloadButtonName = InputManager.RELOAD; // 재장전을 위한 입력 버튼 이름

    // 값 할당은 내부에서만 가능
    public float fbMove { get; private set; } // 감지된 앞뒤 움직임 입력값
    public float lrMove { get; private set; } // 감지된 좌우 움직임 입력값
//    public float rotate { get; private set; } // 감지된 회전 입력값
    public bool fire { get; private set; } // 감지된 발사 입력값
    public bool reload { get; private set; } // 감지된 재장전 입력값
}