using UnityEngine;

public class Lever : MonoBehaviour
{
    public GameObject objectToActivate;  // The GameObject to activate when the lever is triggered.

    private bool isActivated = false;   // Flag to track whether the lever has been activated.

    // This method is called when a GameObject enters the trigger collider of the lever.
    private void OnTriggerEnter(Collider other)
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
            objectToActivate.SetActive(true);  // Activate the specified GameObject.
            isActivated = true;               // Set the lever as activated to prevent further activation.
        }
    }
}
