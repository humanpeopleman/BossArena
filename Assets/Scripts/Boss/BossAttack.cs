using UnityEngine;

public class BossAttack : MonoBehaviour
{
    // 공격 패턴 열거형
    private enum AttackPattern
    {
        Charge,
        Shockwave,
        FanSweep
    }
    private AttackPattern currentPattern;

    // 컴포넌트 참조
    private Rigidbody2D rb;

    // 플레이어 충돌 처리
    private Collider2D bossCollider;
    private Collider2D playerCollider;

    // 플레이어 공격 판정
    private bool hasHitPlayerDuringCharge;
    // 공격 대상
    [SerializeField] private Transform player;

    // 공통 공격 설정   
    [SerializeField] private float attackDamage = 20f;
    private bool isAttackFinished = false;

    // 돌진 설정
    [SerializeField] private float chargeSpeed = 8f;
    [SerializeField] private float chargeDuration = 0.7f;
    [SerializeField] private float chargeRadius = 1.2f;
    private bool isCharging = false;

    // 충격파 설정 
    [SerializeField] private float shockwaveRadius = 3f;
    [SerializeField] private LayerMask playerLayer;
    [SerializeField] private GameObject shockwaveIndicator;

    // 벽 충돌 검사
    private ContactFilter2D wallFilter;
    [SerializeField] private LayerMask wallLayer;
    [SerializeField] private float wallCheckDistance = 0.02f;
    private RaycastHit2D[] hitResults = new RaycastHit2D[5];

    private Vector2 chargeDirection;
    private float chargeTimer;

    public bool IsAttackFinished => isAttackFinished; // 공격 중인지 여부를 외부에서 확인할 수 있도록 프로퍼티 제공
    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        bossCollider = GetComponent<Collider2D>();
        if (rb == null || player == null || bossCollider == null)
        {
            Debug.LogError("필요한 컴포넌트를 찾을 수 없습니다.");
            enabled = false; // 컴포넌트 비활성화
            return;
        }
        playerCollider = player.GetComponent<Collider2D>();
        if (playerCollider == null)
        {
            Debug.LogError("플레이어의 Collider2D를 찾을 수 없습니다.");
            enabled = false; // 컴포넌트 비활성화
            return;
        }
        InitializeWallFilter(); 
    }
    private void FixedUpdate()
    {
        if (isCharging)
        {
            if (chargeTimer >= chargeDuration)
            {
                EndCharge();
                return;
            }
            MoveCharge();
            chargeTimer += Time.fixedDeltaTime;
        }
    }
    private void ApplyDamage(Collider2D target)
    {
        PlayerHealth playerHealth = target.GetComponent<PlayerHealth>();
        if (playerHealth != null)
        {
            playerHealth.TakeDamage(attackDamage);
        }
    }
    // 현재 선택된 공격 패턴
    // 추후 Charge, Shockwave, FanSweep 추가

    // 공격 패턴 선택
    public void SelectAttack()
    {
        AttackPattern[] patterns = { 
            AttackPattern.Charge, 
            AttackPattern.Shockwave
        };
        int index = Random.Range(0, patterns.Length);
        currentPattern = patterns[index];
    }
    // 선택한 공격 실행
    public void ExecuteAttack()
    {
        isAttackFinished = false;
        switch (currentPattern)
        {
            case AttackPattern.Charge:
                ChargeAttack();
                break;
            case AttackPattern.Shockwave:
                ShockwaveAttack();
                break;
            case AttackPattern.FanSweep:
                FanSweepAttack();
                break;
            default:
                Debug.LogWarning("알 수 없는 공격 패턴입니다.");
                break;
        }
    }
    public void StartTelegraph()
    {
        switch (currentPattern)
        {
            case AttackPattern.Charge:
                Debug.Log("보스가 돌진 공격을 예고합니다.");
                break;
            case AttackPattern.Shockwave:
                ShowShockwaveIndicator();
                Debug.Log("보스가 충격파 공격을 예고합니다.");
                break;
            case AttackPattern.FanSweep:
                Debug.Log("보스가 부채꼴 공격을 예고합니다.");
                break;
            default:
                Debug.LogWarning("알 수 없는 공격 패턴입니다.");
                break;
        }
    }
    private void ChargeAttack()
    {
        // 돌진 공격
        hasHitPlayerDuringCharge = false;
        SetPlayerCollision(false); // 플레이어와의 충돌 무시
        chargeDirection = (player.position - transform.position).normalized;
        isCharging = true;
        chargeTimer = 0f;
    }
    private void MoveCharge()
    {
        float moveDistance = chargeSpeed * Time.fixedDeltaTime;
        bool isWallHit;
        moveDistance = CheckWallCollision(moveDistance, out isWallHit);
        CheckPlayerHit(moveDistance);
        if (isWallHit) EndCharge();
        rb.MovePosition(rb.position + chargeDirection * moveDistance);

    }
    private void EndCharge()
    {
        isCharging = false;
        isAttackFinished = true;
        SetPlayerCollision(true); // 플레이어와의 충돌 복원
    }


  
    private float CheckWallCollision(float moveDistance, out bool isWallHit)
    {
        isWallHit = false;
        int hitCount = rb.Cast(chargeDirection, wallFilter, hitResults, moveDistance);
        if (hitCount > 0)
        {
            if (Vector2.Dot(hitResults[0].normal, chargeDirection) < 0)
            {
                isWallHit = true;
            }
            for (int i = 0; i < hitCount; i++)
            {
                moveDistance = Mathf.Max(Mathf.Min(moveDistance, hitResults[i].distance - wallCheckDistance), 0f);
            }
        }
        return moveDistance;
    }
    private void SetPlayerCollision(bool enabled)
    {
        Physics2D.IgnoreCollision(bossCollider, playerCollider, !enabled);
    }
    private void CheckPlayerHit(float moveDistance)
    {
        // 이미 피해를 줬다면 검사하지 않기
        if (hasHitPlayerDuringCharge) return;
        // 돌진 경로에서 플레이어와 겹치는지 검사
        RaycastHit2D hit = Physics2D.CircleCast(bossCollider.bounds.center, chargeRadius, chargeDirection, moveDistance, playerLayer);
        // 플레이어에게 피해 적용
        if (hit.collider != null && hit.collider.CompareTag("Player"))
        {
            ApplyDamage(hit.collider);
            hasHitPlayerDuringCharge = true; // 피해를 줬음을 기록
        }
    }
    private void ShockwaveAttack()
    {
        // 충격파 공격
        Debug.Log("보스가 충격파 공격을 실행합니다.");
        HideShockwaveIndicator();
        Collider2D collider = Physics2D.OverlapCircle(transform.position, shockwaveRadius, playerLayer);
        if (collider != null)
        {
            ApplyDamage(collider);
        }
        isAttackFinished = true; // 공격 완료 처리
    }
    private void ShowShockwaveIndicator()
    {
        // 예고 원 활성화 및 위치 설정
        if (shockwaveIndicator != null)
        {
            shockwaveIndicator.SetActive(true);
        }
    }

    private void HideShockwaveIndicator()
    {
        // 예고 원 비활성화
        if (shockwaveIndicator != null)
        {
            shockwaveIndicator.SetActive(false);
        }
    }
    private void FanSweepAttack()
    {
        // 부채꼴 공격
        Debug.Log("보스가 부채꼴 공격을 실행합니다.");
    }
    private void InitializeWallFilter()
    {
        wallFilter = new ContactFilter2D();
        wallFilter.SetLayerMask(wallLayer);
        wallFilter.useTriggers = false;
    }
}
