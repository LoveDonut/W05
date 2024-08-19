using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting.InputSystem;
using UnityEngine;
using UnityEngine.InputSystem;
using static System.Collections.Specialized.BitVector32;

[RequireComponent(typeof(Status))]
public class PlayerController : MonoBehaviour
{
    public float speed = 3f;
    public float runSpeed = 6f;
    public float crouchSpeed = 1.5f;
    public float climbSpeed = 2f;
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
    private InputAction selectItemAction;
    private InputAction useAction;
    private List<InputAction> itemSelectActions = new List<InputAction>();
    private InputAction climbAction;

    private Vector2 inputVector;
    private Vector2 lookInput;

    // camera
    public Transform cameraTransform;       // Camera Transform
    private float cameraRotationX = 0f;     // Current X-axis rotation state of the camera
    public float minCameraAngle = -60f;     // Maximum angle at which the camera can view upwards
    public float maxCameraAngle = 40f;      // Maximum angle at which the camera can look down
    public float lookSensitivity = 0.03f;    // Camera sensitivity
    public float lookSmoothSpeed = 0.3f;    // Adjust for smoothness
    public float yRotationSmoothSpeed = 1f; // Smoothness for Y-axis rotation

    private Vector2 smoothedLookInput;
    private float currentYRotation = 0f;

    // state
    private bool isCrouching = false;
    public bool isInteraction { get; private set; } = false;
    private bool isClimbing = false;        // is on ladder?

    // Crouch settings
    public float crouchHeight = 1f;             // ��ũ�� �� ĳ������ ����
    public float standingHeight = 2f;           // �� ���� �� ĳ������ ����
    public float crouchSpeedChangeRate = 5f;    // ��ũ����� ���� �ִϸ��̼��� ��ȯ �ӵ�

    // Interaction
    public float interactionDistance = 3f;
    private Door currentDoor;                   // ���� ��ȣ�ۿ� ������ ��

    // Create a ray from the camera to the forward direction
    public Ray ray {  get; private set; }

    // CharacterController Component
    private CharacterController characterController;

    // Status
    private Status status;
    public bool isRunning = true;

    // Inventory
    Inventory inventory;

    // Ladder
    private Transform ladderTransform;      // current ladder transform

    // Audio
    private AudioSource moveSource;
    private AudioSource breathSource;
    [SerializeField] private AudioClip walkClip;
    [SerializeField] private AudioClip runClip;
    [SerializeField] private AudioClip breathClip;
    
    // Camera Shake
    [SerializeField] float shakeDuration = 1f;
    [SerializeField] float shakeMagnitude = 0.1f;
    [SerializeField] float dampingSpeed = 1.0f;
    float currentShakeDuration;
    Vector3 adjustedCameraPos;

    private void Awake()
    {
        // Character Controller component
        characterController = GetComponent<CharacterController>();
        // Connect PlayerInput Component
        playerInput = GetComponent<PlayerInput>();
        // Connect Status Component
        status = GetComponent<Status>();
        // Connect Inventory component
        inventory = GetComponent<Inventory>();
        // AudioSource component
        AudioSource[] audioSources = GetComponents<AudioSource>();
        moveSource = audioSources[0];
        breathSource = audioSources[1];

        // Fixing and hiding the cursor
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;

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

        // SelectItem Action ����
        //selectItemAction = playerInput.actions["SelectItem"];
        //selectItemAction.Enable();

        // Use Action
        useAction = playerInput.actions["Use"];
        useAction.Enable();

        // Crouch Action
        crouchAction = playerInput.actions["Crouch"];
        crouchAction.Enable();

        // Climb Action (Ladder)
        climbAction = playerInput.actions["Climb"];
        climbAction.Enable();

        // Active CharacterController's Overlap Recovery (CharacterController can restore the collision)
        GetComponent<CharacterController>().enableOverlapRecovery = true;
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

        if (Input.GetKeyDown(KeyCode.CapsLock))
        {
            StartCameraShake();
        }

        // Shake Camera
        ShakeCamera();


        if (isClimbing)
        {
            HandleLadderMovement();
        }
        else
        {
            // Handle interaction
            HandleInteraction();

            // Select Item
            OnSelectItem();
        }

        HandleMovementSound();

        HandleBreathSound();
    }

    private void ShakeCamera()
    {
        if (currentShakeDuration > 0)
        {
            cameraTransform.localPosition = adjustedCameraPos + Random.insideUnitSphere * shakeMagnitude;
            currentShakeDuration -= Time.deltaTime * dampingSpeed;
        }
        else
        {
            currentShakeDuration = 0f;
            cameraTransform.localPosition = adjustedCameraPos;
        }
    }

    public void StartCameraShake()
    {
        currentShakeDuration = shakeDuration;
    }

    private void FixedUpdate()
    {
        // Move
        OnMove();

        if (!isClimbing)
        {
            // Apply gravity
            ApplyGravity();
        }
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

        // Set Current Speed based on crouch and run state
        float currentSpeed = (isRunning && status.CanRunning) ? (isCrouching ? crouchSpeed : runSpeed) : (isCrouching ? crouchSpeed : speed);

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

    private void HandleLadderMovement()
    {
        if (isClimbing)
        {
            // Receive vertical direction input (climbAction is the input action set above)
            float verticalInput = climbAction.ReadValue<float>();

            // Move Y axis
            Vector3 climbMovement = new Vector3(0, verticalInput * climbSpeed, 0);

            characterController.Move(climbMovement * Time.deltaTime);

            // No Gravity
            velocity.y = 0f;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Ladder"))
        {
            isClimbing = true;

            // No Gravity
            velocity.y = 0f;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Ladder"))
        {
            isClimbing = false;
        }
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

    private void OnSelectItem()
    {
        for (int i = 1; i <= 9; i++)
        {
            if (playerInput.actions["SelectItem" + i].triggered)
            {
                Debug.Log(i + " slot");
                inventory.SelectItem(i - 1); // Index starts from 0
                return;
            }
        }

        if (playerInput.actions["SelectItem0"].triggered)
        {
            Debug.Log("0 slot");
            inventory.SelectItem(9); // 0 corresponds to the 9th index
        }
    }

    private void OnUse(InputValue value)
    {
        if(value.isPressed)
        {
            if(inventory.SelectedItem != null)
            {
                Debug.Log($"selected item : {inventory.SelectedItem.GetItemType()}");
                inventory.UseItem();
            }
        }
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
        adjustedCameraPos = new Vector3(0f, heightOffset + cameraOffset, 0f);
        cameraTransform.localPosition = adjustedCameraPos;
        // stand y: 1.6, crouch y : 0.5
    }

    private void HandleInteraction()
    {
        ray = new Ray(cameraTransform.position, cameraTransform.forward);
        RaycastHit hit;

        // Draw a debug ray (visible in the Scene view)
        Debug.DrawRay(ray.origin, ray.direction * interactionDistance, Color.red);
        // Check if the ray hits any object within the interaction distance
        if (Physics.Raycast(ray, out hit, interactionDistance))
        {
            // Check if the hit object has a Door component
            Door door = hit.transform.GetComponent<Door>();
            if (door != null)
            {
                currentDoor = door;

                // If the interaction button is pressed, toggle the door state
                if (interactionAction.triggered)
                {
                    door.ToggleDoor();
                }
            }
            else
            {
                currentDoor = null;
            }

            // Check if the hit object has an Item component
            Item item = hit.transform.GetComponent<Item>();
            if (item != null)
            {

                // If the interaction button is pressed, add the item to the inventory
                if (interactionAction.triggered)
                {
                    // Clone the item before adding it to the inventory
                    GameObject itemClone = Instantiate(item.gameObject);

                    // Disable the original item so it's no longer visible in the world
                    item.gameObject.SetActive(false);
                    itemClone.SetActive(false);

                    // Add the cloned item to the inventory
                    inventory.AddItem(itemClone);
                }
            }

            LightEvent[] lightEvents = hit.transform.GetComponents<LightEvent>();
            if (lightEvents != null)
            {
                if(interactionAction.triggered)
                {
                    foreach (var lightEvent in lightEvents)
                    {
                        if (lightEvent != null)
                        {
                            lightEvent.TriggerLightEvent();
                        }
                    }
                }
            }
        }
        else
        {
            currentDoor = null;
        }
    }

    private void HandleMovementSound()
    {
        if (moveDirection.magnitude > 0)
        {
            if (isRunning && status.CanRunning)
            {
                if (moveSource.clip != runClip || !moveSource.isPlaying)
                {
                    moveSource.clip = runClip;
                    moveSource.volume = 0.1f;
                    moveSource.Play();
                }
            }
            else
            {
                if (moveSource.clip != walkClip || !moveSource.isPlaying)
                {
                    moveSource.clip = walkClip;
                    moveSource.volume = 1f;
                    moveSource.Play();
                }
            }
        }
        else if (moveSource.isPlaying)
        {
            moveSource.Stop();
        }
    }

    private void HandleBreathSound()
    {
        if (!status.CanRunning) // CanRunning이 false일 때
        {
            if (breathSource.clip != breathClip || !breathSource.isPlaying)
            {
                //breathSource.Stop();
                // 숨소리 클립 재생
                breathSource.clip = breathClip;
                breathSource.volume = 0.3f;  // 소리의 크기를 50%로 줄임
                breathSource.pitch = 1f;     // 기본 배속
                breathSource.Play();
            }
        }
    }
}
