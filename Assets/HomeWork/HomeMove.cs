using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class HomeMove : MonoBehaviour
{
    [SerializeField] private float moveSpeed;       //ĳ������ ������ �ӵ�
    [SerializeField] private float jumpSpeed;       //ĳ������ ���� �Ŀ�

    private CharacterController controller;     //ĳ���� ��Ʈ���� �ҷ�����
    private Vector3 moveDir;        //�������� ������ �޴� ����
    private float ySpeed = 0;       //������ �ϰ��� ���ǵ�

    private void Awake()
    {
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
        controller.Move(transform.forward * moveDir.z * moveSpeed * Time.deltaTime);
        controller.Move(transform.right * moveDir.x * moveSpeed * Time.deltaTime);
    }

    private void OnMove(InputValue value)   //�������� �޴� �Լ�
    {
        Vector2 input = value.Get<Vector2>();
        moveDir = new Vector3(input.x, 0, input.y);
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
}
