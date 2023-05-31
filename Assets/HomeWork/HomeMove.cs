using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class HomeMove : MonoBehaviour
{
    [SerializeField] private float moveSpeed;       //캐릭터의 움직임 속도
    [SerializeField] private float jumpSpeed;       //캐릭터의 점프 파워

    private CharacterController controller;     //캐릭터 컨트롤을 불러오기
    private Vector3 moveDir;        //움직임의 방향을 받는 벡터
    private float ySpeed = 0;       //점프를 하고서의 스피드

    private void Awake()
    {
        controller = GetComponent<CharacterController>(); //캐릭터 컨트롤을 불러온다
    }

    private void Update()
    {
        //내 입력에 따라 매순간 점프와 움직임을 불러온다.
        Move();    
        Jump();
    }

    private void Move()
    {
        //월드맵 기준으로 인한 움직임
        //월드기준 움직임
        //controller.Move(moveDir*moveSpeed*Time.deltaTime);

        //내가 지정한 캐릭터의 z축 방향으로 인한 움직임
        //로컬기준 움직임
        controller.Move(transform.forward * moveDir.z * moveSpeed * Time.deltaTime);
        controller.Move(transform.right * moveDir.x * moveSpeed * Time.deltaTime);
    }

    private void OnMove(InputValue value)   //움직임을 받는 함수
    {
        Vector2 input = value.Get<Vector2>();
        moveDir = new Vector3(input.x, 0, input.y);
    }

    private void Jump()     //점프가 자연스럽게 착치 하기 위한 함수
    {
        ySpeed += Physics.gravity.y * Time.deltaTime;

        if (GroundCheck() && ySpeed < 0)
            ySpeed = 0;

        controller.Move(Vector3.up * ySpeed * Time.deltaTime);
    }

    private void OnJump(InputValue value)
    {
        if (GroundCheck())
            ySpeed = jumpSpeed;
    }

    private bool GroundCheck()//캐릭터가 땅에 붙어있는지 확인하는 함수(true,false)를 반환하여 확인
    {
        RaycastHit hit;//Raycast를 통해 감지
        return Physics.SphereCast(transform.position + Vector3.up * 1, 0.5f, Vector3.down, out hit, 0.6f);
        //                        지금위치에서         백터3의 위쪽에서부터0.5의너비로 아래쪽으로 내보낸다 0.6만큼 
        // 위에서 0.5위쪽으로 보내는 이유는 플레이어의 기준이 발바닥을 기준으로 하기 때문이다.
    }
}
