using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class HomeTPS : MonoBehaviour
{
    [SerializeField] Transform cameraRoot; //chinema ī�޶� ���� ��ġ
    [SerializeField] float mouseSensitivity; //���콺 �̵� ���ǵ尪
    [SerializeField] Transform aimTarget;
    private Vector2 lookDelta; //���콺 �̵� ���� �� 
    private float xRotation; //x������ �̵��� ��
    private float yRotation; //y������ �̵��� ��

    private void OnEnable()
    {
        Cursor.lockState = CursorLockMode.Locked;   //���콺�� ȭ�鿡 �����ϴ� ���
    }
    private void OnDisable()
    {
        Cursor.lockState = CursorLockMode.None;     //������ ���콺 ���� ���
    }
    private void Update()
    {
        Rotate();   //ĳ���Ͱ� ���� ���� �Լ�
    }
    private void LateUpdate()
    {
        Look();     //���� ���� ȭ���� �Լ�
    }
    private void Rotate()       //���ؾȰ�
    {
        Vector3 lookPoint = Camera.main.transform.position + Camera.main.transform.forward * 20f;
        aimTarget.position = lookPoint;
        lookPoint.y = transform.position.y;        
        transform.LookAt(lookPoint);
    }
    private void Look()
    {
        yRotation += lookDelta.x * mouseSensitivity * Time.deltaTime;       // y������ �����̴� ���콺�� ��
        xRotation -= lookDelta.y * mouseSensitivity * Time.deltaTime;       // x������ �����̴� ���콺�� ��
        xRotation = Mathf.Clamp(xRotation, -80f, 80f);      //x���� ȭ���� ���� ���� ������ �ȳ����� �ϴ� ���

        cameraRoot.rotation = Quaternion.Euler(xRotation, yRotation, 0); //Rotate���� ĳ���Ͱ� ���� ������ ����Ű�� ī�޶�� x�� y���� �ٲ��ش�
    }
    private void OnLook(InputValue value)
    {
        lookDelta = value.Get<Vector2>(); // ���콺�� ������ �޴� ���
    }
}
