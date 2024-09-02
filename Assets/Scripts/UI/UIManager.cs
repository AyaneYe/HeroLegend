using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIManager : MonoBehaviour
{
    public PlayerStatus playerStatus;

    [Header("事件监听")]
    public CharacterEventSO healthEvent;
    public SceneLoadEventSO loadEvent;

    private void OnEnable()
    {
        //添加事件订阅
        healthEvent.OnEventRaised += UpdateHealth;
        loadEvent.LoadRequestEvent += OnLoadEvent;
    }

    private void OnDisable()
    {
        //取消事件订阅
        healthEvent.OnEventRaised -= UpdateHealth;
        loadEvent.LoadRequestEvent -= OnLoadEvent;
    }

    private void OnLoadEvent(GameSceneSO sceneToLoad, Vector3 arg1, bool arg2)
    {
        var isMenu = sceneToLoad.SceneType == SceneType.Menu;
        playerStatus.gameObject.SetActive(!isMenu);
    }


    private void UpdateHealth(Character character)
    {
        var percentage = character.CurrentHealth / character.MaxHealth;
        playerStatus.OnHealthChange(percentage);
    }
}
