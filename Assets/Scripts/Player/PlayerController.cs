using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    public PlayerInputController inputControl;
    private Rigidbody2D rb;
    private PhysicsCheck physicsCheck;
    [Header("基础信息")]
    public Vector2 InputDirection;
    public float speed;
    public float jumpForce;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        inputControl = new PlayerInputController();
        physicsCheck = GetComponent<PhysicsCheck>();

        inputControl.Gameplay.Jump.started += Jump;
    }

    private void OnEnable()
    {
        inputControl.Enable();
    }

    private void OnDisable()
    {
        inputControl.Disable();
    }

    private void Update()
    {
        InputDirection = inputControl.Gameplay.Move.ReadValue<Vector2>();
        if (inputControl.Gameplay.ChangeSpeed.triggered)
        {
            speed = speed > 400 ? 300 : 500;
        }
    }

    //与物理相关的要放到FixedUpdate中
    private void FixedUpdate()
    {
        Move();

    }

    private void Move()
    {

        //更改刚体的移动可以不用deltaTime，而TransForm则需要
        rb.velocity = new Vector2(speed * Time.deltaTime * InputDirection.x, rb.velocity.y);

        int faceDir = (int)transform.localScale.x;

        if (InputDirection.x > 0)
        {
            faceDir = 1;
        }
        else if (InputDirection.x < 0)
        {
            faceDir = -1;
        }
        //不用else是因为InputDirection.x可能为0，导致人物面朝同一方向

        transform.localScale = new Vector3(faceDir, 1, 1);
    }

    private void Jump(InputAction.CallbackContext obj)
    {
        if (physicsCheck.isGrounded)
            rb.AddForce(transform.up * jumpForce, ForceMode2D.Impulse); //施加一个向上瞬时的力
    }

}
