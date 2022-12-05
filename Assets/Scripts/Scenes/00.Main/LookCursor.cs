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
        // ���콺�� ��ġ�� ScreenPointToRay�� �̿��� ī�޶�κ����� ��ũ���� ���� ���� ���̸� ��ȯ�Ѵ�.
        Ray cameraRay = currentCamera.ScreenPointToRay(Input.mousePosition);
        // ���� ��ǥ�� �ϴ� ���⿡ ũ�Ⱑ 1�� �������Ϳ� ������ ���´�.
        Plane GroupPlane = new Plane(Vector3.up, Vector3.zero);

        // cameraRay�� ���� �����ߴ��� ���θ� üũ
        if (GroupPlane.Raycast(cameraRay, out float rayLength))
        {
            // rayLenghth�Ÿ��� ��ġ���� ��ȯ�Ѵ�.
            m_lookingPoint = cameraRay.GetPoint(rayLength);
            transform.LookAt(new Vector3(m_lookingPoint.x, transform.position.y, m_lookingPoint.z));
        }
    }

    public Camera currentCamera;
    public Vector3 lookingPoint { get { return m_lookingPoint; } }

    Vector3 m_lookingPoint;
}
