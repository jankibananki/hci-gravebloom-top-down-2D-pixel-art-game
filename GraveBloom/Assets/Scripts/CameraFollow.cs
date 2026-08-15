using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    public Transform target;
    public BoxCollider2D cameraBounds;
    public float smoothSpeed = 5f;

    private Camera cam;

    void Awake()
    {
        cam = GetComponent<Camera>();
    }

    void LateUpdate()
    {
        if (target == null || cameraBounds == null)
            return;

        Vector3 targetPosition = new Vector3(
            target.position.x,
            target.position.y,
            transform.position.z
        );

        // Koliko kamera vidi od centra do ivice ekrana
        float halfHeight = cam.orthographicSize;
        float halfWidth = halfHeight * cam.aspect;

        Bounds bounds = cameraBounds.bounds;

        float minX = bounds.min.x + halfWidth;
        float maxX = bounds.max.x - halfWidth;

        float minY = bounds.min.y + halfHeight;
        float maxY = bounds.max.y - halfHeight;

        // Sprečava problem ako je mapa manja od kamere
        float clampedX = minX > maxX
            ? bounds.center.x
            : Mathf.Clamp(targetPosition.x, minX, maxX);

        float clampedY = minY > maxY
            ? bounds.center.y
            : Mathf.Clamp(targetPosition.y, minY, maxY);

        Vector3 finalPosition = new Vector3(
            clampedX,
            clampedY,
            transform.position.z
        );

        transform.position = Vector3.Lerp(
            transform.position,
            finalPosition,
            smoothSpeed * Time.deltaTime
        );
    }
}