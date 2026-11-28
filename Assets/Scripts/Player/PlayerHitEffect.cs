using UnityEngine;

public class PlayerHitEffect : MonoBehaviour
{
    private PlayerHealth playerHealth;
    private SpriteRenderer spriteRenderer;
    private Color originalColor;
    private Color hitColor = new Color32(20, 148, 250, 150);
    private bool isHitColor = false;
    private float blinkInterval = 0.1f;
    private float blinkTimer;   
    private void Awake()
    {
        playerHealth = GetComponent<PlayerHealth>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        originalColor = spriteRenderer.color;
    }
    private void Update()
    {
        if (playerHealth.IsInvincible)
        {
            blinkTimer += Time.deltaTime;
            if (blinkTimer >= blinkInterval)
            {
                spriteRenderer.color = isHitColor ? originalColor : hitColor;
                isHitColor = !isHitColor;
                blinkTimer = 0;
            }
        }
        else if (isHitColor)
        {
            spriteRenderer.color = originalColor;
            isHitColor = false;
        }
    }
}
