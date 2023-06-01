using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMover : MonoBehaviour
{
    [SerializeField] private float moveSpeed;
    [SerializeField] private float jumpSpeed;

    private Animator ani;
    private CharacterController controller;
    private Vector3 moveDir;
    private float ySpeed = 0;

    private void Awake()
    {
        ani = GetComponent<Animator>();
        controller = GetComponent<CharacterController>();
    }

    private void Update()
    {
        Move();
        Jump();
    }
    
    private void Move()
    {
       //월드기준 움직임
        //controller.Move(moveDir*moveSpeed*Time.deltaTime);

        //로컬기준 움직임
        controller.Move(transform.forward * moveDir.z * moveSpeed * Time.deltaTime);
        controller.Move(transform.right * moveDir.x * moveSpeed * Time.deltaTime);
    }

    private void OnMove(InputValue value)
    {
        Vector2 input = value.Get<Vector2>();
        moveDir = new Vector3(input.x,0,input.y);

        ani.SetFloat("XSpeed", input.x,0.1f,Time.deltaTime);
        ani.SetFloat("YSpeed", input.y,0.1f,Time.deltaTime);
    }

    private void Jump()
    {
        ySpeed += Physics.gravity.y * Time.deltaTime;

        if(GroundCheck()&&ySpeed<0)
            ySpeed = 0;

        controller.Move(Vector3.up*ySpeed*Time.deltaTime);
    }

    private void OnJump(InputValue value)
    {
        if (GroundCheck())
             ySpeed = jumpSpeed; 
    }

    private bool GroundCheck()
    {
        RaycastHit hit;
        return Physics.SphereCast(transform.position+Vector3.up*1,0.5f,Vector3.down,out hit,0.6f);
    }
}
