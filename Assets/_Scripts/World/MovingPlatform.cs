using UnityEngine;

public class MovingPlatform : MonoBehaviour
{
    public Vector3 startPosition;   // The starting position of the platform.
    public Vector3 endPosition;     // The ending position of the platform.
    public float speed = 2.0f;      // The speed at which the platform moves.

    private Vector3 currentTarget;  // The current target position.

    void Start()
    {
        startPosition = transform.position;
        // Initialize the platform's current target to the starting position.
        currentTarget = startPosition;
    }

    void Update()
    {
        // Move the platform towards the current target position.
        transform.position = Vector3.MoveTowards(transform.position, currentTarget, speed * Time.deltaTime);

        // Check if the platform has reached the current target.
        if (Vector3.Distance(transform.position, currentTarget) < 0.01f)
        {
            // Switch the current target to the other position.
            if (currentTarget == startPosition)
            {
                currentTarget = endPosition;
            }
            else
            {
                currentTarget = startPosition;
            }
        }
    }

    // This method is called when Gizmos are drawn in the Unity Editor.
    private void OnDrawGizmos()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawSphere(startPosition, 0.2f); // Draws a sphere at the start position.
        Gizmos.DrawSphere(endPosition, 0.2f);   // Draws a sphere at the end position.

        // Draw a line between the start and end points.
        Gizmos.DrawLine(startPosition, endPosition);
    }
}
