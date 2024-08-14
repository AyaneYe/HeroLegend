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
        //订阅事件
        InputSystem.onActionChange += OnActionChange;
    }

    private void OnActionChange(object obj, InputActionChange actionChange)
    {
        //当设备切换时，通过设备名判断并播放对应的提示动画
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
        //直接控制物体开启会导致无法触发动画，所以这里直接控制SpriteRenderer
        signSprite.GetComponent<SpriteRenderer>().enabled = canPress;
        //用于解决提示会翻转的问题
        signSprite.transform.localScale = playerTrans.localScale;
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        //通过匹配Tag来判断
        if (other.CompareTag("Interactable"))
        {
            canPress = true;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        //离开可触发区域时关闭提示
        canPress = false;
    }
}
