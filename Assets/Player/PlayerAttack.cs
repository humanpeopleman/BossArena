using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerAttack : MonoBehaviour
{
    // 조준 컴포넌트
    private PlayerAim playerAim;

    // 공격 설정
    [SerializeField] private float attackDistance = 1f;
    [SerializeField] private float attackRadius = 0.5f;
    [SerializeField] private float attackCooldown = 0.5f;
    [SerializeField] private float attackDamage = 20f;

    // 공격 상태
    private float nextAttackTime = 0f;
    private Vector2 attackPosition;

    private void Awake()
    {
        playerAim = GetComponent<PlayerAim>();
    }

    private void Update()
    {
        // 공격 위치 갱신
        attackPosition = (Vector2)transform.position + playerAim.AimDirection * attackDistance;
        // 좌클릭 입력 확인
        if (Mouse.current != null && Mouse.current.leftButton.wasPressedThisFrame)
            Attack();
    }

    private void Attack()
    {
        // 쿨다운 확인
        if (Time.time < nextAttackTime)
            return;
        nextAttackTime = Time.time + attackCooldown;
        // 공격 판정 실행
        Collider2D[] hitEnemies = Physics2D.OverlapCircleAll(attackPosition, attackRadius);
        foreach (Collider2D obj in hitEnemies) {
            if (!obj.CompareTag("Enemy")) continue;
            EnemyHealth enemyHealth = obj.GetComponent<EnemyHealth>();
            if (enemyHealth == null) continue;
            enemyHealth.TakeDamage(attackDamage);
            Debug.Log("적 공격");
        }
    }

    private void OnDrawGizmosSelected()
    {
        // 공격 범위 시각화
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(attackPosition, attackRadius);
    }
}
