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
    [SerializeField] private Slider BossDelayedHpBar;

    [SerializeField] private float delayedHpSpeed = 100f;
    [SerializeField] private float delayedHpDelay = 0.5f;
    private float delayedHpTimer;
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

        BossDelayedHpBar.maxValue = bossHealth.MaxHealth;

        PlayerHpBar.maxValue = playerHealth.MaxHealth;
        UpdatePlayerHpBar(); UpdateBossHpBar();
    }
    private void Start()
    {
        BossHpBar.value = bossHealth.CurrentHealth;
        BossDelayedHpBar.value = bossHealth.CurrentHealth;
        PlayerHpBar.value = playerHealth.CurrentHealth;
    }
    private void Update()
    {
        if (BossDelayedHpBar.value > BossHpBar.value)
        {
            delayedHpTimer += Time.deltaTime;
            if (delayedHpTimer >= delayedHpDelay)
            {
                BossDelayedHpBar.value = Mathf.MoveTowards(BossDelayedHpBar.value, BossHpBar.value, delayedHpSpeed * Time.deltaTime);
            }
        }
        else
            delayedHpTimer = 0;
    }
    private void OnDestroy()
    {
        if (bossHealth != null)
            bossHealth.OnBossHpChanged -= UpdateBossHpBar;
        if (playerHealth != null)
            playerHealth.OnPlayerHpChanged -= UpdatePlayerHpBar;
    }
}
