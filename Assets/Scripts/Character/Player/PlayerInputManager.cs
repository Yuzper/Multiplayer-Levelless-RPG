using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerInputManager : MonoBehaviour
{
    public static PlayerInputManager instance;
    public PlayerManager player;
    PlayerControls playerControls;

    [Header("CAMERA MOVEMENT INPUT")]
    [SerializeField] Vector2 cameraInput;
    public float cameraVerticalInput;
    public float cameraHorizontalInput;

    [Header("PLAYER MOVEMENT INPUT")]
    [SerializeField] Vector2 movementInput;
    public float verticalInput;
    public float horizontalInput;
    public float moveAmount;

    [Header("PLAYER ACTION INPUT")]
    [SerializeField] bool dodgeInput = false;
    [SerializeField] bool jumpInput = false;
    [SerializeField] bool danceInput = false;
    [SerializeField] bool revivalInput = false;
    [SerializeField] bool rightMouseAttackInput = false;
    [SerializeField] bool leftMouseAttackInput = false;
    [SerializeField] bool actionNumber1Input = false;
    [SerializeField] bool actionNumber2Input = false;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void Start()
    {
        DontDestroyOnLoad(gameObject);

        // WHEN THE SCENE CHANGES, RUN THIS LOGIC
        SceneManager.activeSceneChanged += SceneManager_activeSceneChanged;

        instance.enabled = false;

        if (playerControls != null)
        {
            playerControls.Disable();
        }
    }

    private void SceneManager_activeSceneChanged(Scene oldScene, Scene newScene)
    {
        // IF WE ARE LOADING INTO OUR WORLD SCENE, ENABLE OUR PLAYER CONTROLS
        //if (newScene.buildIndex == WorldSaveGameManager.instance.GetWorldSceneIndex())
        if (newScene.buildIndex != 0)
        {
            instance.enabled = true;

            if (playerControls != null)
            {
                playerControls.Enable();
            }
        }
        // OTHERWISE WE MUST BE AT THE MAIN MENU, DISABLE OUR PLAYERS CONTROLS
        // THIS IS SO OUR PLAYER CANT MOVE AROUND IF WE ENTER THINGS LIKE A CHARACTER CREATION MENU ETC...
        else
        {
            instance.enabled = false;

            if (playerControls != null)
            {
                playerControls.Disable();
            }
        }
    }

    private void OnEnable()
    {
        if (playerControls == null)
        {
            playerControls = new PlayerControls();

            playerControls.PlayerMovement.Movement.performed += i => movementInput = i.ReadValue<Vector2>();
            playerControls.PlayerCamera.Movement.performed += i => cameraInput = i.ReadValue<Vector2>();
            playerControls.PlayerActions.Dodge.performed += i => dodgeInput = true;
            playerControls.PlayerActions.Jump.performed += i => jumpInput = true;
            // Other Actions begin
            playerControls.PlayerActions.Dance.performed += i => danceInput = true;
            playerControls.PlayerActions.Revival.performed += i => revivalInput = true;
            playerControls.PlayerActions.ActionNumber1.performed += i => actionNumber1Input = true;
            playerControls.PlayerActions.ActionNumber2.performed += i => actionNumber2Input = true;
            // Other Actions end
            playerControls.PlayerActions.RightMouseAttack.performed += i => rightMouseAttackInput = true;
            playerControls.PlayerActions.LeftMouseAttack.performed += i => leftMouseAttackInput = true;
        }

        playerControls.Enable();
    }

    private void OnDestroy()
    {
        // IF WE DESTROY THIS OBJECT, UNSUBSCRIBE FROM THIS EVENT
        SceneManager.activeSceneChanged -= SceneManager_activeSceneChanged;
    }

    private void OnApplicationFocus(bool focus)
    {
        if (enabled)
        {
            if (focus)
            {
                playerControls.Enable();
            }
            else
            {
                playerControls.Disable();
            }
        }
    }

    private void Update()
    {
        HandleAllInputs();
    }


    private void HandleAllInputs()
    {
        HandlePlayerMovementInput();
        HandleCameraMovementInput();
        HandleDodgeInput();
        HandleJumpInput();
        // Actions
        HandleDanceInput();
        HandleRevivalInput();
        HandleActionInputs();
        //
        HandleMouseAttackInput();
        //HandleRightMouseAttackInput();
        //HandleLeftMouseAttackInput();
    }

    // MOVEMENT SECTION
    private void HandlePlayerMovementInput()
    {
        verticalInput = movementInput.y;
        horizontalInput = movementInput.x;

        moveAmount = Mathf.Clamp01(Mathf.Abs(verticalInput) + Mathf.Abs(horizontalInput));

        // WE CLAMP THE VALUES, SO THEY ARE 0, 0.5 or 1 (OPTIONAL)
        if (moveAmount <= 0.5 && moveAmount > 0)
        {
            moveAmount = 0.5f;
        }
        else if (moveAmount > 0.5 && moveAmount <= 1)
        {
            moveAmount = 1;
        }

        if (player == null) return;

        player.playerAnimatorManager.UpdateAnimatorMovementParameters(0, moveAmount);
    }

    private void HandleCameraMovementInput()
    {
        cameraVerticalInput = cameraInput.y;
        cameraHorizontalInput = cameraInput.x;
    }

    // ACTIONS SECTION
    private void HandleDodgeInput()
    {
        if (dodgeInput)
        {
            dodgeInput = false;
            player.playerLocomotionManager.AttemptToPerformDodge();
        }
    }

    private void HandleJumpInput()
    {
        if (jumpInput)
        {
            jumpInput = false;

            // IF WE HAVE A UI WINDOW OPEN, SIMPLY RETURN WITHOUT DOING ANYTHING

            // ATTEMP TO PERFORM JUMP
            player.playerLocomotionManager.AttemptToPerformJump();

        }
    }

    private void HandleDanceInput()
    {
        if (danceInput)
        {
            danceInput = false;
            player.playerLocomotionManager.AttemptToPerformDance();
        }
    }

    private void HandleRevivalInput()
    {
        if (revivalInput)
        {
            revivalInput = false;
            player.playerLocomotionManager.AttemptToRevive();

        }
    }

    private void HandleActionInputs()
    {
        if (actionNumber1Input)
        {
            actionNumber1Input = false;
            player.playerEquipmentManager.SwitchLeftWeapon();
        }

        if (actionNumber2Input)
        {
            actionNumber2Input = false;
            player.playerEquipmentManager.SwitchRightWeapon();
        }
    }

    //// Button Inputs ////
    public void HandleDanceInputButton()
    {
        danceInput = true;
        HandleDanceInput();
    }

    public void HandleReviveInputButton()
    {
        revivalInput = true;
        HandleRevivalInput();
    }

    public void HandleAction1InputButton()
    {
        actionNumber1Input = true;
        HandleActionInputs();
    }

    public void HandleAction2InputButton()
    {
        actionNumber2Input = true;
        HandleActionInputs();
    }


    //// Attack Inputs ////
    private void HandleMouseAttackInput()
    {
        // RIGHT HAND
        if (rightMouseAttackInput)
        {
            rightMouseAttackInput = false;
            // TODO: IF WE HAVE A UI WINDOW OPEN, RETURN AND DO NOTHING

            player.playerNetworkManager.SetCharacterActionHand(true);
            // TODO: IF WE ARE TWO HANDING THE WEAPON, USE THE TWO HANDED ACTION

            player.playerCombatManager.PerformWeaponBasedAction(player.playerInventoryManager.currentRightHandWeapon.oneHandRightMouseAttack, player.playerInventoryManager.currentRightHandWeapon);
        }

        // LEFT HAND
        if (leftMouseAttackInput)
        {
            leftMouseAttackInput = false;
            // TODO: IF WE HAVE A UI WINDOW OPEN, RETURN AND DO NOTHING

            player.playerNetworkManager.SetCharacterActionHand(false);
            // TODO: IF WE ARE TWO HANDING THE WEAPON, USE THE TWO HANDED ACTION

            player.playerCombatManager.PerformWeaponBasedAction(player.playerInventoryManager.currentLeftHandWeapon.oneHandLeftMouseAttack, player.playerInventoryManager.currentLeftHandWeapon);
        }
    }


    private void HandleRightMouseAttackInput()
    {
        if (leftMouseAttackInput == true)
        {
            leftMouseAttackInput = false;
        }

        if (rightMouseAttackInput)
        {
            rightMouseAttackInput = false;
            // TODO: IF WE HAVE A UI WINDOW OPEN, RETURN AND DO NOTHING

            player.playerNetworkManager.SetCharacterActionHand(false);
            // TODO: IF WE ARE TWO HANDING THE WEAPON, USE THE TWO HANDED ACTION

            player.playerCombatManager.PerformWeaponBasedAction(player.playerInventoryManager.currentRightHandWeapon.oneHandRightMouseAttack, player.playerInventoryManager.currentRightHandWeapon);
        }
    }

    private void HandleLeftMouseAttackInput()
    {
        if (rightMouseAttackInput == true)
        {
            rightMouseAttackInput = false;
        }

        if (leftMouseAttackInput)
        {
            leftMouseAttackInput = false;
            // TODO: IF WE HAVE A UI WINDOW OPEN, RETURN AND DO NOTHING

            player.playerNetworkManager.SetCharacterActionHand(true);
            // TODO: IF WE ARE TWO HANDING THE WEAPON, USE THE TWO HANDED ACTION

            player.playerCombatManager.PerformWeaponBasedAction(player.playerInventoryManager.currentLeftHandWeapon.oneHandLeftMouseAttack, player.playerInventoryManager.currentLeftHandWeapon);
        }
    }
}
