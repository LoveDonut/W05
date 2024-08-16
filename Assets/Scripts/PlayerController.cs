using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting.InputSystem;
using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(Status))]
public class PlayerController : MonoBehaviour
{
    public float speed = 5f;
    public float runSpeed = 10f;
    private Vector3 moveDirection;


    // Input
    private PlayerInput playerInput;
    private InputAction moveAction;
    private InputAction runAction;
    private InputAction interactionAction;
    private InputAction lookAction;

    // camera
    public Transform cameraTransform;       // Camera Transform
    private float cameraRotationX = 0f;     // Current X-axis rotation state of the camera
    public float minCameraAngle = -60f;     // Maximum angle at which the camera can view upwards
    public float maxCameraAngle = 40f;      // Maximum angle at which the camera can look down
    public float lookSensitivity = 0.1f;      // Camera sensitivity
    public float lookSmoothSpeed = 0.1f;    // Adjust for smoothness
    public float yRotationSmoothSpeed = 1f; // Smoothness for Y-axis rotation

    private Vector2 smoothedLookInput;
    private float currentYRotation = 0f;

    private Vector2 inputVector;
    private Vector2 lookInput;

    // Status
    private Status status;
    public bool isRunning = true;

    private void Awake()
    {
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

        // Interaction Action
        interactionAction = playerInput.actions["Interaction"];
        interactionAction.Enable();

        // Look Action
        lookAction = playerInput.actions["Look"];
        lookAction.Enable();

        // Connect Status Component
        status = GetComponent<Status>();
    }

    void Update()
    {
        // Move Input
        inputVector = moveAction.ReadValue<Vector2>();
        moveDirection = new Vector3(inputVector.x, 0, inputVector.y);

        // Look Input (mouse)
        lookInput = lookAction.ReadValue<Vector2>() * lookSensitivity;
        smoothedLookInput = Vector2.Lerp(smoothedLookInput, lookInput, lookSmoothSpeed);

    }

    private void FixedUpdate()
    {
        // Move
        OnMove();
    }

    private void LateUpdate()
    {
        // Look
        OnLook();
    }

    private void OnMove()
    {
        // Check Run Action
        isRunning = runAction.ReadValue<float>() > 0;

        // Set Current Speed
        float currentSpeed = isRunning && status.CanRunning ? runSpeed : speed;

        // Normalize the move direction vector
        if (moveDirection.magnitude > 0)
        {
            moveDirection = moveDirection.normalized;
        }

        // Move
        transform.Translate(moveDirection * currentSpeed * Time.deltaTime);
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

}
