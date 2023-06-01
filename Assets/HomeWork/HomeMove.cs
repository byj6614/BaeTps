using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Windows;

public class HomeMove : MonoBehaviour
{
    [SerializeField] private float walkSpeed;       //ĳ������ ������ �ӵ�
    [SerializeField] private float runSpeed;
    [SerializeField] private float jumpSpeed;       //ĳ������ ���� �Ŀ�

    private Animator ani;
    private CharacterController controller;     //ĳ���� ��Ʈ���� �ҷ�����
    private Vector3 moveDir;        //�������� ������ �޴� ����
    private float moveSpeed;
    private float ySpeed = 0;       //������ �ϰ��� ���ǵ�
    private bool isWalking;
    private void Awake()
    {
        ani = GetComponent<Animator>();
        controller = GetComponent<CharacterController>(); //ĳ���� ��Ʈ���� �ҷ��´�
    }

    private void Update()
    {
        //�� �Է¿� ���� �ż��� ������ �������� �ҷ��´�.
        Move();    
        Jump();
    }

    private void Move()
    {

        //����� �������� ���� ������
        //������� ������
        //controller.Move(moveDir*moveSpeed*Time.deltaTime);

        //���� ������ ĳ������ z�� �������� ���� ������
        //���ñ��� ������
        if(moveDir.magnitude==0)//�ȿ�����
        {
            moveSpeed = Mathf.Lerp(moveSpeed,0,0.5f);
        }
        else if(isWalking)//�����̴µ� ����
        {
            moveSpeed = Mathf.Lerp(moveSpeed, walkSpeed, 0.5f);
        }
        else//�����̴µ� ��
        {
            moveSpeed = Mathf.Lerp(moveSpeed, runSpeed, 0.5f);
        }
        controller.Move(transform.forward * moveDir.z * moveSpeed * Time.deltaTime);
        controller.Move(transform.right * moveDir.x * moveSpeed * Time.deltaTime);

        ani.SetFloat("XSpeed", moveDir.x, 0.1f, Time.deltaTime);
        ani.SetFloat("YSpeed", moveDir.z, 0.1f, Time.deltaTime);
        ani.SetFloat("Speed", moveSpeed);
    }

    private void OnMove(InputValue value)   //�������� �޴� �Լ�
    {
        Vector2 input = value.Get<Vector2>();
        moveDir = new Vector3(input.x, 0, input.y);

        ani.SetFloat("XSpeed", input.x, 0.1f, Time.deltaTime);
        ani.SetFloat("YSpeed", input.y, 0.1f, Time.deltaTime);
    }

    private void Jump()     //������ �ڿ������� ��ġ �ϱ� ���� �Լ�
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

    private bool GroundCheck()//ĳ���Ͱ� ���� �پ��ִ��� Ȯ���ϴ� �Լ�(true,false)�� ��ȯ�Ͽ� Ȯ��
    {
        RaycastHit hit;//Raycast�� ���� ����
        return Physics.SphereCast(transform.position + Vector3.up * 1, 0.5f, Vector3.down, out hit, 0.6f);
        //                        ������ġ����         ����3�� ���ʿ�������0.5�ǳʺ�� �Ʒ������� �������� 0.6��ŭ 
        // ������ 0.5�������� ������ ������ �÷��̾��� ������ �߹ٴ��� �������� �ϱ� �����̴�.
    }

    private void OnWalk(InputValue value)
    {
        isWalking = value.isPressed;
    }
}
