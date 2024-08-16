using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting.InputSystem;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    public float speed = 3f;
    public float runSpeed = 5f;
    public float crouchSpeed = 1.5f;
    public float gravity = -9.81f;
    private Vector3 velocity;

    private Vector3 moveDirection;

    // Input
    private PlayerInput playerInput;
    private InputAction moveAction;
    private InputAction runAction;
    private InputAction interactionAction;
    private InputAction lookAction;
    private InputAction crouchAction;

    private Vector2 inputVector;
    private Vector2 lookInput;

    // camera
    public Transform cameraTransform;       // Camera Transform
    private float cameraRotationX = 0f;     // Current X-axis rotation state of the camera
    public float minCameraAngle = -60f;     // Maximum angle at which the camera can view upwards
    public float maxCameraAngle = 40f;      // Maximum angle at which the camera can look down
    public float lookSensitivity = 0.1f;    // Camera sensitivity
    public float lookSmoothSpeed = 0.1f;    // Adjust for smoothness
    public float yRotationSmoothSpeed = 1f; // Smoothness for Y-axis rotation

    private Vector2 smoothedLookInput;
    private float currentYRotation = 0f;

    // state
    private bool isCrouching = false;
    public bool isInteraction { get; private set; } = false;

    // Crouch settings
    public float crouchHeight = 1f;             // 웅크릴 때 캐릭터의 높이
    public float standingHeight = 2f;           // 서 있을 때 캐릭터의 높이
    public float crouchSpeedChangeRate = 5f;    // 웅크리기와 서기 애니메이션의 전환 속도

    // Interaction
    public float interactionDistance = 3f;
    private Door currentDoor;                   // 현재 상호작용 가능한 문


    // CharacterController Component
    private CharacterController characterController;


    private void Awake()
    {
        // Character Controller component
        characterController = GetComponent<CharacterController>();

        // Fixing and hiding the cursor
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;

        // Connect PlayerInput Component
        playerInput = GetComponent<PlayerInput>();

        // Move Action
        moveAction = playerInput.actions["Move"];
        moveAction.Enable();

        // Run Action
        runAction = playerInput.actions["Run"];
        runAction.Enable();

        // Look Action
        lookAction = playerInput.actions["Look"];
        lookAction.Enable();

        // Interaction Action
        interactionAction = playerInput.actions["Interaction"];
        interactionAction.Enable();

        // Crouch Action
        crouchAction = playerInput.actions["Crouch"];
        crouchAction.Enable();
    }

    void Update()
    {
        // Move Input
        inputVector = moveAction.ReadValue<Vector2>();
        //moveDirection = new Vector3(inputVector.x, 0, inputVector.y);

        // Look Input (mouse)
        lookInput = lookAction.ReadValue<Vector2>() * lookSensitivity;
        smoothedLookInput = Vector2.Lerp(smoothedLookInput, lookInput, lookSmoothSpeed);

        // Crouch Input
        isCrouching = crouchAction.ReadValue<float>() > 0;

        // Adjust character height and camera position based on crouch state
        AdjustHeight();

        // Handle interaction
        HandleInteraction();
    }

    private void FixedUpdate()
    {
        // Move
        OnMove();

        // Apply gravity
        ApplyGravity();
    }

    private void LateUpdate()
    {
        // Look
        OnLook();
    }

    private void OnMove()
    {
        // Check Run Action
        bool isRunning = runAction.ReadValue<float>() > 0;

        // Set Current Speed based on crouch and run state
        float currentSpeed = isRunning ? (isCrouching ? crouchSpeed : runSpeed) : (isCrouching ? crouchSpeed : speed);

        // Calculate move direction relative to player orientation
        moveDirection = transform.right * inputVector.x + transform.forward * inputVector.y;

        // Normalize the move direction vector
        if (moveDirection.magnitude > 0)
        {
            moveDirection = moveDirection.normalized;
        }

        // Move
        //transform.Translate(moveDirection * currentSpeed * Time.deltaTime);
        characterController.Move((moveDirection * currentSpeed + velocity) * Time.deltaTime);
    }

    private void OnLook()
    {
        // Smooth Y axis Player rotation
        currentYRotation += smoothedLookInput.x * yRotationSmoothSpeed;
        transform.localRotation = Quaternion.Euler(0f, currentYRotation, 0f);

        // Camera Rotation (X axis, up - down)
        cameraRotationX -= lookInput.y;
        cameraRotationX = Mathf.Clamp(cameraRotationX, minCameraAngle, maxCameraAngle);

        // Apply camera rotation
        Quaternion targetRotation = Quaternion.Euler(cameraRotationX, 0f, 0f);
        cameraTransform.localRotation = Quaternion.Slerp(cameraTransform.localRotation, targetRotation, Time.deltaTime * 12f); // Adjust the smoothing factor (10f) as needed

    }

    private void ApplyGravity()
    {
        // Check if grounded
        if (characterController.isGrounded && velocity.y < 0)
        {
            velocity.y = -2f; // Keep player grounded
        }

        // Apply gravity to velocity
        velocity.y += gravity * Time.deltaTime;
    }

    private void AdjustHeight()
    {
        // Smoothly adjust height based on crouch state
        float targetHeight = isCrouching ? crouchHeight : standingHeight;
        characterController.height = Mathf.Lerp(characterController.height, targetHeight, crouchSpeedChangeRate * Time.deltaTime);
        // stand 2, crouch 1

        // Adjust the center of the CharacterController to keep it properly aligned
        float heightOffset = (characterController.height / 2) - 0.5f;
        // stand 0.5, crouch 0
        characterController.center = new Vector3(0, heightOffset - 0.5f, 0);
        // stand 0, 0, 0, crouch 0, -0.5, 0

        float cameraOffset = isCrouching ? 0.5f : 1.1f;
        // Adjust the camera position based on character's height
        cameraTransform.localPosition = new Vector3(cameraTransform.localPosition.x, heightOffset + cameraOffset, cameraTransform.localPosition.z);
        // stand y: 1.6, crouch y : 0.5
    }

    private void HandleInteraction()
    {
        // Create a ray from the camera to the forward direction
        Ray ray = new Ray(cameraTransform.position, cameraTransform.forward);
        RaycastHit hit;

        // Draw a debug ray (visible in the Scene view)
        Debug.DrawRay(ray.origin, ray.direction * interactionDistance, Color.red);

        // Check if the ray hits any object within the interaction distance
        if (Physics.Raycast(ray, out hit, interactionDistance))
        {
            Debug.Log("닿음");
            // Check if the hit object has a Door component
            Door door = hit.transform.GetComponent<Door>();
            if (door != null)
            {
                Debug.Log("문 있음");
                currentDoor = door;

                // If the interaction button is pressed, toggle the door state
                if (interactionAction.triggered)
                {
                    Debug.Log("상호작용 키 눌림");
                    door.ToggleDoor();
                }
            }
            else
            {
                currentDoor = null;
            }
        }
        else
        {
            currentDoor = null;
        }
    }

}
