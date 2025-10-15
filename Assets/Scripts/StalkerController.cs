using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StalkerController : MonoBehaviour
{
    [Header("Distance Settings")]
    [SerializeField] private float targetDistance = 10f;
    [SerializeField] private float distanceTolerance = 1f;

    [Header("Movement Settings")]
    [SerializeField] private float moveSpeed = 3f;

    private Transform player;
    private Camera playerCamera;
    private bool isBeingWatched = false;

    // Start is called before the first frame update
    void Start()
    {
        GameObject playerObj = GameObject.FindGameObjectWithTag("Player");
        if (playerObj != null)
        {
            player = playerObj.transform;
            playerCamera = playerObj.GetComponentInChildren<Camera>();
        }
        else
        {
            Debug.LogError("Pursuer: No player assigned and no GameObject with 'Player' tag found!");
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (player == null) return;

        CheckIfBeingWatched();

        if (!isBeingWatched)
        {
            MoveTowardsTargetDistance();
        }
    }

    void CheckIfBeingWatched()
    {
        if (playerCamera == null) return;

        Vector3 viewportPoint = playerCamera.WorldToViewportPoint(transform.position);

        isBeingWatched = viewportPoint.x >= 0 && viewportPoint.x <= 1 &&
                           viewportPoint.y >= 0 && viewportPoint.y <= 1 &&
                           viewportPoint.z > 0;
    }

    void MoveTowardsTargetDistance()
    {
        float currentDistance = Vector3.Distance(transform.position, player.position);

        if (Mathf.Abs(currentDistance - targetDistance) > distanceTolerance)
        {
            Vector3 directionToPlayer = (player.position - transform.position).normalized;

            Vector3 targetPosition;
            if (currentDistance > targetDistance)
            {
                targetPosition = transform.position + directionToPlayer * moveSpeed * Time.deltaTime;
            }
            else
            {
                targetPosition = transform.position - directionToPlayer * moveSpeed * Time.deltaTime;
            }

            transform.position = targetPosition;

            transform.LookAt(new Vector3(player.position.x, transform.position.y, player.position.z));
        }
    }

#if UNITY_EDITOR
    void OnDrawGizmosSelected()
    {
        // For debug
        if (player == null) return;

        // Draw target distance sphere
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(player.position, targetDistance);

        // Draw tolerance range
        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(player.position, targetDistance - distanceTolerance);
        Gizmos.DrawWireSphere(player.position, targetDistance + distanceTolerance);

        // Draw line to player
        Gizmos.color = isBeingWatched ? Color.red : Color.blue;
        Gizmos.DrawLine(transform.position, player.position);
    }
#endif
}
