using UnityEngine;

public class EnemyAttack : MonoBehaviour
{
    [SerializeField] private float attackRange = 1.5f;
    public float AttackRange => attackRange;
    [SerializeField] private float attackDamage = 10f;
    [SerializeField] private float attackCooldown = 1f;

    private float nextAttackTime = 0f;
    [SerializeField] private Transform player;
    private PlayerHealth playerHealth;
    private void Awake()
    {
        if (player == null || (playerHealth = player.GetComponent<PlayerHealth>()) == null)
        {
            Debug.LogError("플레이어 참조가 올바르지 않습니다.");
            enabled = false;
        }
        playerHealth = player.GetComponent<PlayerHealth>();
    }
    private void Update()
    {
        if (Time.time < nextAttackTime) return;
        if (Vector2.Distance(transform.position, player.position) > attackRange) return;

        Attack();
        nextAttackTime = Time.time + attackCooldown;
    }
    private void Attack()
    {
        playerHealth.TakeDamage(attackDamage);
        Debug.Log($"적이 플레이어에게 {attackDamage}만큼 피해를 입혔습니다.");
    }
}
