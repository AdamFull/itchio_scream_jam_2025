using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Experimental.GlobalIllumination;

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

    [Header("Flashlight")]
    public Light flashlightSource;
    public AudioClip flashlightPhaseOnBegin;
    public AudioClip flashlightPhaseOnEnd;
    public AudioClip flashlightPhaseOffBegin;
    public AudioClip flashlightPhaseOffEnd;

    [Header("Audio")]
    public AudioSource audioSource;

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

        if(audioSource == null)
        {
            audioSource = GetComponent<AudioSource>();
        }

        // Show cursor since we're not using mouse look
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
    }

    // Update is called once per frame
    void Update()
    {
        HandleFlashlight();
        HandleRotation();
        HandleMovement();
    }

    void HandleFlashlight()
    {
        bool wasEnabled = flashlightSource.enabled;
        bool flashlightKeyDown = Input.GetKeyDown(KeyCode.F);
        bool flashlightKeyUp = Input.GetKeyUp(KeyCode.F);

        // Toggle flashlight
        if (flashlightKeyUp)
        {
            flashlightSource.enabled = !flashlightSource.enabled;
        }

        //
        if(flashlightKeyDown)
        {
            audioSource.clip = wasEnabled ? flashlightPhaseOffBegin : flashlightPhaseOnBegin;
        }
        if(flashlightKeyUp)
        {
            audioSource.clip = wasEnabled ? flashlightPhaseOffEnd : flashlightPhaseOnEnd;
        }

        if (flashlightKeyDown || flashlightKeyUp)
        {
            audioSource.Play();
        }
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
        cameraTransform.Rotate(Vector3.up * rotationInput * rotationSpeed * Time.deltaTime);
    }

    void HandleMovement()
    {
        // Get forward/backward input only (W/S or Arrow keys)
        // TODO: Make key bindngs in project settings or use input mapper package
        float moveZ = Input.GetAxis("Vertical");

        // NOTE: We can't move backwards
        if (moveZ < 0f)
        {
            return;
        }

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
