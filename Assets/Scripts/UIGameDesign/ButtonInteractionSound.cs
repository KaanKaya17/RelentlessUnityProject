using UnityEngine;
using UnityEngine.EventSystems;

public class ButtonInteractionSound : MonoBehaviour, IPointerEnterHandler, IPointerClickHandler
{
    [Header("Volume Settings")]
    public AudioClip hoverSound; 
    public AudioClip clickSound;

    [Range(0f, 1f)] public float volume = 0.5f;

    private AudioSource audioSource;

    void Awake()
    {       
        audioSource = gameObject.AddComponent<AudioSource>();
        audioSource.playOnAwake = false;
        audioSource.volume = volume;
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        if (hoverSound != null)
        {
            audioSource.PlayOneShot(hoverSound);
        }
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        if (clickSound != null)
        {
            audioSource.PlayOneShot(clickSound);
        }
    }
}