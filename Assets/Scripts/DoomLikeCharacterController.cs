using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(CharacterController))]
public class DoomLikeCharacterController : MonoBehaviour
{
    [Header("Movement")]
    public float movementSpeed = 7.5f;
    public float gravity = 20f;

    [Header("Camera")]
    public float rotationSpeed = 15f;

    // TODO: Add head bob (Like in doom)

    [Header("References")]
    public Transform cameraTransform;

    private CharacterController controller;
    private Vector3 moveDirection = Vector3.zero;
    private float verticalVelocity = 0f;

    // Start is called before the first frame update
    void Start()
    {
        controller = GetComponent<CharacterController>();

        if (cameraTransform == null)
        {
            cameraTransform = GetComponentInChildren<Camera>().transform;
        }

        // Show cursor since we're not using mouse look
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
    }

    // Update is called once per frame
    void Update()
    {
        HandleRotation();
        HandleMovement();
    }

    void HandleRotation()
    {
        // Rotate player left/right with A and D keys
        // TODO: Make key bindngs in project settings or use input mapper package
        float rotationInput = 0f;

        if (Input.GetKey(KeyCode.A))
        {
            rotationInput = -1f;
        }
        else if (Input.GetKey(KeyCode.D))
        {
            rotationInput = 1f;
        }

        // Apply rotation
        transform.Rotate(Vector3.up * rotationInput * rotationSpeed * Time.deltaTime);
    }

    void HandleMovement()
    {
        // Get forward/backward input only (W/S or Arrow keys)
        // TODO: Make key bindngs in project settings or use input mapper package
        float moveZ = Input.GetAxis("Vertical");

        // Calculate movement direction (only forward/backward relative to player rotation)
        Vector3 forward = transform.TransformDirection(Vector3.forward);
        moveDirection = forward * moveZ * movementSpeed;

        // Handle gravity
        if (controller.isGrounded)
        {
            verticalVelocity = 0f;
        }
        else
        {
            verticalVelocity -= gravity * Time.deltaTime;
        }

        // Apply vertical velocity
        moveDirection.y = verticalVelocity;

        // Move the controller
        controller.Move(moveDirection * Time.deltaTime);
    }
}
