using System;
using UnityEngine;

public class PlayerHealth : MonoBehaviour
{
    [SerializeField] private float maxHealth = 100f;
    public float MaxHealth => maxHealth;
    private float currentHealth;
    public float CurrentHealth => currentHealth;
    private bool isDead = false;
    public event Action OnPlayerDead;
    public event Action OnPlayerHpChanged;
    private void Awake()
    {
        currentHealth = maxHealth;
    }

    public void TakeDamage(float damage)
    {
        if (isDead || damage <= 0) return;
        currentHealth = Mathf.Max(0, currentHealth - damage);
        OnPlayerHpChanged?.Invoke();
        if (currentHealth <= 0) Die();
    }
    private void Die()
    {
        isDead = true;
        OnPlayerDead?.Invoke();
    }
}
