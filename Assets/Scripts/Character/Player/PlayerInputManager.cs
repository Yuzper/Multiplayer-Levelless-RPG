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

    [Header("LOCK ON INPUT")]
    [SerializeField] bool lockOnInput;
    [SerializeField] bool lockOnRightInput;
    [SerializeField] bool lockOnLeftInput;
    private Coroutine lockOnCoroutine;

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
    [SerializeField] bool actionNumber1Input = false;
    [SerializeField] bool actionNumber2Input = false;
    [SerializeField] bool actionNumber3Input = false;

    [Header("Mouse Attack Inputs")]
    [SerializeField] bool rightMouseChargeAttackInput = false;
    [SerializeField] bool rightMouseAttackInput = false;

    [SerializeField] bool leftMouseChargeAttackInput = false;
    [SerializeField] bool leftMouseAttackInput = false;

    [Header("UI")]
    [SerializeField] bool escapeMenuInput = false;
    public EscapeMenuManager escapeMenu;

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
        escapeMenu = EscapeMenuManager.instance;
    }

    // GETTER FUNCTIONS
    public bool Get_RightMouseAttackInput()
    {
        return rightMouseAttackInput;
    }

    public bool Get_LeftMouseAttackInput()
    {
        return leftMouseAttackInput;
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

            // Lock on input
            playerControls.PlayerActions.LockOn.performed += i => lockOnInput = true;
            playerControls.PlayerActions.SeekRightLockOnTarget.performed += i => lockOnRightInput = true;
            playerControls.PlayerActions.SeekLeftLockOnTarget.performed += i => lockOnLeftInput = true;

            // Other Actions
            playerControls.PlayerActions.Dance.performed += i => danceInput = true;
            playerControls.PlayerActions.Revival.performed += i => revivalInput = true;
            playerControls.PlayerActions.ActionNumber1.performed += i => actionNumber1Input = true;
            playerControls.PlayerActions.ActionNumber2.performed += i => actionNumber2Input = true;
            playerControls.PlayerActions.ActionNumber3.performed += i => actionNumber3Input = true;

            // Mouse Attack Actions
            playerControls.PlayerActions.RightMouseAttack.performed += i => rightMouseAttackInput = true;
            playerControls.PlayerActions.RightMouseChargeAttack.performed += i => rightMouseChargeAttackInput = true;
            playerControls.PlayerActions.RightMouseChargeAttack.canceled += i => rightMouseChargeAttackInput = false;

            playerControls.PlayerActions.LeftMouseAttack.performed += i => leftMouseAttackInput = true;
            playerControls.PlayerActions.LeftMouseChargeAttack.performed += i => leftMouseChargeAttackInput = true;
            playerControls.PlayerActions.LeftMouseChargeAttack.canceled += i => leftMouseChargeAttackInput = false;

            // UI
            playerControls.UI.EscapeMenu.performed += i => escapeMenuInput = true;
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
        HandleEscapeMenu();
        HandleLockOnInput();
        HandleLockOnSwitchTargetInput();
        HandlePlayerMovementInput();
        HandleCameraMovementInput();
        HandleDodgeInput();
        HandleJumpInput();
        // Actions
        HandleDanceInput();
        HandleRevivalInput();
        HandleActionInputs();
        // Attack Inputs
        HandleMouseAttackInput();
        HandleMouseChargeAttackInput();
    }

    // LOCK ON
    private void HandleLockOnInput()
    {
        // CHECK FOR DEAD TARGET
        if (player.playerNetworkManager.isLockedOn.Value)
        {
            if (player.playerCombatManager.currentTarget.isDead.Value)
            {
                // THIS ASSURES US THAT THE COROUTINE NEVER RUNS MULTIPLE TIMES OVERLAPPING ITSELF
                if (lockOnCoroutine != null)
                {
                    StopCoroutine(lockOnCoroutine);
                }

                // ATTEMPT TO FIND NEW TARGET
                lockOnCoroutine = StartCoroutine(PlayerCamera.instance.WaitThenFindNewTarget());
            }

            if (player.playerCombatManager.currentTarget == null)
            {
                player.playerNetworkManager.isLockedOn.Value = false;
            }
        }


        if (lockOnInput && player.playerNetworkManager.isLockedOn.Value)
        {
            // DISABLE LOCK ON   
            lockOnInput = false;
            player.playerCombatManager.SetTarget(null);
            PlayerCamera.instance.ClearLockOnTargets();
            player.playerNetworkManager.isLockedOn.Value = false; 
            return;
        }

        if (lockOnInput && !player.playerNetworkManager.isLockedOn.Value)
        {
            // ENABLE LOCK ON   
            lockOnInput = false;
            // TODO IF WE ARE AIMING USING RANGED WEAPONS RETURN (DO NOT ALLOW LOCK ON WHILST AIMING)

            PlayerCamera.instance.HandleLocatingLockOnTarget();

            if (PlayerCamera.instance.nearestLockOnTarget != null)
            {
                // SET THE TARGET AS OUR CURRENT TARGET
                player.playerCombatManager.SetTarget(PlayerCamera.instance.nearestLockOnTarget);
                player.playerNetworkManager.isLockedOn.Value = true;
            }
        }
    }

    private void HandleLockOnSwitchTargetInput()
    {
        // KNOWN BUG!!!
        // If you keep switching left and right target the list of available targets grow without limit. It has no functional error but should be fixed.
        if (lockOnLeftInput)
        {
            lockOnLeftInput = false;

            if (player.playerNetworkManager.isLockedOn.Value)
            {
                PlayerCamera.instance.HandleLocatingLockOnTarget();
                
                if (PlayerCamera.instance.leftLockOnTarget != null)
                {
                    player.playerCombatManager.SetTarget(PlayerCamera.instance.leftLockOnTarget);
                }
            }
        }

        if (lockOnRightInput)
        {
            lockOnRightInput = false;

            if (player.playerNetworkManager.isLockedOn.Value)
            {
                PlayerCamera.instance.HandleLocatingLockOnTarget();

                if (PlayerCamera.instance.rightLockOnTarget != null)
                {
                    player.playerCombatManager.SetTarget(PlayerCamera.instance.rightLockOnTarget);
                }
            }
        }
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

        if(moveAmount != 0)
        {
            player.playerNetworkManager.isMoving.Value = true;
        } 
        else
        {
            player.playerNetworkManager.isMoving.Value = false;
        }

        // IF WE ARE NOT LOCKED ON, ONLY USE THE MOVE AMOUNT

        if (!player.playerNetworkManager.isLockedOn.Value || player.playerLocomotionManager.isRolling)
        {
            player.playerAnimatorManager.UpdateAnimatorMovementParameters(0, moveAmount);
        }
        else
        {
            player.playerAnimatorManager.UpdateAnimatorMovementParameters(horizontalInput, verticalInput);
        }


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
        if (actionNumber3Input && player.playerInventoryManager.currentRightHandWeapon != null)
        {
            actionNumber3Input = false;
            player.playerCombatManager.PerformWeaponBasedAction(player.playerInventoryManager.currentRightHandWeapon.JumpAttack, player.playerInventoryManager.currentRightHandWeapon);
        }
    }

    //// UI Button Inputs ////
    private void HandleEscapeMenu()
    {
        // TODO NOT IMPLEMENTED CORRECTLY WITH MULTIPLAYER 
        //return;
        if (escapeMenuInput && WorldSaveGameManager.instance.GetWorldSceneIndex() != 0) // Index 0 is Main Menu
        {
            escapeMenuInput = false;
            EscapeMenuManager.instance.DecideOpenOrCloseEscapeMenu();
        }
    }

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

    public void HandleAction3InputButton()
    {
        actionNumber3Input = true;
        HandleActionInputs();
    }


    //// Attack Inputs ////
    private void HandleMouseAttackInput()
    {
        // RIGHT HAND
        if (rightMouseChargeAttackInput)
        {
            rightMouseAttackInput = false;
            // TODO: IF WE HAVE A UI WINDOW OPEN, RETURN AND DO NOTHING
            player.playerNetworkManager.SetCharacterActionHand(true);
            // TODO: IF WE ARE TWO HANDING THE WEAPON, USE THE TWO HANDED ACTION

            player.playerCombatManager.PerformWeaponBasedAction(player.playerInventoryManager.currentRightHandWeapon.oneHandHeavyRightMouseAttack, player.playerInventoryManager.currentRightHandWeapon);
        }
        else if (rightMouseAttackInput)
        {
            rightMouseAttackInput = false;
            // TODO: IF WE HAVE A UI WINDOW OPEN, RETURN AND DO NOTHING

            player.playerNetworkManager.SetCharacterActionHand(true);
            // TODO: IF WE ARE TWO HANDING THE WEAPON, USE THE TWO HANDED ACTION

            player.playerCombatManager.PerformWeaponBasedAction(player.playerInventoryManager.currentRightHandWeapon.oneHandRightMouseAttack, player.playerInventoryManager.currentRightHandWeapon);
        }
        
        // LEFT HAND
        if (leftMouseChargeAttackInput)
        {
            leftMouseAttackInput = false;
            // TODO: IF WE HAVE A UI WINDOW OPEN, RETURN AND DO NOTHING
            player.playerNetworkManager.SetCharacterActionHand(false);
            // TODO: IF WE ARE TWO HANDING THE WEAPON, USE THE TWO HANDED ACTION

            player.playerCombatManager.PerformWeaponBasedAction(player.playerInventoryManager.currentLeftHandWeapon.oneHandHeavyLeftMouseAttack, player.playerInventoryManager.currentLeftHandWeapon);
        }
        else if (leftMouseAttackInput)
        {
            leftMouseAttackInput = false;
            // TODO: IF WE HAVE A UI WINDOW OPEN, RETURN AND DO NOTHING

            player.playerNetworkManager.SetCharacterActionHand(false);
            // TODO: IF WE ARE TWO HANDING THE WEAPON, USE THE TWO HANDED ACTION

            player.playerCombatManager.PerformWeaponBasedAction(player.playerInventoryManager.currentLeftHandWeapon.oneHandLeftMouseAttack, player.playerInventoryManager.currentLeftHandWeapon);
        }
    }

    private void HandleMouseChargeAttackInput()
    {
        if (player.isPerformingAction)
        {
            if (player.playerNetworkManager.isUsingRightHand.Value)
            {
                player.playerNetworkManager.isChargingRightAttack.Value = rightMouseChargeAttackInput;
            }

            if (player.playerNetworkManager.isUsingLeftHand.Value)
            {
                player.playerNetworkManager.isChargingLeftAttack.Value = leftMouseChargeAttackInput;
            }
        }
    }

}
