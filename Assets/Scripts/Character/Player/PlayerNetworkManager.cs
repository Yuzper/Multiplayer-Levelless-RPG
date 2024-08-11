using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Netcode;
using Unity.Collections;
using UnityEngine.TextCore.Text;

public class PlayerNetworkManager : CharacterNetworkManager
{
    PlayerManager player;

    public NetworkVariable<FixedString64Bytes> characterName = new NetworkVariable<FixedString64Bytes>("Character", NetworkVariableReadPermission.Everyone, NetworkVariableWritePermission.Owner);

    [Header("Equipment")]
    public NetworkVariable<int> currentWeaponBeingUsed = new NetworkVariable<int>(0, NetworkVariableReadPermission.Everyone, NetworkVariableWritePermission.Owner);
    public NetworkVariable<int> currentMainHandWeaponID = new NetworkVariable<int>(0, NetworkVariableReadPermission.Everyone, NetworkVariableWritePermission.Owner);
    public NetworkVariable<int> currentOffHandWeaponID = new NetworkVariable<int>(0, NetworkVariableReadPermission.Everyone, NetworkVariableWritePermission.Owner);
    public NetworkVariable<bool> isUsingMainHand = new NetworkVariable<bool>(false, NetworkVariableReadPermission.Everyone, NetworkVariableWritePermission.Owner);
    public NetworkVariable<bool> isUsingOffHand = new NetworkVariable<bool>(false, NetworkVariableReadPermission.Everyone, NetworkVariableWritePermission.Owner);

    protected override void Awake()
    {
        base.Awake();

        player = GetComponent<PlayerManager>();
    }

    
    public void SetCharacterActionHand(bool mainHandedAction)
    {
        if (mainHandedAction)
        {
            isUsingMainHand.Value = true;
            isUsingOffHand.Value = false;
        }
        else
        {
            isUsingMainHand.Value = false;
            isUsingOffHand.Value = true;
        }
    }

    public override void CheckHP(float oldValue, float newValue)
    {
        if (currentHealth.Value <= (maxHealth.Value * 0.1f) && currentHealth.Value >= 0) // If player has less than 10% of maxHealth but not 0.
        {
            PlayerUIManager.instance.playerUIPopUpManager.SendAbilityAndResourceErrorPopUp("Low Health!", true, false, false);
        }

        base.CheckHP(oldValue, newValue);
    }



public void SetNewMaxHealthValue(int oldConstitution, int newConstitution)
    {
        maxHealth.Value = player.playerStatsManager.CalculateHealthBasedOnConstitution(newConstitution);
        PlayerUIManager.instance.playerUIHudManager.SetMaxHealthValue(maxHealth.Value);
        currentHealth.Value = maxHealth.Value; // When setting new max fill resource bar to max like a level up
    }
    public void SetNewMaxManaValue(int oldIntelligence, int newIntelligence)
    {
        maxMana.Value = player.playerStatsManager.CalculateManaBasedOnIntelligence(newIntelligence);
        PlayerUIManager.instance.playerUIHudManager.SetMaxStaminaValue(maxMana.Value);
        currentMana.Value = maxMana.Value; // When setting new max fill resource bar to max like a level up
    }
    public void SetNewMaxStaminaValue(int oldEndurance, int newEndurance)
    {
        maxStamina.Value = player.playerStatsManager.CalculateStaminaBasedOnEndurance(newEndurance);
        PlayerUIManager.instance.playerUIHudManager.SetMaxStaminaValue(maxStamina.Value);
        currentStamina.Value = maxStamina.Value; // When setting new max fill resource bar to max like a level up
    }

    public void OnCurrentMainHandWeaponIDChange(int oldID, int newID)
    {
        WeaponItems newWeapon = Instantiate(WorldItemDatabase.instance.GetWeaponByID(newID));
        player.playerInventoryManager.currentMainHandWeapon = newWeapon;
        player.playerEquipmentManager.LoadMainHandWeapon();
    }

    public void OnCurrentOffHandWeaponIDChange(int oldID, int newID)
    {
        WeaponItems newWeapon = Instantiate(WorldItemDatabase.instance.GetWeaponByID(newID));
        player.playerInventoryManager.currentOffHandWeapon = newWeapon;
        player.playerEquipmentManager.LoadOffHandWeapon();
    }

    public void OnCurrentWeaponBeingUsedIDChange(int oldID, int newID)
    {
        WeaponItems newWeapon = Instantiate(WorldItemDatabase.instance.GetWeaponByID(newID));
        player.playerCombatManager.currentWeaponBeingUsed = newWeapon;

        // we dont need to run this code if we are the owner because is this done already locally
        if (!player.IsOwner) return;

        if(player.playerCombatManager.currentWeaponBeingUsed != null)
        {
            player.playerAnimatorManager.UpdateAnimatorController(player.playerCombatManager.currentWeaponBeingUsed.weaponAnimator);
        }
    }

    public override void OnIsBlockingChanged(bool old, bool newvalue)
    {
        base.OnIsBlockingChanged(old, newvalue);
        if (IsOwner)
        {
            player.playerStatsManager.blockingPhysicalAbsorption = player.playerCombatManager.currentWeaponBeingUsed.physicalBaseDamageAbsoption;
            player.playerStatsManager.blockingMagicAbsorption = player.playerCombatManager.currentWeaponBeingUsed.magicBaseDamageAbsoption;
            player.playerStatsManager.blockingFireAbsorption = player.playerCombatManager.currentWeaponBeingUsed.fireBaseDamageAbsoption;
            player.playerStatsManager.blockingHolyAbsorption = player.playerCombatManager.currentWeaponBeingUsed.holyBaseDamageAbsoption;
            player.playerStatsManager.blockingLightningAbsorption = player.playerCombatManager.currentWeaponBeingUsed.lightningBaseDamageAbsoption;
            player.playerStatsManager.blockingStability = player.playerCombatManager.currentWeaponBeingUsed.stability;
        }
    }

    // ITEM ACTIONS
    [ServerRpc]
    public void NotifyTheServerOfWeaponActionServerRpc(ulong clientID, int actionID, int weaponID)
    {
        if (IsServer)
        {
            NotifyTheServerOfWeaponActionClientRpc(clientID, actionID, weaponID);
        }
    }

    [ClientRpc]
    public void NotifyTheServerOfWeaponActionClientRpc(ulong clientID, int actionID, int weaponID)
    {
        if (clientID != NetworkManager.Singleton.LocalClientId)
        {
            PerformWeaponBasedAction(actionID, weaponID);
        }
    }

    private void PerformWeaponBasedAction(int actionID, int weaponID)
    {
        WeaponItemAction weaponAction = WorldActionManager.instance.GetWeaponItemActionByID(actionID);

        if (weaponAction != null)
        {
            // SETS TURN TORWARDS LOOKING
            weaponAction.AttemptToPerformAction(player, WorldItemDatabase.instance.GetWeaponByID(weaponID));
        }
        else
        {
            Debug.LogError("ACTION IS NULL, CANNOT BE PERFORMED");
        }
    }


    [ServerRpc]
    public void NotifyTheServerOfSpellEquipServerRpc(ulong clientID, int spellID)
    {
        if (IsServer)
        {
            NotifyTheServerOfSpellEquipClientRpc(clientID, spellID);
        }
    }

    [ClientRpc]
    public void NotifyTheServerOfSpellEquipClientRpc(ulong clientID, int spellID)
    {
        if (clientID != NetworkManager.Singleton.LocalClientId)
        {
            player.characterSpellManager.equippedSpell = player.characterSpellManager.spell_List[spellID];
        }
    }


}

