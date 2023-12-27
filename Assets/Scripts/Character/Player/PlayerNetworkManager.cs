using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Netcode;
using Unity.Collections;

public class PlayerNetworkManager : CharacterNetworkManager
{
    PlayerManager player;

    public NetworkVariable<FixedString64Bytes> characterName = new NetworkVariable<FixedString64Bytes>("Character", NetworkVariableReadPermission.Everyone, NetworkVariableWritePermission.Owner);

    [Header("Equipment")]
    public NetworkVariable<int> currentRightHandWeaponID = new NetworkVariable<int>(0, NetworkVariableReadPermission.Everyone, NetworkVariableWritePermission.Owner);
    public NetworkVariable<int> currentLeftHandWeaponID = new NetworkVariable<int>(0, NetworkVariableReadPermission.Everyone, NetworkVariableWritePermission.Owner);

    protected override void Awake()
    {
        base.Awake();

        player = GetComponent<PlayerManager>();
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
        PlayerUIManager.instance.playerUIHudManager.SetMaxManaValue(maxMana.Value);
        currentMana.Value = maxMana.Value; // When setting new max fill resource bar to max like a level up
    }
    
    public void OnCurrentRightHandWeaponIDChange(int oldID, int newID)
    {
        WeaponItems newWeapon = Instantiate(WorldItemDatabase.instance.GetWeaponByID(newID));
        player.playerInventoryManager.currentRightHandWeapon = newWeapon;
        player.playerEquipmentManager.LoadRightWeapon();
    }

    public void OnCurrentLeftHandWeaponIDChange(int oldID, int newID)
    {
        WeaponItems newWeapon = Instantiate(WorldItemDatabase.instance.GetWeaponByID(newID));
        player.playerInventoryManager.currentLeftHandWeapon = newWeapon;
        player.playerEquipmentManager.LoadLeftWeapon();
    }
}

