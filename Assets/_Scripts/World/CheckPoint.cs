using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CheckPoint : MonoBehaviour
{

    [SerializeField] private GameObject objectToActivate;
    private bool isActivated = false;
    // Start is called before the first frame update


    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player") && !isActivated)
        {
            isActivated = true;
            if (objectToActivate != null)
            objectToActivate.SetActive(true);
        }

    }

}
