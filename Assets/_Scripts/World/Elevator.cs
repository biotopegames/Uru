using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Elevator : MonoBehaviour
{
    public float[] yPositions; // Array of y positions
    public float moveSpeed; // Speed of the elevator
    private int currentPositionIndex = 0; // Current position index
    private bool isMoving = false; // Is the elevator moving?
    [SerializeField] private GameObject elevatorObject; // Reference to the elevator object

    void Start()
    {
    }

    void Update()
    {
        if (isMoving)
        {
            MoveElevator();
        }
    }

    void MoveElevator()
    {
        // Calculate the target position
        Vector3 targetPosition = new Vector3(elevatorObject.transform.position.x, yPositions[currentPositionIndex], elevatorObject.transform.position.z);

        // Move the elevator towards the target position
        elevatorObject.transform.position = Vector3.MoveTowards(elevatorObject.transform.position, targetPosition, moveSpeed * Time.deltaTime);

        // Check if the elevator has reached the target position
        if (elevatorObject.transform.position == targetPosition)
        {
            // The elevator has reached the target position, stop moving
            isMoving = false;
        }
    }

    void OnTriggerEnter2D(Collider2D other)
{
   if (other.gameObject.CompareTag("Player attack") && !isMoving)
   {
       // The player has attacked the lever, start moving the elevator
       isMoving = true;

       // Move to the next position in the array
       currentPositionIndex++;

       // If we are at the end of the array, go back to the start
       if (currentPositionIndex >= yPositions.Length)
       {
           currentPositionIndex = 0;
       }
   }
}

}

