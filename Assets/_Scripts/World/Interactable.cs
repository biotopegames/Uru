using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Interactable : MonoBehaviour
{

    [Header("References")]
    [SerializeField] private Animator iconAnimator; //The E icon animator
    [SerializeField] private Animator objectAnimator; //The E icon animator
    [SerializeField] private Instantiator instantiator; //The E icon animator



    [Header("Trigger")]
    [SerializeField] private bool autoHit; //Does the player need to press the interact button, or will it simply fire automatically?
    [SerializeField] private bool repeat; //Set to true if the player should be able to talk again and again to the NPC. 
    // Start is called before the first frame update
    private bool hasBeenActivated;

void OnTriggerEnter2D(Collider2D col)
{
    if (col.gameObject.tag == "Player" && !autoHit && !hasBeenActivated)
    {
            iconAnimator.SetBool("active", true);
    }
}

void OnTriggerStay2D(Collider2D col)
{
        if (col.gameObject.tag == "Player" && !autoHit && !hasBeenActivated)
    {
            if(Input.GetKey(KeyCode.W) || Input.GetKey(KeyCode.UpArrow))
            {
                objectAnimator.SetTrigger("activate");
                iconAnimator.SetBool("active", false);
                instantiator.InstantiateObjects();
                hasBeenActivated = true;
            }
    }
}

void OnTriggerExit2D(Collider2D col)
{
    if (col.gameObject.tag == "Player")
    {
            iconAnimator.SetBool("active", false);
    }
}

}
