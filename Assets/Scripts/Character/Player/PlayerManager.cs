using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerManager : CharacterManager
{
    [Header("DEBUG MENU")]
    [SerializeField] bool respawnCharacter = false;

    [HideInInspector] public PlayerAnimatorManager playerAnimatorManager;
    [HideInInspector] public PlayerLocomotionManager playerLocomotionManager;
    [HideInInspector] public PlayerNetworkManager playerNetworkManager;
    [HideInInspector] public PlayerStatsManager playerStatsManager;
    [HideInInspector] public PlayerInventoryManager playerInventoryManager;

    protected override void Awake()
    {
        base.Awake();

        // DO MORE FOR PLAYER
        playerLocomotionManager = GetComponent<PlayerLocomotionManager>();
        playerAnimatorManager = GetComponent<PlayerAnimatorManager>();
        playerNetworkManager = GetComponent<PlayerNetworkManager>();
        playerStatsManager = GetComponent<PlayerStatsManager>();
        playerInventoryManager = GetComponent<PlayerInventoryManager>();
    }

    protected override void Update()
    {
        base.Update();
        // IF WE DO NOT OWN THIS GAMEOBJECT, WE DO NOT CONTROL OR EDIT IT
        if (!IsOwner) return;

        // HANDLE ALL CHARACTER MOVEMENT
        playerLocomotionManager.HandleAllMovement();

        // REGEN MANA
        playerStatsManager.RegenerateMana();

        DebugMenu();
    }

    protected override void LateUpdate()
    {
        if (!IsOwner) return;

        base.LateUpdate();

        PlayerCamera.instance.HandleAllCameraActions();
    }

    public override void OnNetworkSpawn()
    {
        base.OnNetworkSpawn();

        // IF THIS IS THE PLAYER OBJECT OWNED BY THIS CLIENT
        if (IsOwner)
        {
            PlayerCamera.instance.player = this;
            PlayerInputManager.instance.player = this;
            WorldSaveGameManager.instance.player = this;

            // UPDATES THE TOTAL AMOUNT OF HEALTH OR MANA WHEN THE STAT LINKED TO EITHER CHANGES
            playerNetworkManager.constitution.OnValueChanged += playerNetworkManager.SetNewMaxHealthValue;
            playerNetworkManager.intelligence.OnValueChanged += playerNetworkManager.SetNewMaxManaValue;

            // UPDATES UI STAT BARS WHEN A STAT CHANGES (HEALTH OR MANA ETC)
            playerNetworkManager.currentHealth.OnValueChanged += PlayerUIManager.instance.playerUIHudManager.SetNewHealthValue;
            playerNetworkManager.currentMana.OnValueChanged += PlayerUIManager.instance.playerUIHudManager.SetNewManaValue;
            playerNetworkManager.currentMana.OnValueChanged += playerStatsManager.ResetManaRegenTimer;
        }
        playerNetworkManager.currentHealth.OnValueChanged += playerNetworkManager.CheckHP;
    }

    public override IEnumerator ProcessDeathEvent(bool manuallySelectDeathAnimation = false)
    {
        if (IsOwner)
        {
            PlayerUIManager.instance.playerUIPopUpManager.SendYouDiedPopUp();
        }

        return base.ProcessDeathEvent(manuallySelectDeathAnimation);
    }


    public override void ReviveCharacter()
    {
        base.ReviveCharacter();

        if (IsOwner)
        {
            playerNetworkManager.currentHealth.Value = playerNetworkManager.maxHealth.Value;
            playerNetworkManager.currentMana.Value = playerNetworkManager.maxMana.Value;
            // Play rebirth effects
            playerAnimatorManager.PlayerTargetActionAnimation("Empty", false);

            // Reenable control over player movement
            canRotate = true;
            canMove = true;
        }
    }

    // SAVES VARIABLES, STATS ETC TO THE SAVE FILE DATA
    public void SaveGameDataToCurrentCharacterData(ref CharacterSaveData currentCharacterData)
    {
        //currentCharacterData.sceneIndex = SceneManager.GetActiveScene().buildIndex;
        currentCharacterData.sceneIndex = GetSceneIndex();

        currentCharacterData.characterName = playerNetworkManager.characterName.Value.ToString();
        currentCharacterData.xPosition = transform.position.x;
        currentCharacterData.yPosition = transform.position.y;
        currentCharacterData.zPosition = transform.position.z;

        currentCharacterData.currentHealth = playerNetworkManager.currentHealth.Value;
        currentCharacterData.currentMana = playerNetworkManager.currentMana.Value;

        currentCharacterData.intelligence = playerNetworkManager.intelligence.Value;
        currentCharacterData.constitution = playerNetworkManager.constitution.Value;
        currentCharacterData.fortitude = playerNetworkManager.fortitude.Value;
    }

    public void LoadGameDataFromCurrentCharacterData(ref CharacterSaveData currentCharacterData)
    {
        playerNetworkManager.characterName.Value = currentCharacterData.characterName;
        Vector3 myPosition = new Vector3(currentCharacterData.xPosition, currentCharacterData.yPosition, currentCharacterData.zPosition);
        transform.position = myPosition;

        playerNetworkManager.intelligence.Value = currentCharacterData.intelligence;
        playerNetworkManager.constitution.Value = currentCharacterData.constitution;
        playerNetworkManager.fortitude.Value = currentCharacterData.fortitude;

        playerNetworkManager.maxHealth.Value = playerStatsManager.CalculateHealthBasedOnConstitution(currentCharacterData.constitution);
        playerNetworkManager.maxMana.Value = playerStatsManager.CalculateManaBasedOnIntelligence(currentCharacterData.intelligence);
        playerNetworkManager.currentHealth.Value = currentCharacterData.currentHealth;
        playerNetworkManager.currentMana.Value = currentCharacterData.currentMana;
        PlayerUIManager.instance.playerUIHudManager.SetMaxManaValue(playerNetworkManager.maxMana.Value);

    }

    // IS USED TO RETURN THE CURRENT SCENE ID EXCEPT WHEN IT IS THE MAIN MENU ID, FIXED BUG IN "NEWGAME" FUNCTION IN "WORLDSAVEGAMEMANAGER"
    private int GetSceneIndex()
    {
        int buildId = SceneManager.GetActiveScene().buildIndex;
        if (buildId != 0) // Main menu ID
        {
            return buildId;
        }
        else
        {
            return 1; // Default to the first Game Scene that is not Main Menu
        }
    }


    // DEBUG DELETE LATER
    private void DebugMenu()
    {
        if (respawnCharacter)
        {
            respawnCharacter = false;
            ReviveCharacter();
        }
    }


}

