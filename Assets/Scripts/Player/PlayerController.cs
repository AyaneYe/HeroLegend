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
    [Header("������Ϣ")]
    public Vector2 InputDirection;
    public float speed;
    public float jumpForce;

    //�¶ײ���(��δʵ��)
    public bool isCrouch;
    private Vector2 originalOffset;
    private Vector2 originalSize;

    //�ܻ�����
    public float hurtForce;
    public bool isHurt;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        inputControl = new PlayerInputController();
        physicsCheck = GetComponent<PhysicsCheck>();
        coll = GetComponent<CapsuleCollider2D>();

        originalOffset = coll.offset;
        originalSize = coll.size;

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

    //��������ص�Ҫ�ŵ�FixedUpdate��
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
            //���ĸ�����ƶ����Բ���deltaTime����TransForm����Ҫ
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
        //����else����ΪInputDirection.x����Ϊ0�����������泯ͬһ����

        transform.localScale = new Vector3(faceDir, 1, 1);

        //�¶�
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
            rb.AddForce(transform.up * jumpForce, ForceMode2D.Impulse); //ʩ��һ������˲ʱ����
    }

    public void GetHurt(Transform attacker)
    {
        isHurt = true;
        rb.velocity = Vector2.zero; //������ͣ����
        //�������Ļ����õ�ǰ���������x����ȥ�����������x
        Vector2 dir = new Vector2((transform.position.x - attacker.position.x), 0).normalized;  //��ֵ��һ������Ϊ�����Զ���ᵼ�����Ĵ�С��һ��

        rb.AddForce(dir * hurtForce, ForceMode2D.Impulse);
    }
}
