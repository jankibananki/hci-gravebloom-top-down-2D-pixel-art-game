using UnityEngine;
using UnityEngine.UI;

public class ExitIndicator : MonoBehaviour
{
    public float screenMargin = 80f;

    private Transform target;
    private Camera mainCamera;
    private RectTransform rectTransform;
    private Image image;

    void Awake()
    {
        mainCamera = Camera.main;

        rectTransform =
            GetComponent<RectTransform>();

        image =
            GetComponent<Image>();

        image.enabled = false;
    }

    public void SetTarget(Transform newTarget)
    {
        target = newTarget;

        if (target != null)
            image.enabled = true;
    }

    void Update()
    {
        if (target == null)
        {
            image.enabled = false;
            return;
        }

        Vector3 screenPosition =
            mainCamera.WorldToScreenPoint(
                target.position
            );

        bool targetVisible =
            screenPosition.z > 0 &&
            screenPosition.x > 0 &&
            screenPosition.x < Screen.width &&
            screenPosition.y > 0 &&
            screenPosition.y < Screen.height;

        // ako je point vec na ekranu
        // ne treba nam UI strelica
        if (targetVisible)
        {
            image.enabled = false;
            return;
        }

        image.enabled = true;

        Vector2 screenCenter =
            new Vector2(
                Screen.width / 2f,
                Screen.height / 2f
            );

        Vector2 direction =
            (Vector2)screenPosition -
            screenCenter;

        //ako je target iza kamere
        if (screenPosition.z < 0)
            direction *= -1f;

        direction.Normalize();

        // rotacija strelice
        float angle =
            Mathf.Atan2(
                direction.y,
                direction.x
            ) * Mathf.Rad2Deg;

        // +/−90 zavisi kako ti je nacrtan PNG
        // ovaj kod pretpostavlja da sprite pokazuje GORE.
        rectTransform.rotation =
            Quaternion.Euler(
                0f,
                0f,
                angle - 90f
            );

        // Pozicija na ivici ekrana
        Vector2 position =
            screenCenter +
            direction * 1000f;

        position.x =
            Mathf.Clamp(
                position.x,
                screenMargin,
                Screen.width - screenMargin
            );

        position.y =
            Mathf.Clamp(
                position.y,
                screenMargin,
                Screen.height - screenMargin
            );

        rectTransform.position =
            position;
    }
}
