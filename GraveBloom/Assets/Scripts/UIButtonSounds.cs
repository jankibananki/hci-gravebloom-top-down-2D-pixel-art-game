using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class UIButtonSounds : MonoBehaviour,
    IPointerEnterHandler,
    IPointerClickHandler
{
    [Header("Audio")]
    public AudioSource audioSource;
    public AudioClip hoverSound;
    public AudioClip clickSound;

    private Button button;

    void Awake()
    {
        button = GetComponent<Button>();
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        if (button != null && !button.interactable)
            return;

        if (audioSource != null && hoverSound != null)
            audioSource.PlayOneShot(hoverSound);
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        if (button != null && !button.interactable)
            return;

        if (audioSource != null && clickSound != null)
            audioSource.PlayOneShot(clickSound);
    }
}