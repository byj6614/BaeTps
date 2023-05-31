using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class HomeFps : MonoBehaviour
{
    [SerializeField] Transform cameraRoot;      //ī�޶� ������ ��ġ
    [SerializeField] float mouseSensitivity;    //���콺 ������ �ӵ�

    private Vector2 lookDelta;  //���콺�� �̵� ���� ����
    private float xRotation;    //x�� ȸ�� float
    private float yRotation;    //y�� ȸ�� float

    private void OnEnable() 
    {
        Cursor.lockState = CursorLockMode.Locked; //���콺�� ȭ�鿡 ���� ��Ű�� ���
    }
    private void OnDisable()
    {
        Cursor.lockState = CursorLockMode.None; //ESC�� ������ ���콺 ������ Ǫ�� ���
    }

    private void LateUpdate()
    {
        Look();  //1��Ī ī�޶� �����̴� �Լ�
    }

    private void Look()
    {
        yRotation += lookDelta.x * mouseSensitivity * Time.deltaTime; // y������ �����̴� ���콺�� ��
        xRotation -= lookDelta.y * mouseSensitivity * Time.deltaTime; // x������ �����̴� ���콺�� �� (-�� �ִ� ������ +�� �־��� ��츶�콺 �ݴ� �������� ī�޶� �����̱� ����)
        xRotation = Mathf.Clamp(xRotation, -80f, 80f); //ī�޶� ������ ���Ʒ� ���� ���� ������ ������ �ʰ� ���밪�� �����ִ� ���

        cameraRoot.localRotation = Quaternion.Euler(xRotation, 0, 0); //ī�޶��� ��ġ�� �ٲٴ� ���
        transform.localRotation = Quaternion.Euler(0, yRotation, 0); //y���� ĳ���Ͱ� ���� �������� ���� ���⵵ �ٲ�� �ϱ⶧���� transform���� ������ �ٲ۴�
    }
    private void OnLook(InputValue value)
    {
        lookDelta = value.Get<Vector2>(); //���콺 �̵����� ���͸� �޴� ���
    }
}
