using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Character : MonoBehaviour
{
    [Header("������Ϣ")]
    public float MaxHealth;
    public float CurrentHealth;
    [Header("�����޵�")]
    public float invincibleTime;
    private float invincibleCounter;
    public bool invincible;

    public UnityEvent<Transform> onTakeDamage;
    public UnityEvent OnDie;

    private void Start()
    {
        CurrentHealth = MaxHealth;
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
            // ���������¼�
            onTakeDamage?.Invoke(attacker.transform);
        }
        else
        {
            CurrentHealth = 0;
            OnDie?.Invoke();
        }
    }

    //�޵�״̬
    public void triggerInvincible()
    {
        if (!invincible)
        {
            invincible = true;
            invincibleCounter = invincibleTime;
        }
    }
}
