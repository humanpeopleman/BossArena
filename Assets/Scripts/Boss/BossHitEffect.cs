using UnityEngine;

public class BossHitEffect : MonoBehaviour
{
    private SpriteRenderer spriteRenderer;
    private Color originalColor;
    private Color hitColor = Color.red;
    private bool isHitColor = false;
    private float blinkDuration = 0.3f;
    private float blinkTimer = 0;
    private void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        originalColor = spriteRenderer.color;
    }
    private void Update()
    {
        if (isHitColor)
        {
            blinkTimer += Time.deltaTime;
            if (blinkTimer >= blinkDuration)
            {
                isHitColor = false;
                blinkTimer = 0;
                spriteRenderer.color = originalColor;
            }
        }
    }
    public void PlayHitBlink()
    {
        spriteRenderer.color = hitColor;
        blinkTimer = 0;
        isHitColor = true;
    }
}
