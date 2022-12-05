using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LookCursor : MonoBehaviour
{
    void OnEnable()
    {
        if (currentCamera == null)
            currentCamera = Camera.main;
    }

    void FixedUpdate()
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

    public Camera currentCamera;
    public Vector3 lookingPoint { get { return m_lookingPoint; } }

    Vector3 m_lookingPoint;
}
