using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using Unity.Netcode;

public class PlayerManager : CharacterManager
{
    [HideInInspector] public PlayerAnimatorManager playerAnimatorManager;
    [HideInInspector] public PlayerLocomotionManager playerLocomotionManager;
    [HideInInspector] public PlayerNetworkManager playerNetworkManager;
    [HideInInspector] public PlayerSoundFXManager playerSoundFXManager;
    [HideInInspector] public PlayerStatsManager playerStatsManager;
    [HideInInspector] public PlayerInventoryManager playerInventoryManager;
    [HideInInspector] public PlayerEquipmentManager playerEquipmentManager;
    [HideInInspector] public PlayerCombatManager playerCombatManager;
    [HideInInspector] public PlayerSpellManager playerSpellManager;
    [HideInInspector] public PlayerInteractionManager playerInteractionManager;

    protected override void Awake()
    {
        base.Awake();

        // DO MORE FOR PLAYER
        playerLocomotionManager = GetComponent<PlayerLocomotionManager>();
        playerAnimatorManager = GetComponent<PlayerAnimatorManager>();
        playerNetworkManager = GetComponent<PlayerNetworkManager>();
        playerSoundFXManager = GetComponent<PlayerSoundFXManager>();
        playerStatsManager = GetComponent<PlayerStatsManager>();
        playerInventoryManager = GetComponent<PlayerInventoryManager>();
        playerEquipmentManager = GetComponent<PlayerEquipmentManager>();
        playerCombatManager = GetComponent<PlayerCombatManager>();
        playerSpellManager = GetComponent<PlayerSpellManager>();
        playerInteractionManager = GetComponent<PlayerInteractionManager>();
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
        // REGEN STAMINA
        playerStatsManager.RegenerateStamina();
    }

    protected override void LateUpdate()
    {
        if (!IsOwner) return;

        base.LateUpdate();

        PlayerCamera.instance.HandleAllCameraActions();
    }

    protected override void OnEnable()
    {
        base.OnEnable();


    }

    protected override void OnDisable()
    {
        base.OnDisable();

    }

    // WHEN ADDING TO NETWORK-SPAWN ALSO ADD TO ONNETWORK-DESPAWN
    public override void OnNetworkSpawn()
    {
        base.OnNetworkSpawn();
        NetworkManager.Singleton.OnClientConnectedCallback += OnClientConnectedCallback;

        // IF THIS IS THE PLAYER OBJECT OWNED BY THIS CLIENT
        if (IsOwner)
        {
            PlayerCamera.instance.player = this;
            PlayerInputManager.instance.player = this;
            PlayerUIManager.instance.player = this;
            WorldSaveGameManager.instance.player = this;

            // UPDATES THE TOTAL AMOUNT OF HEALTH OR MANA WHEN THE STAT LINKED TO EITHER CHANGES
            playerNetworkManager.constitution.OnValueChanged += playerNetworkManager.SetNewMaxHealthValue;
            playerNetworkManager.intelligence.OnValueChanged += playerNetworkManager.SetNewMaxManaValue;
            playerNetworkManager.endurance.OnValueChanged += playerNetworkManager.SetNewMaxStaminaValue;

            // UPDATES UI STAT BARS WHEN A STAT CHANGES (HEALTH OR MANA ETC)
            playerNetworkManager.currentHealth.OnValueChanged += PlayerUIManager.instance.playerUIHudManager.SetNewHealthValue;
            playerNetworkManager.currentMana.OnValueChanged += PlayerUIManager.instance.playerUIHudManager.SetNewManaValue;
            playerNetworkManager.currentMana.OnValueChanged += playerStatsManager.ResetManaRegenTimer;
            playerNetworkManager.currentStamina.OnValueChanged += PlayerUIManager.instance.playerUIHudManager.SetNewStaminaValue;
            playerNetworkManager.currentStamina.OnValueChanged += playerStatsManager.ResetStaminaRegenTimer;
        }

        // ONLY UPDATE FLOATING HP BAR IF THIS CHARACTER IS NOT YOUR LOCAL PLAYERS CHARACTER (YOU DONT WANNA SEE A HP BAR FLOATING ABOVE YOUR OWN HEAD)
        if (!IsOwner)
        {
            characterNetworkManager.currentHealth.OnValueChanged += characterUIManger.OnHPChanged;
        }


        // STATS
        // Moved to CharacterManager
        //playerNetworkManager.currentHealth.OnValueChanged += playerNetworkManager.CheckHP;
        // Set Health based on Constitution
        if (IsOwner)
        {
            characterNetworkManager.maxHealth.Value = characterStatsManager.CalculateHealthBasedOnConstitution(characterNetworkManager.constitution.Value);
            characterNetworkManager.currentHealth.Value = characterNetworkManager.maxHealth.Value;
            // Set Mana based on Intelligence
            characterNetworkManager.maxMana.Value = characterStatsManager.CalculateManaBasedOnIntelligence(characterNetworkManager.intelligence.Value);
            characterNetworkManager.currentMana.Value = characterNetworkManager.maxMana.Value;
            // Set Stamina based on Endurance
            characterNetworkManager.maxMana.Value = characterStatsManager.CalculateStaminaBasedOnEndurance(characterNetworkManager.endurance.Value);
            characterNetworkManager.currentStamina.Value = characterNetworkManager.maxStamina.Value;
        }

        // LOCK ON
        playerNetworkManager.isLockedOn.OnValueChanged += playerNetworkManager.OnIsLockedOnChanged;
        playerNetworkManager.currentTargetNetworkObjectID.OnValueChanged += playerNetworkManager.OnLockOnTargetIDChange;

        // EQUIPMENT
        playerNetworkManager.currentMainHandWeaponID.OnValueChanged += playerNetworkManager.OnCurrentMainHandWeaponIDChange;
        playerNetworkManager.currentOffHandWeaponID.OnValueChanged += playerNetworkManager.OnCurrentOffHandWeaponIDChange;
        playerNetworkManager.currentWeaponBeingUsed.OnValueChanged += playerNetworkManager.OnCurrentWeaponBeingUsedIDChange;

        // FLAGS
        playerNetworkManager.isChargingMainHandAttack.OnValueChanged += playerNetworkManager.OnIsChargingAttackChanged;
        playerNetworkManager.isHoldingDownSpell.OnValueChanged += playerNetworkManager.OnIsHoldingDownSpellChanged;


        // UPON CONNECTING, IF WE ARE THE OWNER OF THIS CHARACTER, BUT WE ARE NOT THE SERVER, RELOAD OUR CHRACTER DATA TO THIS NEWLY INSTANTIATED CHARACTER
        // WE DON'T RUN THIS IF WE ARE THE SERVER, BECAUSE SINCE THEY ARE THE HOST, THEY ARE ALREADY LOADED IN AND DON'T NEED TO RELOAD THEIR DATA
        if (IsOwner && !IsServer)
        {
            LoadGameDataFromCurrentCharacterData(ref WorldSaveGameManager.instance.currentCharacterData);
        }
    }

    // WHEN ADDING TO NETWORK-SPAWN ALSO ADD TO ONNETWORK-DESPAWN
    public override void OnNetworkDespawn()
    {
        base.OnNetworkDespawn();
        NetworkManager.Singleton.OnClientConnectedCallback -= OnClientConnectedCallback;

        // IF THIS IS THE PLAYER OBJECT OWNED BY THIS CLIENT
        if (IsOwner)
        {
            // UPDATES THE TOTAL AMOUNT OF HEALTH OR MANA WHEN THE STAT LINKED TO EITHER CHANGES
            playerNetworkManager.constitution.OnValueChanged -= playerNetworkManager.SetNewMaxHealthValue;
            playerNetworkManager.intelligence.OnValueChanged -= playerNetworkManager.SetNewMaxManaValue;
            playerNetworkManager.endurance.OnValueChanged -= playerNetworkManager.SetNewMaxStaminaValue;

            // UPDATES UI STAT BARS WHEN A STAT CHANGES (HEALTH OR MANA ETC)
            playerNetworkManager.currentHealth.OnValueChanged -= PlayerUIManager.instance.playerUIHudManager.SetNewHealthValue;
            playerNetworkManager.currentMana.OnValueChanged -= PlayerUIManager.instance.playerUIHudManager.SetNewManaValue;
            playerNetworkManager.currentMana.OnValueChanged -= playerStatsManager.ResetManaRegenTimer;
            playerNetworkManager.currentStamina.OnValueChanged -= PlayerUIManager.instance.playerUIHudManager.SetNewStaminaValue;
            playerNetworkManager.currentStamina.OnValueChanged -= playerStatsManager.ResetStaminaRegenTimer;
        }

        if (!IsOwner)
        {
            characterNetworkManager.currentHealth.OnValueChanged -= characterUIManger.OnHPChanged;
        }


        // STATS
        //playerNetworkManager.currentHealth.OnValueChanged -= playerNetworkManager.CheckHP;

        // LOCK ON
        playerNetworkManager.isLockedOn.OnValueChanged -= playerNetworkManager.OnIsLockedOnChanged;
        playerNetworkManager.currentTargetNetworkObjectID.OnValueChanged -= playerNetworkManager.OnLockOnTargetIDChange;

        // EQUIPMENT
        playerNetworkManager.currentMainHandWeaponID.OnValueChanged -= playerNetworkManager.OnCurrentMainHandWeaponIDChange;
        playerNetworkManager.currentOffHandWeaponID.OnValueChanged -= playerNetworkManager.OnCurrentOffHandWeaponIDChange;
        playerNetworkManager.currentWeaponBeingUsed.OnValueChanged -= playerNetworkManager.OnCurrentWeaponBeingUsedIDChange;

        // FLAGS
        playerNetworkManager.isChargingMainHandAttack.OnValueChanged -= playerNetworkManager.OnIsChargingAttackChanged;
        playerNetworkManager.isHoldingDownSpell.OnValueChanged -= playerNetworkManager.OnIsHoldingDownSpellChanged;
    }

    private void OnClientConnectedCallback(ulong clientID)
    {
        WorldGameSessionManager.instance.AddPlayerToActivePlayersList(this);

        // IF WE ARE THE SERVER, WE ARE THE HOST, SO WE DONT NEED TO LOAD PLAYERS TO SYNC THEM
        // YOU ONLY NEED TO LOAD OTHER PLAYERS GEAR TO SYNC IT IF YOU JOIN A GAME THATS ALREADY BEEN ACTIVE WITHOUT YOU BEING PRESENT
        if (!IsServer && IsOwner)
        {
            foreach (var player in WorldGameSessionManager.instance.players)
            {
                if (player != this)
                {
                    player.LoadOtherPlayersCharacterWhenJoiningServer();
                }
            }
        }
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
            isDead.Value = false;
            playerNetworkManager.currentHealth.Value = playerNetworkManager.maxHealth.Value;
            playerNetworkManager.currentMana.Value = playerNetworkManager.maxMana.Value;
            playerNetworkManager.currentStamina.Value = playerNetworkManager.maxStamina.Value;

            // Play rebirth effects
            playerAnimatorManager.PlayerTargetActionAnimation("Empty", false);

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
        currentCharacterData.currentStamina = playerNetworkManager.currentStamina.Value;

        currentCharacterData.intelligence = playerNetworkManager.intelligence.Value;
        currentCharacterData.constitution = playerNetworkManager.constitution.Value;
        currentCharacterData.endurance = playerNetworkManager.endurance.Value;
        currentCharacterData.fortitude = playerNetworkManager.fortitude.Value;
    }

    public void LoadGameDataFromCurrentCharacterData(ref CharacterSaveData currentCharacterData)
    {
        playerNetworkManager.characterName.Value = currentCharacterData.characterName;
        Vector3 myPosition = new Vector3(currentCharacterData.xPosition, currentCharacterData.yPosition, currentCharacterData.zPosition);
        transform.position = myPosition;

        playerNetworkManager.intelligence.Value = currentCharacterData.intelligence;
        playerNetworkManager.constitution.Value = currentCharacterData.constitution;
        playerNetworkManager.endurance.Value = currentCharacterData.endurance;
        playerNetworkManager.fortitude.Value = currentCharacterData.fortitude;

        playerNetworkManager.maxHealth.Value = playerStatsManager.CalculateHealthBasedOnConstitution(currentCharacterData.constitution);
        playerNetworkManager.maxMana.Value = playerStatsManager.CalculateManaBasedOnIntelligence(currentCharacterData.intelligence);
        playerNetworkManager.maxStamina.Value = playerStatsManager.CalculateStaminaBasedOnEndurance(currentCharacterData.endurance);
        playerNetworkManager.currentHealth.Value = currentCharacterData.currentHealth;
        playerNetworkManager.currentMana.Value = currentCharacterData.currentMana;
        playerNetworkManager.currentStamina.Value = currentCharacterData.currentStamina;
        PlayerUIManager.instance.playerUIHudManager.SetMaxManaValue(playerNetworkManager.maxMana.Value);
        PlayerUIManager.instance.playerUIHudManager.SetMaxStaminaValue(playerNetworkManager.maxStamina.Value);

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

    // USED IF YOU JOIN AN ACTIVE SESSION, THEN YOU LOAD THE CURRENT EQUIPMENT FOR THE OTHER PLAYERS ON YOUR END
    public void LoadOtherPlayersCharacterWhenJoiningServer()
    {
        // SYNC WEAPONS
        playerNetworkManager.OnCurrentMainHandWeaponIDChange(0, playerNetworkManager.currentMainHandWeaponID.Value);
        playerNetworkManager.OnCurrentOffHandWeaponIDChange(0, playerNetworkManager.currentOffHandWeaponID.Value);

        // ARMOR

        // LOCK ON
        if (playerNetworkManager.isLockedOn.Value)
        {
            playerNetworkManager.OnLockOnTargetIDChange(0, playerNetworkManager.currentTargetNetworkObjectID.Value);
        }
    }


}

