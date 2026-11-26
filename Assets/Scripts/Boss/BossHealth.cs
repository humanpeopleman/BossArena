using System;
using UnityEngine;

public class BossHealth : MonoBehaviour
{
    // 체력 설정
    [SerializeField] private float maxHealth = 500f;
    public float MaxHealth => maxHealth;

    // 현재 상태
    private float currentHealth;
    public float CurrentHealth => currentHealth;
    private bool isDead;
    public event Action OnBossDead;
    public event Action OnBossHpChanged;

    private void Awake()
    {
        // 체력 초기화
        currentHealth = maxHealth;
    }

    public void TakeDamage(float damage)
    {
        // 사망 상태 및 데미지 양수인지 확인
        if (isDead || damage <= 0) return;
        // 체력 감소
        currentHealth = Mathf.Max(0, currentHealth - damage);
        OnBossHpChanged?.Invoke();
        // 체력이 0 이하라면 사망 처리
        if (currentHealth <= 0) Die();
    }

    private void Die()
    {
        // 사망 처리
        isDead = true;
        OnBossDead?.Invoke();
    }
}
