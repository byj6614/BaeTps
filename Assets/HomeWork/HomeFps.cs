using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class HomeFps : MonoBehaviour
{
    [SerializeField] Transform cameraRoot;      //카메라가 고정될 위치
    [SerializeField] float mouseSensitivity;    //마우스 움직임 속도

    private Vector2 lookDelta;  //마우스의 이동 방향 벡터
    private float xRotation;    //x축 회전 float
    private float yRotation;    //y축 회전 float

    private void OnEnable() 
    {
        Cursor.lockState = CursorLockMode.Locked; //마우스가 화면에 고정 시키는 방법
    }
    private void OnDisable()
    {
        Cursor.lockState = CursorLockMode.None; //ESC를 눌러서 마우스 고정을 푸는 방법
    }

    private void LateUpdate()
    {
        Look();  //1인칭 카메라가 움직이는 함수
    }

    private void Look()
    {
        yRotation += lookDelta.x * mouseSensitivity * Time.deltaTime; // y축으로 움직이는 마우스의 값
        xRotation -= lookDelta.y * mouseSensitivity * Time.deltaTime; // x축으로 움직이는 마우스의 값 (-를 주는 이유는 +로 주었을 경우마우스 반대 방향으로 카메라가 움직이기 때문)
        xRotation = Mathf.Clamp(xRotation, -80f, 80f); //카메라 시점이 위아래 지정 범위 밖으로 나가지 않게 절대값을 보내주는 방법

        cameraRoot.localRotation = Quaternion.Euler(xRotation, 0, 0); //카메라의 위치만 바꾸는 방법
        transform.localRotation = Quaternion.Euler(0, yRotation, 0); //y축은 캐릭터가 보는 방향으로 가는 방향도 바꿔야 하기때문에 transform으로 방향을 바꾼다
    }
    private void OnLook(InputValue value)
    {
        lookDelta = value.Get<Vector2>(); //마우스 이동방향 벡터를 받는 방법
    }
}
