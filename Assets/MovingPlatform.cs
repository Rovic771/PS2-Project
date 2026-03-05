using UnityEngine;

public class MovingPlatform : MonoBehaviour
{
    public float moveSpeed = 2f;
    public float moveDistance = 3f;

    private bool isMoving = false;
    private bool hasMoved = false;
    private Vector3 targetPosition;

    public void MoveTowardPlayer()
    {
        if (isMoving || hasMoved) return;

        GameObject player = GameObject.FindGameObjectWithTag("Player");

        if (player != null)
        {
            Vector3 direction = (player.transform.position - transform.position).normalized;
            targetPosition = transform.position + direction * moveDistance;

            isMoving = true;
            hasMoved = true;
        }
    }

    void Update()
    {
        if (isMoving)
        {
            transform.position = Vector3.MoveTowards(
                transform.position,
                targetPosition,
                moveSpeed * Time.deltaTime
            );

            if (Vector3.Distance(transform.position, targetPosition) < 0.05f)
            {
                isMoving = false;
            }
        }
    }
}