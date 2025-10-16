using System.Collections;
using System.Collections.Generic;
using TMPro;
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

    [Header("Footstep Sounds")]
    public List<AudioClip> footstepSounds = new List<AudioClip>();
    [Tooltip("Distance player needs to travel before playing next footstep")]
    public float stepDistance = 2f;
    public bool reshuffleFoodsteps = false;

    private CharacterController controller;
    private Vector3 moveDirection = Vector3.zero;
    private float verticalVelocity = 0f;
    private float distanceTraveled = 0f;
    private Vector3 lastPosition;

    // Needed additional shuffling, because without it footsteps sounds shitty
    private List<AudioClip> shuffledFootsteps = new List<AudioClip>();
    private int currentFootstepIndex = 0;

    private bool blockAllInput = false;
    private bool isRotatingToTarget = false;
    private Quaternion targetRotation;
    private Quaternion initialRotation;

    // Start is called before the first frame update
    void Start()
    {
        controller = GetComponent<CharacterController>();

        if (cameraTransform == null)
        {
            cameraTransform = GetComponentInChildren<Camera>().transform;
        }

        if (audioSource == null)
        {
            audioSource = GetComponent<AudioSource>();
        }

        //if (footstepAudioSource == null)
        //{
        //    GameObject footstepObj = new GameObject("FootstepAudioSource");
        //    footstepObj.transform.SetParent(transform);
        //    footstepObj.transform.localPosition = Vector3.zero;
        //    footstepAudioSource = footstepObj.AddComponent<AudioSource>();
        //    footstepAudioSource.spatialBlend = 0f;
        //    footstepAudioSource.playOnAwake = false;
        //}

        ShuffleFootsteps();

        lastPosition = transform.position;

        // Show cursor since we're not using mouse look
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
    }

    // Update is called once per frame
    void Update()
    {
        HandleLockOnTarget();

        if (blockAllInput)
        {
            return;
        }

        HandleFlashlight();
        HandleRotation();
        HandleMovement();
        HandleFootsteps();
    }

    public void SetBlockAllInput(bool value)
    {
        blockAllInput = value;
    }

    public void LockOnTarget(Transform target)
    {
        if (!target) return;

        Vector3 direction = target.position - cameraTransform.position;
        if (direction != Vector3.zero)
        {
            initialRotation = cameraTransform.rotation;
            targetRotation = Quaternion.LookRotation(direction);
            isRotatingToTarget = true;
        }
    }

    public void UnlockTarget()
    {
        isRotatingToTarget = true;
        targetRotation = initialRotation;
    }

    void HandleLockOnTarget()
    {
        if (!isRotatingToTarget) return;

        cameraTransform.rotation = Quaternion.Slerp(
            cameraTransform.rotation,
            targetRotation,
            2f * Time.deltaTime
        );

        float angle = Quaternion.Angle(cameraTransform.rotation, targetRotation);
        if (angle < 0.1f)
        {
            cameraTransform.rotation = targetRotation;
            isRotatingToTarget = false;
        }
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

        // Play sound effect depends on current flashlight state
        if (flashlightKeyDown)
        {
            audioSource.PlayOneShot(wasEnabled ? flashlightPhaseOffBegin : flashlightPhaseOnBegin);
        }
        if (flashlightKeyUp)
        {
            audioSource.PlayOneShot(wasEnabled ? flashlightPhaseOffEnd : flashlightPhaseOnEnd);
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
            isRotatingToTarget = false;
        }
        else if (Input.GetKey(KeyCode.D))
        {
            rotationInput = 1f;
            isRotatingToTarget = false;
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

    void HandleFootsteps()
    {
        // Only play footsteps if grounded and moving
        if (!controller.isGrounded || footstepSounds.Count == 0)
        {
            return;
        }

        Vector3 currentPosition = transform.position;
        Vector3 horizontalMovement = currentPosition - lastPosition;
        horizontalMovement.y = 0f;

        float frameDist = horizontalMovement.magnitude;

        if (frameDist > 0.001f)
        {
            distanceTraveled += frameDist;

            if (distanceTraveled >= stepDistance)
            {
                PlayRandomFootstep();
                distanceTraveled = 0f;
            }
        }

        lastPosition = currentPosition;
    }

    void ShuffleFootsteps()
    {
        if (footstepSounds.Count == 0)
            return;

        AudioClip lastPlayed = null;
        if (shuffledFootsteps.Count > 0 && currentFootstepIndex > 0)
        {
            lastPlayed = shuffledFootsteps[currentFootstepIndex - 1];
        }

        shuffledFootsteps = ShuffleUtility.SpotifyShuffle(footstepSounds, lastPlayed);

        // Reset index to start of new shuffle
        currentFootstepIndex = 0;
    }

    void PlayRandomFootstep()
    {
        if (footstepSounds.Count == 0)
            return;

        if (reshuffleFoodsteps && currentFootstepIndex >= shuffledFootsteps.Count)
        {
            ShuffleFootsteps();
        }

        AudioClip footstepClip = shuffledFootsteps[currentFootstepIndex];

        if (footstepClip != null)
        {
            float randomVolume = Random.Range(0.1f, 0.3f);
            audioSource.PlayOneShot(footstepClip, randomVolume);
        }

        currentFootstepIndex = (currentFootstepIndex + 1) % shuffledFootsteps.Count;
    }
}
