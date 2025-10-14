using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BillboardBaseScript : MonoBehaviour
{
    [Header("Discrete Step Settings")]
    [Tooltip("Number of discrete rotation steps (e.g., 8 means 8 directions: 45� apart)")]
    [Range(4, 16)]
    public int numberOfSteps = 8;

    [Tooltip("Lock rotation on specific axes")]
    public bool lockX = true;
    public bool lockY = false;
    public bool lockZ = true;

    private Transform targetTransform;
    private float angleStep;

    // Start is called before the first frame update
    void Start()
    {
        targetTransform = GameManager.instance.characterInstance.transform;
        angleStep = 360f / (float)numberOfSteps;
    }
    
    void LateUpdate()
    {
        if (targetTransform == null) return;

        Vector3 directionToCamera = targetTransform.position - transform.position;
        float angleToCamera = Mathf.Atan2(directionToCamera.x, directionToCamera.z) * Mathf.Rad2Deg;
        float snappedAngle = Mathf.Round(angleToCamera / angleStep) * angleStep;
        
        Quaternion targetRotation = Quaternion.Euler(
            lockX ? transform.rotation.eulerAngles.x : 0f,
            lockY ? transform.rotation.eulerAngles.y : snappedAngle,
            lockZ ? transform.rotation.eulerAngles.z : 0f
        );
        
        transform.rotation = targetRotation;
    }

    // Visualize the discrete directions in the editor
#if UNITY_EDITOR
    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.yellow;
        float currentAngleStep = 360f / numberOfSteps;

        for (int i = 0; i < numberOfSteps; i++)
        {
            float angle = i * currentAngleStep * Mathf.Deg2Rad;
            Vector3 direction = new Vector3(Mathf.Sin(angle), 0, Mathf.Cos(angle));
            Gizmos.DrawLine(transform.position, transform.position + direction * 2f);
        }
    }
#endif
}
