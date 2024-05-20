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
    private CapsuleCollider2D coll;
    private PlayerAnimation playerAnimation;
    [Header("基础信息")]
    public Vector2 InputDirection;
    public float speed;
    public float jumpForce;

    //下蹲部分(暂未实现)
    public bool isCrouch;
    private Vector2 originalOffset;
    private Vector2 originalSize;

    //受击反弹
    public float hurtForce;
    public bool isHurt;
    public bool isDead;
    public bool isAttack;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        inputControl = new PlayerInputController();
        physicsCheck = GetComponent<PhysicsCheck>();
        coll = GetComponent<CapsuleCollider2D>();
        playerAnimation = GetComponent<PlayerAnimation>();

        originalOffset = coll.offset;
        originalSize = coll.size;

        inputControl.Gameplay.Jump.started += Jump;

        //攻击
        inputControl.Gameplay.Attack.started += PlayerAttack;
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
        if (!isHurt)
        {
            Move();
        }

    }

    private void Move()
    {
        if (!isCrouch)
        {
            //更改刚体的移动可以不用deltaTime，而TransForm则需要
            rb.velocity = new Vector2(speed * Time.deltaTime * InputDirection.x, rb.velocity.y);
        }

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

        //下蹲
        isCrouch = InputDirection.y < -0.5f && physicsCheck.isGrounded;
        if (isCrouch)
        {
            coll.offset = new Vector2(-0.05f, 0.85f);
            coll.size = new Vector2(0.7f, 1.7f);
        }
        else
        {
            coll.size = originalSize;
            coll.offset = originalOffset;
        }
    }

    private void Jump(InputAction.CallbackContext obj)
    {
        if (physicsCheck.isGrounded)
            rb.AddForce(transform.up * jumpForce, ForceMode2D.Impulse); //施加一个向上瞬时的力
    }

    private void PlayerAttack(InputAction.CallbackContext context)
    {
        playerAnimation.PlayerAttack();
        isAttack = true;
    }


    public void GetHurt(Transform attacker)
    {
        isHurt = true;
        rb.velocity = Vector2.zero; //让人物停下来
        //方向计算的话，用当前人物坐标的x，减去攻击者坐标的x
        Vector2 dir = new Vector2((transform.position.x - attacker.position.x), 0).normalized;  //数值归一化，因为距离的远近会导致力的大小不一样

        rb.AddForce(dir * hurtForce, ForceMode2D.Impulse);
    }

    public void PlayerDead()
    {
        isDead = true;
        inputControl.Gameplay.Disable();
    }
}
