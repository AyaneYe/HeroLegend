using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIManager : MonoBehaviour
{
    public PlayerStatus playerStatus;

    [Header("事件监听")]
    public CharacterEventSO healthEvent;

    private void OnEnable()
    {
        //添加事件订阅
        healthEvent.OnEventRaised += UpdateHealth;
    }

    private void OnDisable()
    {
        //取消事件订阅
        healthEvent.OnEventRaised -= UpdateHealth;
    }

    private void UpdateHealth(Character character)
    {
        var percentage = character.CurrentHealth / character.MaxHealth;
        playerStatus.OnHealthChange(percentage);
    }
}
