using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    // Movement speed
    public float moveSpeed = 5f;
    // Mouse sensitivity for looking around
    public float mouseSensitivity = 2f;
    // Jump settings
    public float jumpHeight = 2f;
    public float groundCheckDistance = 0.2f;
    // Reference to the Character Controller
    private CharacterController controller;
    // Reference to the Camera
    private Camera playerCamera;
    // For looking up and down
    private float verticalRotation = 0f;
    // Gravity
    private float gravity = -9.81f;
    private Vector3 velocity;

    // --- Footstep audio settings ---
    [Header("Footsteps")]
    public AudioClip[] footstepClips;          // assign several footstep clips in Inspector
    public AudioSource footstepSource;         // optional: assign an AudioSource; will create one if null
    [Tooltip("Base interval between steps (seconds). Lower = faster footsteps.")]
    public float baseStepInterval = 0.5f;
    [Tooltip("Minimum movement magnitude to trigger footsteps.")]
    public float stepMovementThreshold = 0.1f;
    private float footstepTimer = 0f;

    void Start()
    {
        // Get the Character Controller component
        controller = GetComponent<CharacterController>();
        // Get the camera (it's a child of the player)
        playerCamera = GetComponentInChildren<Camera>();

        // Ensure we have an AudioSource for footsteps
        if (footstepSource == null)
        {
            footstepSource = gameObject.AddComponent<AudioSource>();
            footstepSource.playOnAwake = false;
            footstepSource.spatialBlend = 1f; // 3D sound
            footstepSource.rolloffMode = AudioRolloffMode.Logarithmic;
            footstepSource.minDistance = 0.1f;
            footstepSource.maxDistance = 15f;
        }

        // Lock and hide the cursor
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    void Update()
    {
        // Handle movement
        Vector3 moveVector = MovePlayer();
        // Handle mouse look
        LookAround();
        // Handle jumping
        HandleJump();
        // Apply gravity
        ApplyGravity();
        // Handle footsteps
        HandleFootsteps(moveVector);

        // Unlock cursor with Escape key
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
        }
    }

    // Returns the horizontal movement vector used for footsteps calculation
    Vector3 MovePlayer()
    {
        // Get input from keyboard (WASD or Arrow keys)
        float horizontal = Input.GetAxis("Horizontal"); // A/D or Left/Right
        float vertical = Input.GetAxis("Vertical");     // W/S or Up/Down
        // Calculate movement direction relative to where player is facing
        Vector3 move = transform.right * horizontal + transform.forward * vertical;
        // Move the player
        controller.Move(move * moveSpeed * Time.deltaTime);
        // return movement ignoring vertical component for footsteps
        Vector3 horizontalMove = new Vector3(move.x, 0f, move.z);
        return horizontalMove;
    }

    void LookAround()
    {
        // Get mouse movement
        float mouseX = Input.GetAxis("Mouse X") * mouseSensitivity;
        float mouseY = Input.GetAxis("Mouse Y") * mouseSensitivity;
        // Rotate player left and right
        transform.Rotate(Vector3.up * mouseX);
        // Look up and down (only the camera)
        verticalRotation -= mouseY;
        verticalRotation = Mathf.Clamp(verticalRotation, -90f, 90f); // Limit looking up/down
        playerCamera.transform.localRotation = Quaternion.Euler(verticalRotation, 0f, 0f);
    }

    void HandleJump()
    {
        // Check if player is on the ground and presses Space
        if (controller.isGrounded && Input.GetButtonDown("Jump"))
        {
            // Calculate jump velocity using physics formula: v = sqrt(2 * jumpHeight * gravity)
            velocity.y = Mathf.Sqrt(jumpHeight * -2f * gravity);
            Debug.Log("Player jumped!"); // Optional: see when jump happens in Console
        }
    }

    void ApplyGravity()
    {
        // Check if player is on the ground
        if (controller.isGrounded && velocity.y < 0)
        {
            velocity.y = -2f; // Small downward force to keep grounded
        }
        // Apply gravity
        velocity.y += gravity * Time.deltaTime;
        // Move player down
        controller.Move(velocity * Time.deltaTime);
    }

    void HandleFootsteps(Vector3 horizontalMove)
    {
        // Check movement and grounded
        if (controller.isGrounded && horizontalMove.magnitude > 0.1f)
        {
            // Play only if not already playing
            if (!footstepSource.isPlaying)
            {
                PlayFootstep();
            }
        }
        else
        {
            // Stop immediately when player stops
            footstepSource.Stop();
        }
    }

    void PlayFootstep()
    {
        if (footstepClips == null || footstepClips.Length == 0) return;

        int index = Random.Range(0, footstepClips.Length);
        footstepSource.clip = footstepClips[index];

        footstepSource.pitch = Random.Range(0.95f, 1.05f);

        footstepSource.loop = true;  // looping footsteps
        footstepSource.Play();
    }

}
