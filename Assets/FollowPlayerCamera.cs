using System.Collections;
using System.Collections.Generic;
using Cinemachine;
using UnityEngine;

public class FollowPlayerCamera : MonoBehaviour
{
    [SerializeField] private string playerTag = "Player"; // The tag to identify the player.
    [SerializeField] private Transform player; // Reference to the player's transform.
    [SerializeField] private CinemachineVirtualCamera virtualCamera; // Reference to the Cinemachine Virtual Camera.

    private void Start()
    {

        virtualCamera = GetComponent<CinemachineVirtualCamera>();

        // If the player reference is not assigned and the player tag exists, find the player by tag.
        if (player == null && !string.IsNullOrEmpty(playerTag))
        {
            GameObject playerObject = GameObject.FindWithTag("Player");
            if (playerObject != null)
            {
                player = playerObject.transform;
            }
        }

        // Set the Virtual Camera to follow the player.
        if (player != null && virtualCamera != null)
        {
            virtualCamera.Follow = player;
        }
    }
}
