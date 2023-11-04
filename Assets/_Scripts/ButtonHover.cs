using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class ButtonHover : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    private AudioSource audioSource;
    public AudioClip hoverSound;

    private Button button;

    private void Start()
    {
        button = GetComponent<Button>();
        audioSource = GetComponent<AudioSource>(); // Replace with the actual name of your AudioSource GameObject.
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        if (button.interactable) // Check if the button is interactable (optional)
        {
            audioSource.PlayOneShot(hoverSound);
        }
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        // Additional actions when the mouse pointer exits the button area
    }
}
