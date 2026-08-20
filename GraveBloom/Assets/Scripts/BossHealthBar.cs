using UnityEngine;

public class BossHealthBar : MonoBehaviour
{
    public EnemyHealth bossHealth;
    public RectTransform fill;

    private float fullWidth;

    void Start()
    {
        if (fill != null)
            fullWidth = fill.rect.width;
    }

    void Update()
    {
        if (bossHealth == null || fill == null)
            return;

        float healthPercent =
            (float)bossHealth.CurrentHealth /
            bossHealth.MaxHealth;

        healthPercent =
            Mathf.Clamp01(healthPercent);

        fill.SetSizeWithCurrentAnchors(
            RectTransform.Axis.Horizontal,
            fullWidth * healthPercent
        );
    }
}