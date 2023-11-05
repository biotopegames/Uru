using UnityEngine;

public class Lever : MonoBehaviour
{
    public GameObject objectToActivate;  // The GameObject to activate when the lever is triggered.

    [SerializeField]private bool isActivated = false;   // Flag to track whether the lever has been activated.
    private AudioSource audio;


    void Start()
    {
        audio = GetComponent<AudioSource>();
    }
    // This method is called when a GameObject enters the trigger collider of the lever.
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player") && !isActivated)
        {
            // Check if the colliding GameObject has the "Player" tag and the lever hasn't been activated yet.
            ActivateObject();
        }
    }

    // Method to activate the specified GameObject.
    private void ActivateObject()
    {
        if (objectToActivate != null)
        {
            audio.Play();
            objectToActivate.SetActive(true);  // Activate the specified GameObject.
            isActivated = true;               // Set the lever as activated to prevent further activation.
        }
    }
}
