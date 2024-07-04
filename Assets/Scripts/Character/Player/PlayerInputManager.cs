using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.TextCore.Text;

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
    [SerializeField] bool sprint_Input = false;

    [SerializeField] bool danceInput = false;
    [SerializeField] bool revivalInput = false;
    
    [SerializeField] bool actionNumber1Input = false;
    [SerializeField] bool actionNumber2Input = false;
    [SerializeField] bool actionNumber3Input = false;
    [SerializeField] bool actionNumber4Input = false;
    [SerializeField] bool actionNumber5Input = false;
    [SerializeField] bool actionNumber6Input = false;
    [SerializeField] bool actionNumber7Input = false;
    [SerializeField] bool actionNumber8Input = false;
    [SerializeField] bool actionNumber9Input = false;

    [Header("Mouse Attack Inputs")]
    [SerializeField] bool mainHandAttackInput = false;
    [SerializeField] bool mainHandHeavyAttackInput = false;
    [SerializeField] bool mainHandChargeAttackInput = false;

    [Header("QUED INPUTS")]
    [SerializeField] private bool input_Que_Is_Active = false;
    [SerializeField] float default_Que_Input_Time = 0.35f;
    [SerializeField] float que_Input_Timer = 0;
    [SerializeField] bool que_RB_Input = false;
    [SerializeField] bool que_RT_Input = false;

    [Header("UI")]
    [SerializeField] bool escapeMenuInput = false;
    public EscapeMenuManager escapeMenu;

    [Header("Draw Spell Canvas")]
    public SpellDrawingManager spellDrawingCanvas;
    public UILineRenderer UI_LineRenderer;

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
        spellDrawingCanvas = SpellDrawingManager.instance;
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

            //  HOLDING THE INPUT, SETS THE BOOL TO TRUE
            playerControls.PlayerActions.Sprint.performed += i => sprint_Input = true;
            //  RELEASING THE INPUT, SETS THE BOOL TO FALSE
            playerControls.PlayerActions.Sprint.canceled += i => sprint_Input = false;

            // Other Actions
            playerControls.PlayerActions.Dance.performed += i => danceInput = true;
            playerControls.PlayerActions.Revival.performed += i => revivalInput = true;
            playerControls.PlayerActions.ActionNumber1.performed += i => actionNumber1Input = true;
            playerControls.PlayerActions.ActionNumber2.performed += i => actionNumber2Input = true;
            playerControls.PlayerActions.ActionNumber3.performed += i => actionNumber3Input = true;
            playerControls.PlayerActions.ActionNumber4.performed += i => actionNumber4Input = true;
            playerControls.PlayerActions.ActionNumber5.performed += i => actionNumber5Input = true;
            playerControls.PlayerActions.ActionNumber6.performed += i => actionNumber6Input = true;
            playerControls.PlayerActions.ActionNumber7.performed += i => actionNumber7Input = true;
            playerControls.PlayerActions.ActionNumber8.performed += i => actionNumber8Input = true;
            playerControls.PlayerActions.ActionNumber9.performed += i => actionNumber9Input = true;

            // Mouse Attack Actions
            playerControls.PlayerActions.MainHandAttack.performed += i => mainHandAttackInput = true;
            playerControls.PlayerActions.MainHandHeavyAttack.performed += i => mainHandHeavyAttackInput = true;
            playerControls.PlayerActions.MainHandChargeAttack.performed += i => mainHandChargeAttackInput = true;
            playerControls.PlayerActions.MainHandChargeAttack.canceled += i => mainHandChargeAttackInput = false;

            // QUED Inputs
            playerControls.PlayerActions.QueMainHandAttack.performed += i => QueInput(ref que_RB_Input);
            playerControls.PlayerActions.QueMainHandHeavyAttack.performed += i => QueInput(ref que_RT_Input);

            // UI
            playerControls.UI.EscapeMenu.performed += i => escapeMenuInput = true;

            // Spell casting
            playerControls.PlayerSpellcasting.SpellMode.performed += i => player.characterSpellManager.inSpellMode = !player.characterSpellManager.inSpellMode;
            //playerControls.PlayerSpellcasting.SpellMode.canceled += i => inSpellMode = false;
            playerControls.PlayerSpellcasting.UseSpell.performed += i => player.characterSpellManager.castSpell = true;
            playerControls.PlayerSpellcasting.UseSpellHold.started += i => player.characterSpellManager.castSpellHold = true;
            playerControls.PlayerSpellcasting.UseSpellHold.canceled += i => player.characterSpellManager.castSpellHold = false;
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
        HandleSprintInput();
        HandleJumpInput();
        // Actions
        HandleDanceInput();
        HandleRevivalInput();
        HandleActionInputs();

        if(player.characterSpellManager.inSpellMode)
        {
            spellDrawingCanvas.OpenSpellDrawingMenu(); // Opens the spell drawing menu
            mainHandChargeAttackInput = false;
            mainHandHeavyAttackInput = false;
            mainHandAttackInput = false;
            HandleSpellAttackInput();
        } 
        else
        {
            spellDrawingCanvas.CloseSpellDrawingMenu(); // Closes the spell drawing menu
            // Might be too expensive to have in update method //
            UI_LineRenderer.ResetDrawing(); // Makes sure the drawing is reset whenever you reopen the spell drawing menu.
            // Attack Inputs
            HandleMouseAttackInput();
            HandleMouseHeavyAttackInput();
            HandleMouseChargeAttackInput();
            HandleQuedInputs();
        }

    }

    private void HandleSpellAttackInput()
    {
        if (player.characterSpellManager.inSpellMode)
        {
            //player.animator.SetBool("isHoldingDownSpell", player.characterSpellManager.castSpellHold); // TODO Make network variable? like charge attack
            player.playerNetworkManager.isHoldingDownSpell.Value = player.characterSpellManager.castSpellHold;

            if (player.characterSpellManager.castSpell)
            {
                player.characterSpellManager.castSpell = false;
                player.playerSpellManager.equippedSpell.UseSpell(player);
            }
        }
        else
        {
            player.characterSpellManager.castSpell = false;
            //player.animator.SetBool("isHoldingDownSpell", false);
            player.playerNetworkManager.isHoldingDownSpell.Value = false;
        }
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

        if (!player.playerNetworkManager.isLockedOn.Value || player.playerLocomotionManager.isRolling || player.playerNetworkManager.isSprinting.Value)
        {
            player.playerAnimatorManager.UpdateAnimatorMovementParameters(0, moveAmount, player.playerNetworkManager.isSprinting.Value);
        }
        else
        {
            player.playerAnimatorManager.UpdateAnimatorMovementParameters(horizontalInput, verticalInput, player.playerNetworkManager.isSprinting.Value);
        }


        //player.playerAnimatorManager.UpdateAnimatorMovementParameters(0, moveAmount, player.playerNetworkManager.isSprinting.Value);
    }

    private void HandleCameraMovementInput()
    {
        if (player.characterSpellManager.inSpellMode)
        {
            cameraVerticalInput = 0;
            cameraHorizontalInput = 0;
        }
        else
        {
            cameraVerticalInput = cameraInput.y;
            cameraHorizontalInput = cameraInput.x;
        }

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

    private void HandleSprintInput()
    {
        if (sprint_Input)
        {
            player.playerLocomotionManager.HandleSprinting();
        }
        else
        {
            player.playerNetworkManager.isSprinting.Value = false;
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
        // 1
        if (actionNumber1Input)
        {
            actionNumber1Input = false;
            player.playerEquipmentManager.SwitchOffHandWeapon();
        }
        // 2
        if (actionNumber2Input)
        {
            actionNumber2Input = false;
            player.playerEquipmentManager.SwitchMainHandWeapon();
        }
        // 3
        if (actionNumber3Input)
        {
            actionNumber3Input = false;
        }
        // 4
        if (actionNumber4Input)
        {
            actionNumber4Input = false;
        }
        // 5
        if (actionNumber5Input)
        {
            actionNumber5Input = false;
        }
        // 6
        if (actionNumber6Input)
        {
            actionNumber6Input = false;
        }
        // 7
        if (actionNumber7Input)
        {
            actionNumber7Input = false;
        }
        // 8
        if (actionNumber8Input)
        {
            actionNumber8Input = false;
        }
        // 9
        if (actionNumber9Input)
        {
            actionNumber9Input = false;
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

    // Action Buttons
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

    public void HandleAction4InputButton()
    {
        actionNumber4Input = true;
        HandleActionInputs();
    }

    public void HandleAction5InputButton()
    {
        actionNumber5Input = true;
        HandleActionInputs();
    }

    public void HandleAction6InputButton()
    {
        actionNumber6Input = true;
        HandleActionInputs();
    }

    public void HandleAction7InputButton()
    {
        actionNumber7Input = true;
        HandleActionInputs();
    }

    public void HandleAction8InputButton()
    {
        actionNumber8Input = true;
        HandleActionInputs();
    }

    public void HandleAction9InputButton()
    {
        actionNumber9Input = true;
        HandleActionInputs();
    }

    //// Attack Inputs ////
    private void HandleMouseAttackInput()
    {
        // LEFT MOUSE CLICK
        if (mainHandAttackInput)
        {
            mainHandAttackInput = false;
            // TODO: IF WE HAVE A UI WINDOW OPEN, RETURN AND DO NOTHING

            player.playerNetworkManager.SetCharacterActionHand(true);
            // TODO: IF WE ARE TWO HANDING THE WEAPON, USE THE TWO HANDED ACTION

            player.playerCombatManager.PerformWeaponBasedAction(player.playerInventoryManager.currentMainHandWeapon.oneHandMainHandMouseAttack, player.playerInventoryManager.currentMainHandWeapon);
        }
    }

    private void HandleMouseHeavyAttackInput()
    {
        // LEFT MOUSE CLICK
        if (mainHandHeavyAttackInput)
        {
            mainHandHeavyAttackInput = false;
            // TODO: IF WE HAVE A UI WINDOW OPEN, RETURN AND DO NOTHING
            player.playerNetworkManager.SetCharacterActionHand(true);
            // TODO: IF WE ARE TWO HANDING THE WEAPON, USE THE TWO HANDED ACTION

            player.playerCombatManager.PerformWeaponBasedAction(player.playerInventoryManager.currentMainHandWeapon.oneHandHeavyMainHandMouseAttack, player.playerInventoryManager.currentMainHandWeapon);
        }
    }

    private void HandleMouseChargeAttackInput()
    {
        if (player.isPerformingAction)
        {
            if (player.playerNetworkManager.isUsingMainHand.Value)
            {
                player.playerNetworkManager.isChargingMainHandAttack.Value = mainHandChargeAttackInput;
            }
        }
    }

    private void QueInput(ref bool quedInput)   //  PASSING A REFERENCE MEANS WE PASS A SPECIFIC BOOL, AND NOT THE VALUE OF THAT BOOL (TRUE OR FALSE)
    {
        //  RESET ALL QUED INPUTS SO ONLY ONE CAN QUE AT A TIME
        que_RB_Input = false;
        que_RT_Input = false;
        //que_LB_Input = false;
        //que_LT_Input = false;

        //  CHECK FOR UI WINDOW BEING OPEN, IF ITS OPEN RETURN

        if (player.isPerformingAction || player.playerNetworkManager.isJumping.Value)
        {
            quedInput = true;
            que_Input_Timer = default_Que_Input_Time;
            input_Que_Is_Active = true;
        }
    }

    private void ProcessQuedInput()
    {
        if (player.isDead.Value)
            return;

        if (que_RB_Input)
            mainHandAttackInput = true;

        if (que_RT_Input)
            mainHandHeavyAttackInput = true;
    }

    private void HandleQuedInputs()
    {
        if (input_Que_Is_Active)
        {
            //  WHILE THE TIMER IS ABOVE 0, KEEP ATTEMPTING TO PRESS THE INPUT
            if (que_Input_Timer > 0)
            {
                que_Input_Timer -= Time.deltaTime;
                ProcessQuedInput();
            }
            else
            {
                //  RESET ALL QUED INPUTS
                que_RB_Input = false;
                que_RT_Input = false;

                input_Que_Is_Active = false;
                que_Input_Timer = 0;
            }
        }
    }

}
