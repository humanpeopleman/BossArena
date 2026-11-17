using UnityEngine;

public class EnemyMovement : MonoBehaviour
{
    [SerializeField] private float moveSpeed = 2f;
    [SerializeField] private Transform player;

    private Rigidbody2D rb;
    private EnemyAttack enemyAttack;
    private Vector2 movement;

    private void Awake()
    {
        // Rigidbody2D 참조
        rb = GetComponent<Rigidbody2D>();
        // EnemyAttack 컴포넌트 참조
        enemyAttack = GetComponent<EnemyAttack>();
    }

    private void Update()
    {
        // 플레이어와의 거리 계산
        float distanceToPlayer = Vector2.Distance(transform.position, player.position);
        // 정지 거리보다 멀면 이동 방향 계산
        if (distanceToPlayer > enemyAttack.AttackRange)
        {
            movement = (player.position - transform.position).normalized;
        }
        // 가까우면 이동 방향 초기화
        else
        {
            movement = Vector2.zero;
        }
    }

    private void FixedUpdate()
    {
        // Rigidbody2D를 이용한 이동
        rb.MovePosition(rb.position + movement * moveSpeed * Time.fixedDeltaTime);
    }
}