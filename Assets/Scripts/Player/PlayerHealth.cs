using System;
using UnityEngine;

public class PlayerHealth : MonoBehaviour
{
    [SerializeField] private float maxHealth = 100f;
    public float MaxHealth => maxHealth;
    private float currentHealth;
    public float CurrentHealth => currentHealth;
    private bool isDead = false;
    private float invincibilityTimer = 0;
    private bool isInvincible = false;
    public bool IsInvincible => isInvincible;
    [SerializeField] private float invincibilityDuration = 0.5f;


    public event Action OnPlayerDead;
    public event Action OnPlayerHpChanged;
    private void Update()
    {
        if (isInvincible)
        {
            invincibilityTimer += Time.deltaTime;
            if (invincibilityTimer >= invincibilityDuration)
            {
                isInvincible = false;
                invincibilityTimer = 0;
            }
        }
    }
    private void Awake()
    {
        currentHealth = maxHealth;
    }

    public void TakeDamage(float damage)
    {
        if (isDead || damage <= 0 || IsInvincible) return;
        currentHealth = Mathf.Max(0, currentHealth - damage);
        OnPlayerHpChanged?.Invoke();
        if (currentHealth <= 0)
        {
            Die();
            return;
        }
        invincibilityTimer = 0;
        isInvincible = true;
    }
    private void Die()
    {
        isDead = true;
        OnPlayerDead?.Invoke();
    }
}
