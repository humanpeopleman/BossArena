using UnityEngine;

public class BossController : MonoBehaviour
{
    // 보스 상태
    private enum BossState
    {
        Idle,
        Telegraph,
        Attack,
        Recovery,
        PhaseChange,
        Dead
    }
    // 참조
    private BossAttack bossAttack;

    // 현재 상태
    private BossState currentState;

    // 상태 진행 시간
    private float stateTimer;

    // 상태별 소요 시간
    [SerializeField] private float idleDuration = 1f;
    [SerializeField] private float telegraphDuration = 1f;
    [SerializeField] private float attackDuration = 0.5f;
    [SerializeField] private float recoveryDuration = 1.5f;
    private void Awake()
    {
        bossAttack = GetComponent<BossAttack>();
        if (bossAttack == null)
        {
            Debug.LogError("BossAttack 컴포넌트를 할당해야 합니다.");
            enabled = false; // 컴포넌트 비활성화
        }
    }
    private void Start()
    {
        // 초기 상태 설정
       ChangeState(BossState.Idle);
    }

    private void Update()
    {
        // 현재 상태에 따른 로직 실행
        UpdateState();
    }

    private void ChangeState(BossState newState)
    {
        if (currentState == BossState.Dead) return; // 이미 사망 상태라면 상태 변경 불가
        // 상태 변경
        currentState = newState;
        Debug.Log($"보스 상태 변경: {currentState}");
        stateTimer = 0f;
    }

    private void UpdateState()
    {
        // 상태별 시간 및 전환 조건 처리
        stateTimer += Time.deltaTime;

        switch (currentState)
        {
            case BossState.Idle:
                if (stateTimer >= idleDuration)
                {
                    bossAttack.SelectAttack(); // 공격 패턴 선택
                    bossAttack.StartTelegraph(); // 텔레그래프 시작
                    ChangeState(BossState.Telegraph);
                }
                break;
            case BossState.Telegraph:
                if (stateTimer >= telegraphDuration)
                {
                    bossAttack.ExecuteAttack(); // 선택한 공격 실행
                    ChangeState(BossState.Attack);
                }
                break;
            case BossState.Attack:
                if (bossAttack.IsAttackFinished)
                {
                    ChangeState(BossState.Recovery);
                }
                break;
            case BossState.Recovery:
                if (stateTimer >= recoveryDuration)
                {
                    ChangeState(BossState.Idle);
                }
                break;
            case BossState.Dead:
                break;

        }
    }
}