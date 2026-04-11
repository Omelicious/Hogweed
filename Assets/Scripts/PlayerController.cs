using System;
using System.Data;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UI;
using UnityEngine.Animations.Rigging;
using UnityEngine.Playables;
using System.ComponentModel;
using System.Threading.Tasks;

using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    #region Fields

    [SerializeField] private float movementSpeedBase = 5.0f;
    [SerializeField] private float runSpeedMultiplyer = 2.5f;
    private float sensitivity;
    private float ySensitivity;
    private float xSensitivity;
    private int weaponSelection = 0; // 0 - hands, 1 - scythe, 2 - trimmer
    private int viewSelection = 0; // 0 - back, 1 - top
    private bool verticalViewEnabled = true;

    #endregion

    #region Objects For Interaction

    private GameObject scythe;
    private GameObject trimmer;
    private Weapon weapon;

    #endregion

    #region Player Components

    [SerializeField] private GameObject raycaster;

    private Rigidbody playerRigidBody;
    private Camera playerCamera;

    private Animator animator;
    private AudioSource audioSource;

    private TwoBoneIKConstraint rightHandIKScythe;
    private TwoBoneIKConstraint leftHandIKScythe;
    private TwoBoneIKConstraint rightHandIKTrimmer;
    private TwoBoneIKConstraint leftHandIKTrimmer;

    #endregion
    
    #region Input Actions

    private InputAction moveAction;
    private InputAction interactAction;
    private InputAction sprintAction;
    private InputAction attackAction;
    private InputAction viewAction;
    private InputAction lookAction;

    #endregion

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        FindActionBindings();
        UpdateSensititvity();

        playerRigidBody = GetComponent<Rigidbody>(); // Taking Rigidbody
        playerCamera = GetComponentInChildren<Camera>(); // Taking child camera

        scythe = GameObject.Find("Scythe"); // Tags will be better, but it'll do it for now<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<
        trimmer = GameObject.Find("Trimmer");
        

        leftHandIKScythe = transform.Find("Character/Rig/Hand_L_IK_Scythe").GetComponent<TwoBoneIKConstraint>(); // Get constraints for character arms
        rightHandIKScythe = transform.Find("Character/Rig/Hand_R_IK_Scythe").GetComponent<TwoBoneIKConstraint>();
        leftHandIKTrimmer = transform.Find("Character/Rig/Hand_L_IK_Trimmer").GetComponent<TwoBoneIKConstraint>();
        rightHandIKTrimmer = transform.Find("Character/Rig/Hand_R_IK_Trimmer").GetComponent<TwoBoneIKConstraint>(); // transform searches through children, not the whole scene!!!
        
        animator = GameObject.Find("Character").GetComponent<Animator>();
        audioSource = GetComponent<AudioSource>(); // it is on the Player, not the character

        scythe.SetActive(false); // Hide them
        trimmer.SetActive(false); // Sort of object pooling

        SwitchHandIK();
    }

    // Update is called once per frame
    void Update()
    {
        PlayerAction();

        
        if (Time.timeScale > 0f)
            PlayerLook();
    }

    void FixedUpdate()
    {
        if (Time.timeScale > 0f)
            PlayerMovement();
    }

    void PlayerAction()
    {
        if (attackAction.IsPressed())
        {
            switch (weaponSelection)
            {
                case 1:
                    animator.SetBool("Scythe", true);
                    break;

                case 2:
                    animator.SetBool("Trimmer", true);
                    break;
            }

            weapon?.Attack(); // the ? checks for null
            weapon?.animator.SetBool("isAttacking", true);
        }
        else if (attackAction.WasReleasedThisFrame())
        {
            weapon?.CancelAttack();
        }
        else
        {
            animator.SetBool("Scythe", false);
            animator.SetBool("Trimmer", false);
            weapon?.animator.SetBool("isAttacking", false);
        }

        // Switch weapons
        if (interactAction.WasPressedThisFrame())
        {
            weaponSelection += 1;
            weaponSelection %= 3;

            switch (weaponSelection)
            {
                case 0: // Hands
                    scythe.SetActive(false);
                    trimmer.SetActive(false);
                    weapon = null;
                    break;

                case 1: // Scythe
                    scythe.SetActive(true);
                    trimmer.SetActive(false);
                    weapon = scythe.GetComponent<Weapon>();
                    weapon.UpdateAnimator(); // Hiding objects nullifies Animators parameters, we need to set them again
                    break;

                case 2: // Trimmer
                    scythe.SetActive(false);
                    trimmer.SetActive(true);
                    weapon = trimmer.GetComponent<Weapon>();
                    weapon.UpdateAnimator();
                    break;
            }

            SwitchHandIK();
        }

        if (viewAction.WasPressedThisFrame())
        {
            viewSelection += 1;
            viewSelection %=3;

            switch (viewSelection)
            {
                case 0:
                    verticalViewEnabled = true;
                    playerCamera.transform.position = transform.Find("CamPosBack").transform.position;
                    playerCamera.transform.eulerAngles = transform.Find("CamPosBack").transform.eulerAngles;
                    break;
                case 1:
                    verticalViewEnabled = false;
                    playerCamera.transform.position = transform.Find("CamPosTop").transform.position;
                    playerCamera.transform.eulerAngles = transform.Find("CamPosTop").transform.eulerAngles;
                    break;
                case 2:
                    verticalViewEnabled = true;
                    playerCamera.transform.position = transform.Find("CamPosFirst").transform.position;
                    playerCamera.transform.eulerAngles = transform.Find("CamPosFirst").transform.eulerAngles;
                    break;
            }
        }
    }

    void PlayerLook()
    {
        //playerRigidBody.constraints = RigidbodyConstraints.FreezeRotationX | RigidbodyConstraints.FreezeRotationY | RigidbodyConstraints.FreezeRotationZ; // needed to be done because of a bug after climbing slopes
        Vector2 lookValue = lookAction.ReadValue<Vector2>();
        float inputMouseX = lookValue.x; // Vertical
        float inputMouseY = -lookValue.y; // Horizontal

        if (inputMouseX == 0f && inputMouseY == 0f)
        {
            return;
        }

        Vector3 cameraRotation = playerCamera.transform.eulerAngles; // Taking global rotation in Vector3 (Euler angles)
        Vector3 playerRotation = transform.eulerAngles;              // because quaternions are too complex for me

        if (verticalViewEnabled)
        {
            cameraRotation.x += inputMouseY * ySensitivity * sensitivity * Time.fixedDeltaTime;
        }
        playerRotation.y += inputMouseX * xSensitivity * sensitivity * Time.fixedDeltaTime;

        if (cameraRotation.x > 180f) // Unity represents negative angles as >180 angles. 
        {                            // So working with Euler angles we have to represent >180 angles as negative.
            cameraRotation.x -= 360f;// Otherwise it takes -10 in Transform (what we can see in the Inspector),
        }                            // represents as 350 and Clamp cuts it off to 80 which makes camera look down

        cameraRotation.x = Mathf.Clamp(cameraRotation.x, -90f, 80f); // Restricting camera angle on X axis to 80 down and 90 up

        playerCamera.transform.eulerAngles = cameraRotation; // May be i should clamp camera angles, so player can not look down?<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<
        playerRigidBody.MoveRotation(Quaternion.Euler(playerRotation)); // Rigidbody rotates Player object, we're in clear

        //playerRigidBody.constraints = RigidbodyConstraints.FreezeRotationX | RigidbodyConstraints.FreezeRotationZ; // Removing y rotation constraint
    }

    void PlayerMovement()
    {
        Vector2 moveValue = moveAction.ReadValue<Vector2>();
        float inputHorizontal = moveValue.x;
        float inputVertical = moveValue.y;
        float movementSpeed = movementSpeedBase; // For not to change movementSpeedBase. Makes sprint code easier

        if (inputHorizontal == 0f && inputVertical == 0f) // no movement
        {
            animator.SetBool("isWalking", false);
            animator.SetBool("isRunning", false);
            return;
        }

        if (sprintAction.IsPressed())
        {
            animator.SetBool("isRunning", true);
            animator.SetBool("isWalking", false);
            movementSpeed *= runSpeedMultiplyer;
        }
        else
        {
            animator.SetBool("isRunning", false);
            animator.SetBool("isWalking", true);
        }

        // Adds (+) two local vectors into one
        Vector3 movementVector = transform.forward * inputVertical + transform.right * inputHorizontal; // First creating movement direction

        movementVector = Vector3.ClampMagnitude(movementVector, 1f); // If magnitude (length) > 1, reduces to 1

        movementVector *= movementSpeed * PointsSystem.Instance.movementSpeedValue * Time.fixedDeltaTime; // Then turning it into final vector

        playerRigidBody.MovePosition(playerRigidBody.position + movementVector);
        
        FixGroundCollision();

        if (!audioSource.isPlaying)
            audioSource.Play();
    }

    void SwitchHandIK()
    {
        switch (weaponSelection)
        {
            case 0: // Hands
                leftHandIKScythe.weight = 0;
                rightHandIKScythe.weight = 0;
                leftHandIKTrimmer.weight = 0;
                rightHandIKTrimmer.weight = 0;
                break;

            case 1: // Scythe
                leftHandIKScythe.weight = 1;
                rightHandIKScythe.weight = 1;
                leftHandIKTrimmer.weight = 0;
                rightHandIKTrimmer.weight = 0;
                break;

            case 2: // Trimmer
                leftHandIKScythe.weight = 0;
                rightHandIKScythe.weight = 0;
                leftHandIKTrimmer.weight = 1;
                rightHandIKTrimmer.weight = 1;
                break;
        }
    }

    void FixGroundCollision () // Raycasting ground. If player is sinking, moving him up to the surface
{
    RaycastHit hit;
        Ray ray = new Ray(raycaster.transform.position, Vector3.down);

        if (Physics.Raycast(ray, out hit, 3.9f, 1 << LayerMask.NameToLayer("Ground"))) // check for ground
        {
            //Debug.Log($"Hit point: {hit.point.y}, distance: {hit.distance}, origin: {ray.origin.y} \n Teleported to the surface");
            
            Vector3 newPosition = transform.position;
            newPosition.y = hit.point.y; // teleporting ourselves to the hitpoint (surface)
            playerRigidBody.MovePosition(newPosition);
        }
}

    void FindActionBindings()
    {
        moveAction = InputSystem.actions.FindAction("Move");
        moveAction?.Enable();
        interactAction = InputSystem.actions.FindAction("Interact");
        interactAction?.Enable();
        sprintAction = InputSystem.actions.FindAction("Sprint");
        sprintAction?.Enable();
        attackAction = InputSystem.actions.FindAction("Attack");
        attackAction?.Enable();
        viewAction = InputSystem.actions.FindAction("View");
        viewAction?.Enable();
        lookAction = InputSystem.actions.FindAction("Look");
        lookAction?.Enable();
    }

    public void UpdateSensititvity()
    {
        sensitivity = SettingsManager.Instance.sensitivity;
        xSensitivity = SettingsManager.Instance.xSensitivity;
        ySensitivity = SettingsManager.Instance.ySensitivity;
    }
}