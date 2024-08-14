using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.DualShock;

public class Sign : MonoBehaviour
{
    public PlayerInputController playerInput;
    public Transform playerTrans;
    private Animator anim;
    public GameObject signSprite;
    private bool canPress;

    private void Awake()
    {
        anim = signSprite.GetComponent<Animator>();

        playerInput = new PlayerInputController();
        playerInput.Enable();
    }

    private void OnEnable()
    {
        //�����¼�
        InputSystem.onActionChange += OnActionChange;
    }

    private void OnActionChange(object obj, InputActionChange actionChange)
    {
        //���豸�л�ʱ��ͨ���豸���жϲ����Ŷ�Ӧ����ʾ����
        if (actionChange == InputActionChange.ActionStarted)
        {
            var d = ((InputAction)obj).activeControl.device;
            switch (d.device)
            {
                case Keyboard:
                    anim.Play("Keyboard");
                    break;
                case DualShockGamepad:
                    anim.Play("Playstation");
                    break;
            }
        }
    }

    private void Update()
    {
        //ֱ�ӿ������忪���ᵼ���޷�������������������ֱ�ӿ���SpriteRenderer
        signSprite.GetComponent<SpriteRenderer>().enabled = canPress;
        //���ڽ����ʾ�ᷭת������
        signSprite.transform.localScale = playerTrans.localScale;
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        //ͨ��ƥ��Tag���ж�
        if (other.CompareTag("Interactable"))
        {
            canPress = true;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        //�뿪�ɴ�������ʱ�ر���ʾ
        canPress = false;
    }
}
