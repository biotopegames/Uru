using UnityEngine;

public class PressureButton : MonoBehaviour
{
    private Animator buttonAnimator;
    public GameObject[] connectedButtons; // Other pressure buttons to check
    public GameObject activateObject; // Object to activate when all buttons are pressed

    private bool isActivated = false;

    void Start()
    {
        buttonAnimator = GetComponent<Animator>();
    }

    private void OnTriggerStay2D(Collider2D other)
    {
        if (other.CompareTag("Player") || other.CompareTag("Companion"))
        {
            ActivateButton();
        }
    }

        private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player") || other.CompareTag("Companion"))
        {
            isActivated = false;
            buttonAnimator.SetBool("active", false);
        }
    }

    private void ActivateButton()
    {
        if (!isActivated)
        {


            // Mark as activated
            isActivated = true;

            // Play activation animation
            buttonAnimator.SetBool("active", isActivated);

            // Check if all connected buttons are activated
            if (CheckAllButtonsActivated())
            {
                // Activate the connected object
                if (activateObject != null)
                {
                    activateObject.SetActive(true);
                }
            }
        }
    }

    private bool CheckAllButtonsActivated()
    {
        foreach (GameObject button in connectedButtons)
        {
            if (button != null)
            {
                PressureButton pressureButton = button.GetComponent<PressureButton>();
                if (pressureButton != null && !pressureButton.isActivated)
                {
                    return false;
                }
            }
        }
        return true;
    }
}
