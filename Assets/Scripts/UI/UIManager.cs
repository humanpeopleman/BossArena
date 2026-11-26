using UnityEngine;
using TMPro;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
public class UIManager : MonoBehaviour
{
    [SerializeField] private BossHealth bossHealth;
    [SerializeField] private PlayerHealth playerHealth;

    [SerializeField] private GameObject gameOverPanel;
    [SerializeField] private TMP_Text gameOverText;
    [SerializeField] private Slider BossHpBar;
    [SerializeField] private Slider PlayerHpBar;

    public void ShowGameOver(string text)
    {
        gameOverText.SetText(text);
        gameOverPanel.SetActive(true);
    }
    public void RestartGame()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene("SampleScene");
    }
    private void UpdateBossHpBar()
    {
        BossHpBar.value = bossHealth.CurrentHealth;
    }
    private void UpdatePlayerHpBar()
    {
        PlayerHpBar.value = playerHealth.CurrentHealth;
    }
    private void Awake()
    {
        if (bossHealth != null)
            bossHealth.OnBossHpChanged += UpdateBossHpBar;
        if (playerHealth != null)
            playerHealth.OnPlayerHpChanged += UpdatePlayerHpBar;
        BossHpBar.maxValue = bossHealth.MaxHealth;
        PlayerHpBar.maxValue = playerHealth.MaxHealth;
        UpdatePlayerHpBar(); UpdateBossHpBar();
    }
    private void OnDestroy()
    {
        if (bossHealth != null)
            bossHealth.OnBossHpChanged -= UpdateBossHpBar;
        if (playerHealth != null)
            playerHealth.OnPlayerHpChanged -= UpdatePlayerHpBar;
    }
}
