using UnityEngine;

public class PlayerHealth : MonoBehaviour
{
    [SerializeField] private float maxHealth = 100f;
    private float currentHealth;
    private bool isDead = false;

    private void Awake()
    {
        currentHealth = maxHealth;
    }

    public void TakeDamage(float damage)
    {
        if (isDead || damage <= 0) return;
        currentHealth = Mathf.Max(0, currentHealth - damage);
        Debug.Log($"플레이어가 {damage}만큼 피해를 입었습니다. 현재 체력: {currentHealth}");
        if (currentHealth <= 0)
        {
            Die();
        }
    }
    private void Die()
    {
        isDead = true;
        Debug.Log("플레이어가 사망했습니다.");
        // 사망 처리 로직 추가 (예: 게임 오버 화면, 리스폰 등)
    }
}
