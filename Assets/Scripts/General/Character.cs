using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Character : MonoBehaviour
{
    [Header("基础信息")]
    public float MaxHealth;
    public float CurrentHealth;
    [Header("受伤无敌")]
    public float invincibleTime;
    private float invincibleCounter;
    public bool invincible;

    public UnityEvent<Character> onHealthChange;
    public UnityEvent<Transform> onTakeDamage;
    public UnityEvent OnDie;

    private void Start()
    {
        CurrentHealth = MaxHealth;
        onHealthChange?.Invoke(this);
    }

    private void Update()
    {
        if (invincible)
        {
            invincibleCounter -= Time.deltaTime;
            if (invincibleCounter <= 0)
            {
                invincible = false;
            }
        }
    }

    private void OnTriggerStay2D(Collider2D other)
    {
        if (other.CompareTag("Water"))
        {
            CurrentHealth = 0;
            onHealthChange?.Invoke(this);
            OnDie?.Invoke();
        }
    }

    public void TakeDamage(Attack attacker)
    {
        if (invincible)
        {
            return;
        }
        if (CurrentHealth - attacker.damage > 0)
        {
            CurrentHealth -= attacker.damage;
            triggerInvincible();
            // 触发受伤事件
            onTakeDamage?.Invoke(attacker.transform);
        }
        else
        {
            CurrentHealth = 0;
            OnDie?.Invoke();
        }

        onHealthChange?.Invoke(this);
    }

    //无敌状态
    public void triggerInvincible()
    {
        if (!invincible)
        {
            invincible = true;
            invincibleCounter = invincibleTime;
        }
    }
}
