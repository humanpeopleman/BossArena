using System;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    [SerializeField] private BossHealth bossHealth;
    [SerializeField] private PlayerHealth playerHealth;
    [SerializeField] private UIManager UIManager;
    private GameState currentState = GameState.Playing;
    private void Awake()
    {
        if (bossHealth != null)
            bossHealth.OnBossDead += HandleBossDead;
        if (playerHealth != null)
            playerHealth.OnPlayerDead += HandlePlayerDead;
    }
    private void EndGame(GameState result)
    {
        if (currentState != GameState.Playing) return;
        currentState = result;
        Debug.Log("엔드게임");
        Time.timeScale = 0.0f;
        UIManager.ShowGameOver(result.ToString());
    }
    private void HandlePlayerDead()
    {
        EndGame(GameState.GameOver);
    }
    private void HandleBossDead()
    {
        EndGame(GameState.Victory);
    }
    private void OnDestroy()
    {
        if (bossHealth != null)
            bossHealth.OnBossDead -= HandleBossDead;
        if (playerHealth != null)
            playerHealth.OnPlayerDead -= HandlePlayerDead;
    }
}
