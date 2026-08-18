using UnityEngine;

public class WorldMarkerBob : MonoBehaviour
{
    public float bobHeight = 0.2f;
    public float bobSpeed = 3f;

    private Vector3 startPosition;

    void Start()
    {
        startPosition = transform.localPosition;
    }

    void Update()
    {
        float offset =
            Mathf.Sin(Time.time * bobSpeed) *
            bobHeight;

        transform.localPosition =
            startPosition +
            Vector3.up * offset;
    }
}