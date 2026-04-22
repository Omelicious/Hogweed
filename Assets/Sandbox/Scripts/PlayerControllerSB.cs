using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerControllerSB : MonoBehaviour
{
    private InputAction moveAction;
    private InputAction interactAction;
    private InputAction sprintAction;
    private InputAction attackAction;
    private InputAction viewAction;
    private InputAction lookAction;

    [SerializeField] private Camera camera;
    private CharacterController characterController;
    private PlayerInput playerInput;
    private Painter painter;
    private Vector3 moveAmount;
    private Vector3 lookAmount;
    private Vector3 moveDirection;
    private Vector3 lookRotation;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        characterController = GetComponent<CharacterController>();
        playerInput = GetComponent<PlayerInput>();
        painter = GetComponent<Painter>();

        moveAction = InputSystem.actions.FindAction("Move");
        interactAction = InputSystem.actions.FindAction("Interact");
        sprintAction = InputSystem.actions.FindAction("Sprint");
        attackAction = InputSystem.actions.FindAction("Attack");
        viewAction = InputSystem.actions.FindAction("View");
        lookAction = InputSystem.actions.FindAction("Look");

        playerInput.onActionTriggered += OnLook;
        playerInput.onActionTriggered += OnMove;
        playerInput.onActionTriggered += OnAttack;

        lookRotation = camera.transform.rotation.eulerAngles;
    }

    // Update is called once per frame
    void Update()
    {
        if (!attackAction.IsPressed())
            return;
        
        painter?.PaintTerrain();
        painter?.PaintWallCPU();
    }

    /// This function is called every fixed framerate frame, if the MonoBehaviour is enabled.
    void FixedUpdate()
    {
        characterController.SimpleMove(10f * moveDirection);
    }

    /// This function is called when the object becomes enabled and active.
    void OnEnable()
    {
        moveAction?.Enable();
        interactAction?.Enable();
        sprintAction?.Enable();
        attackAction?.Enable();
        viewAction?.Enable();
        lookAction?.Enable();
    }

    /// This function is called when the behaviour becomes disabled or inactive.
    void OnDisable()
    {
        moveAction?.Disable();
        interactAction?.Disable();
        sprintAction?.Disable();
        attackAction?.Disable();
        viewAction?.Disable();
        lookAction?.Disable();
    }

    void OnMove(InputAction.CallbackContext context)
    {
        if (context.action != moveAction)
            return;
        Vector2 moveInput = context.action.ReadValue<Vector2>();
        moveAmount.x = -moveInput.x; // somehow w is backwards
        moveAmount.z =  moveInput.y;

        ChangeDirection();
    }

    void OnLook(InputAction.CallbackContext context)
    {
        if (context.action != lookAction)
            return;

        gameObject.transform.Rotate(0f, camera.transform.rotation.eulerAngles.y - lookRotation.y, 0f);

        lookRotation = camera.transform.rotation.eulerAngles;

        ChangeDirection();
    }

    void OnAttack(InputAction.CallbackContext context)
    {
        if (context.action != attackAction)
            return;

    }

    void ChangeDirection()
    {
        moveDirection = transform.forward * moveAmount.x + transform.right * moveAmount.z;
    }
}
