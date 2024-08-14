using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIManager : MonoBehaviour
{
    public PlayerStatus playerStatus;

    [Header("�¼�����")]
    public CharacterEventSO healthEvent;

    private void OnEnable()
    {
        //����¼�����
        healthEvent.OnEventRaised += UpdateHealth;
    }

    private void OnDisable()
    {
        //ȡ���¼�����
        healthEvent.OnEventRaised -= UpdateHealth;
    }

    private void UpdateHealth(Character character)
    {
        var percentage = character.CurrentHealth / character.MaxHealth;
        playerStatus.OnHealthChange(percentage);
    }
}
