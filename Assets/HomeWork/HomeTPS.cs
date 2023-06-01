using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class HomeTPS : MonoBehaviour
{
    [SerializeField] Transform cameraRoot; //chinema 카메라가 따라갈 위치
    [SerializeField] float mouseSensitivity; //마우스 이동 스피드값
    [SerializeField] Transform aimTarget;
    private Vector2 lookDelta; //마우스 이동 방향 값 
    private float xRotation; //x축으로 이동할 값
    private float yRotation; //y축으로 이동할 값

    private void OnEnable()
    {
        Cursor.lockState = CursorLockMode.Locked;   //마우스가 화면에 고정하는 방법
    }
    private void OnDisable()
    {
        Cursor.lockState = CursorLockMode.None;     //고정된 마우스 해제 방법
    }
    private void Update()
    {
        Rotate();   //캐릭터가 보는 방향 함수
    }
    private void LateUpdate()
    {
        Look();     //내가 보는 화면의 함수
    }
    private void Rotate()       //이해안감
    {
        Vector3 lookPoint = Camera.main.transform.position + Camera.main.transform.forward * 20f;
        aimTarget.position = lookPoint;
        lookPoint.y = transform.position.y;        
        transform.LookAt(lookPoint);
    }
    private void Look()
    {
        yRotation += lookDelta.x * mouseSensitivity * Time.deltaTime;       // y축으로 움직이는 마우스의 값
        xRotation -= lookDelta.y * mouseSensitivity * Time.deltaTime;       // x축으로 움직이는 마우스의 값
        xRotation = Mathf.Clamp(xRotation, -80f, 80f);      //x축의 화면이 지정 범위 밖으로 안나가게 하는 방법

        cameraRoot.rotation = Quaternion.Euler(xRotation, yRotation, 0); //Rotate에서 캐릭터가 보는 방향을 가르키니 카메라로 x축 y축을 바꿔준다
    }
    private void OnLook(InputValue value)
    {
        lookDelta = value.Get<Vector2>(); // 마우스의 방향을 받는 방법
    }
}
