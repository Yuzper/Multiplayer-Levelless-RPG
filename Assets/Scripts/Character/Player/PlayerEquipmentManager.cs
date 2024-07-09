using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerEquipmentManager : CharacterEquipmentManager
{
    PlayerManager player;
    public WeaponModelInstantiationSlot mainHandSlot;
    public WeaponModelInstantiationSlot offHandSlot;

    [SerializeField] WeaponManager mainHandWeaponManager;
    [SerializeField] WeaponManager offHandWeaponManager;

    public GameObject mainHandWeaponModel;
    public GameObject offHandWeaponModel;

    // A quick fix for preventing the draw weapon sound of unarmed weapon to play when character spawns.
    private int playSoundCheck = 0;

    public SpellDrawingManager spellDrawingCanvas;

    protected override void Awake()
    {
        base.Awake();
        player = GetComponent<PlayerManager>();
        IntializeWeaponSlots();
    }

    protected override void Start()
    {
        base.Start();
        spellDrawingCanvas = SpellDrawingManager.instance;
        LoadWeaponOnBothHands();
    }

    // Getters
    public WeaponManager get_mainHandWeaponManager()
    {
        return mainHandWeaponManager;
    }

    public WeaponManager get_offHandWeaponManager()
    {
        return offHandWeaponManager;
    }



    private void IntializeWeaponSlots()
    {
        WeaponModelInstantiationSlot[] weaponSlots = GetComponentsInChildren<WeaponModelInstantiationSlot>();
        
        foreach (var weaponSlot in weaponSlots)
        {
            if (weaponSlot.weaponSlot == WeaponModelSlot.RightHand)
            {
                mainHandSlot = weaponSlot;
            }
            else if (weaponSlot.weaponSlot == WeaponModelSlot.LeftHand)
            {
                offHandSlot = weaponSlot;
            }
        }
    }

    public void LoadWeaponOnBothHands()
    {
        LoadMainHandWeapon();
        LoadOffHandWeapon();
    }

    // RIGHT WEAPON
    public void LoadMainHandWeapon()
    {
        if (player.playerInventoryManager.currentMainHandWeapon != null)
        {
            // REMOVE OLD WEAPON
            mainHandSlot.UnloadWeapon();

            // BRING IN NEW WEAPON
            mainHandWeaponModel = Instantiate(player.playerInventoryManager.currentMainHandWeapon.weaponModel);
            mainHandSlot.LoadWeapon(mainHandWeaponModel);
            // ASSIGN WEAPONS DAMAGE, TO ITS COLLIDER
            mainHandWeaponManager = mainHandWeaponModel.GetComponent<WeaponManager>();
            mainHandWeaponManager.SetWeaponDamage(player, player.playerInventoryManager.currentMainHandWeapon);
            // Play draw weapon sound
            DecideDrawWeaponSound(player.playerInventoryManager.currentMainHandWeapon);
        }
    }
    /*
    public void SwitchRightWeapon_old()
    {
        if (!player.IsOwner) return;

        // PLAY SWAPPING ANIMATION                               AnimationName ,isPerformingAction, applyRootMotion, canRotate, canMove
        player.playerAnimatorManager.PlayerTargetActionAnimation("Swap_Weapon_Right", false, false, true, true);

        WeaponItems selectedWeapon = null;
        // TOGO https://youtu.be/xrw_yOGp9Jo?si=t52MywHL2l_Lq-Np&t=644
        // From this timestamp he explains Elden Ring/Dark Souls weapon system, which is the initial implementation, might be changed later on.

        // ADD ONE TO OUR INDEX TO SWITCH TO THE NEXT POTENTIAL WEAPON
        player.playerInventoryManager.rightHandWeaponIndex += 1;

        // IF OUR INDEX IS OUT OF BOUNDS, RESET IT TO POSITION #1 (0)
        if (player.playerInventoryManager.rightHandWeaponIndex < 0 || player.playerInventoryManager.rightHandWeaponIndex > 2)
        {
            player.playerInventoryManager.rightHandWeaponIndex = 0;

            // WE CHECK IF WE ARE HOLDING, MORE THAN ONE WEAPON
            float weaponCount = 0;
            WeaponItems firstWeapon = null;
            int firstWeaponPosition = 0;

            for (int i = 0; i < player.playerInventoryManager.weaponsInRightHandSlots.Length; i++)
            {
                if (player.playerInventoryManager.weaponsInRightHandSlots[i].itemID != WorldItemDatabase.instance.unarmedWeapon.itemID)
                {
                    weaponCount += 1;

                    if (firstWeapon == null)
                    {
                        firstWeapon = player.playerInventoryManager.weaponsInRightHandSlots[i];
                        firstWeaponPosition = i;
                    }
                }
            }

            if (weaponCount <= 1)
            {
                player.playerInventoryManager.rightHandWeaponIndex = -1;
                selectedWeapon = WorldItemDatabase.instance.unarmedWeapon;
                player.playerNetworkManager.currentRightHandWeaponID.Value = selectedWeapon.itemID;
            }
            else
            {
                player.playerInventoryManager.rightHandWeaponIndex = firstWeaponPosition;
                player.playerNetworkManager.currentRightHandWeaponID.Value = firstWeapon.itemID;
            }
            return;
        }

        foreach (WeaponItems weapon in player.playerInventoryManager.weaponsInRightHandSlots)
        {
            // IF THIS WEAPON IS NOT EQUAL TO THE UNARMED WEAPON
            if (player.playerInventoryManager.weaponsInRightHandSlots[player.playerInventoryManager.rightHandWeaponIndex].itemID != WorldItemDatabase.instance.unarmedWeapon.itemID)
            {
                selectedWeapon = player.playerInventoryManager.weaponsInRightHandSlots[player.playerInventoryManager.rightHandWeaponIndex];
                player.playerNetworkManager.currentRightHandWeaponID.Value = player.playerInventoryManager.weaponsInRightHandSlots[player.playerInventoryManager.rightHandWeaponIndex].itemID;
                // Play draw weapon sound
                DecideDrawWeaponSound(selectedWeapon);
                return;
            }
        }

        if (selectedWeapon == null && player.playerInventoryManager.rightHandWeaponIndex <= 2)
        {
            SwitchMainHandWeapon();
        }
    }
    */

    public void SwitchMainHandWeapon()
    {
        if (!player.IsOwner) return;

        // PLAY SWAPPING ANIMATION                               AnimationName ,isPerformingAction, applyRootMotion, canRotate, canMove
        player.playerAnimatorManager.PlayerTargetActionAnimation("Swap_Weapon_Right", false, false, true, true);

        WeaponItems selectedWeapon = null;

        if (player.playerInventoryManager.mainHandWeaponIndex < player.playerInventoryManager.weaponsInMainHandSlots.Length-1)
        {
            // ADD ONE TO OUR INDEX TO SWITCH TO THE NEXT WEAPON
            player.playerInventoryManager.mainHandWeaponIndex += 1;
        }

        else if (player.playerInventoryManager.mainHandWeaponIndex == player.playerInventoryManager.weaponsInMainHandSlots.Length-1)
        {
            // SUB ONE TO OUR INDEX TO SWITCH TO THE NEXT WEAPON
            player.playerInventoryManager.mainHandWeaponIndex = 0;
        }

        selectedWeapon = player.playerInventoryManager.weaponsInMainHandSlots[player.playerInventoryManager.mainHandWeaponIndex];
        player.playerNetworkManager.currentMainHandWeaponID.Value = player.playerInventoryManager.weaponsInMainHandSlots[player.playerInventoryManager.mainHandWeaponIndex].itemID;
        DecideDrawWeaponSound(selectedWeapon);

        // UI
        PlayerUIManager.instance.playerUIPopUpManager.SendWeaponDescriptionPopUp(selectedWeapon.name, selectedWeapon.itemDescription);
        if (selectedWeapon.weaponType == WeaponType.Staff || selectedWeapon.weaponType == WeaponType.Wand) // Toggles crosshair on/off
        {
            PlayerUIManager.instance.playerUIHudManager.ToggleCrosshairOn();
        }
        else
        {
            //spellDrawingCanvas.CloseSpellDrawingMenu();
            PlayerUIManager.instance.playerUIHudManager.ToggleCrosshairOff();
        }

        return;
    }

    // LEFT WEAPON
    public void LoadOffHandWeapon()
    {
        if (player.playerInventoryManager.currentOffHandWeapon != null)
        {
            // REMOVE OLD WEAPON
            offHandSlot.UnloadWeapon();

            // BRING IN NEW WEAPON
            offHandWeaponModel = Instantiate(player.playerInventoryManager.currentOffHandWeapon.weaponModel);
            offHandSlot.LoadWeapon(offHandWeaponModel);
            // ASSIGN WEAPONS DAMAGE, TO ITS COLLIDER
            offHandWeaponManager = offHandWeaponModel.GetComponent<WeaponManager>();
            offHandWeaponManager.SetWeaponDamage(player, player.playerInventoryManager.currentOffHandWeapon);
            // Play draw weapon sound
            DecideDrawWeaponSound(player.playerInventoryManager.currentOffHandWeapon);
        }
    }
    /*
    public void SwitchLeftWeapon_old()
    {
        if (!player.IsOwner) return;

        // PLAY SWAPPING ANIMATION                               AnimationName ,isPerformingAction, applyRootMotion, canRotate, canMove
        player.playerAnimatorManager.PlayerTargetActionAnimation("Swap_Weapon_Right", false, false, true, true);

        WeaponItems selectedWeapon = null;
        // TOGO https://youtu.be/xrw_yOGp9Jo?si=t52MywHL2l_Lq-Np&t=644
        // From this timestamp he explains Elden Ring/Dark Souls weapon system, which is the initial implementation, might be changed later on.

        // ADD ONE TO OUR INDEX TO SWITCH TO THE NEXT POTENTIAL WEAPON
        player.playerInventoryManager.leftHandWeaponIndex += 1;

        // IF OUR INDEX IS OUT OF BOUNDS, RESET IT TO POSITION #1 (0)
        if (player.playerInventoryManager.leftHandWeaponIndex < 0 || player.playerInventoryManager.leftHandWeaponIndex > 2)
        {
            player.playerInventoryManager.leftHandWeaponIndex = 0;

            // WE CHECK IF WE ARE HOLDING, MORE THAN ONE WEAPON
            float weaponCount = 0;
            WeaponItems firstWeapon = null;
            int firstWeaponPosition = 0;

            for (int i = 0; i < player.playerInventoryManager.weaponsInLeftHandSlots.Length; i++)
            {
                if (player.playerInventoryManager.weaponsInLeftHandSlots[i].itemID != WorldItemDatabase.instance.unarmedWeapon.itemID)
                {
                    weaponCount += 1;

                    if (firstWeapon == null)
                    {
                        firstWeapon = player.playerInventoryManager.weaponsInLeftHandSlots[i];
                        firstWeaponPosition = i;
                    }
                }
            }

            if (weaponCount <= 1)
            {
                player.playerInventoryManager.leftHandWeaponIndex = -1;
                selectedWeapon = WorldItemDatabase.instance.unarmedWeapon;
                player.playerNetworkManager.currentLeftHandWeaponID.Value = selectedWeapon.itemID;
            }
            else
            {
                player.playerInventoryManager.leftHandWeaponIndex = firstWeaponPosition;
                player.playerNetworkManager.currentLeftHandWeaponID.Value = firstWeapon.itemID;
            }
            return;
        }

        foreach (WeaponItems weapon in player.playerInventoryManager.weaponsInLeftHandSlots)
        {
            // IF THIS WEAPON IS NOT EQUAL TO THE UNARMED WEAPON
            if (player.playerInventoryManager.weaponsInLeftHandSlots[player.playerInventoryManager.leftHandWeaponIndex].itemID != WorldItemDatabase.instance.unarmedWeapon.itemID)
            {
                selectedWeapon = player.playerInventoryManager.weaponsInLeftHandSlots[player.playerInventoryManager.leftHandWeaponIndex];
                player.playerNetworkManager.currentLeftHandWeaponID.Value = player.playerInventoryManager.weaponsInLeftHandSlots[player.playerInventoryManager.leftHandWeaponIndex].itemID;
                // Play draw weapon sound
                DecideDrawWeaponSound(selectedWeapon);
                return;
            }
        }

        if (selectedWeapon == null && player.playerInventoryManager.leftHandWeaponIndex <= 2)
        {
            SwitchOffHandWeapon();
        }

        
    }
    */

    public void SwitchOffHandWeapon()
    {
        if (!player.IsOwner) return;

        // PLAY SWAPPING ANIMATION                               AnimationName ,isPerformingAction, applyRootMotion, canRotate, canMove
        player.playerAnimatorManager.PlayerTargetActionAnimation("Swap_Weapon_Right", false, false, true, true);

        WeaponItems selectedWeapon = null;

        if (player.playerInventoryManager.offHandWeaponIndex < player.playerInventoryManager.weaponsInOffHandSlots.Length-1)
        {
            // ADD ONE TO OUR INDEX TO SWITCH TO THE NEXT WEAPON
            player.playerInventoryManager.offHandWeaponIndex += 1;
        }

        else if (player.playerInventoryManager.offHandWeaponIndex == player.playerInventoryManager.weaponsInOffHandSlots.Length-1)
        {
            // SUB ONE TO OUR INDEX TO SWITCH TO THE NEXT WEAPON
            player.playerInventoryManager.offHandWeaponIndex = 0;
        }

        selectedWeapon = player.playerInventoryManager.weaponsInOffHandSlots[player.playerInventoryManager.offHandWeaponIndex];
        player.playerNetworkManager.currentOffHandWeaponID.Value = player.playerInventoryManager.weaponsInOffHandSlots[player.playerInventoryManager.offHandWeaponIndex].itemID;
        DecideDrawWeaponSound(selectedWeapon);
        PlayerUIManager.instance.playerUIPopUpManager.SendWeaponDescriptionPopUp(selectedWeapon.name, selectedWeapon.itemDescription);
        return;
    }

    private void DecideDrawWeaponSound(WeaponItems selectedWeapon)
    {
        if (playSoundCheck > 2)
        {
            // Deciding draw weapon sound based on the weapon type.
            if (selectedWeapon.weaponType == WeaponType.OneHandedSword ||
                selectedWeapon.weaponType == WeaponType.TwoHandedSword ||
                selectedWeapon.weaponType == WeaponType.OneHandedAxe ||
                selectedWeapon.weaponType == WeaponType.TwoHandedAxe ||
                selectedWeapon.weaponType == WeaponType.OneHandedMace ||
                selectedWeapon.weaponType == WeaponType.TwoHandedMace)
            {
                player.playerSoundFXManager.PlayDrawSwordSFX();
            }
            else if (selectedWeapon.weaponType == WeaponType.Staff || selectedWeapon.weaponType == WeaponType.Wand)
            {
                player.playerSoundFXManager.PlayDrawStaffSFX();
            }
            else if (selectedWeapon.weaponType == WeaponType.Unarmed)
            {
                player.playerSoundFXManager.PlaySheathSwordSFX();
            }
        }
        
        if (playSoundCheck <= 2) // Again quick fix for preventing draw unarmed weapon sound on spawn.
        {
            playSoundCheck += 1;
        }
    }

    // DAMAGE COLLIDERS
    public void OpenDamageCollider()
    {
        // OPEN RIGHT WEAPON DAMAGE COLLIDER
        if (player.playerNetworkManager.isUsingMainHand.Value)
        {
            mainHandWeaponManager.meleeDamageCollider.EnableDamageCollider();
            player.characterSoundFXManager.PlaySoundFX(WorldSoundFXManager.instance.ChooseRandomSFXFromArray(player.playerInventoryManager.currentMainHandWeapon.whooshes));
        }
        // OPEN LEFT WEAPON DAMAGE COLLIDER
        else if (player.playerNetworkManager.isUsingOffHand.Value)
        {
            offHandWeaponManager.meleeDamageCollider.EnableDamageCollider();
            player.characterSoundFXManager.PlaySoundFX(WorldSoundFXManager.instance.ChooseRandomSFXFromArray(player.playerInventoryManager.currentOffHandWeapon.whooshes));
        }
    }

    public void CloseDamageCollider()
    {
        // CLOSE RIGHT WEAPON DAMAGE COLLIDER
        if (player.playerNetworkManager.isUsingMainHand.Value)
        {
            mainHandWeaponManager.meleeDamageCollider.DisableDamageCollider();
        }
        // CLOSE LEFT WEAPON DAMAGE COLLIDER
        else if (player.playerNetworkManager.isUsingOffHand.Value)
        {
            offHandWeaponManager.meleeDamageCollider.DisableDamageCollider();
        }
    }


}
