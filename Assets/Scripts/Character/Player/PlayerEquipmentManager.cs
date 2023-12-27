using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerEquipmentManager : CharacterEquipmentManager
{
    PlayerManager player;
    public WeaponModelInstantiationSlot rightHandSlot;
    public WeaponModelInstantiationSlot leftHandSlot;

    [SerializeField] WeaponManager rightWeaponManager;
    [SerializeField] WeaponManager leftWeaponManager;

    public GameObject rightHandWeaponModel;
    public GameObject leftHandWeaponModel;

    protected override void Awake()
    {
        base.Awake();
        player = GetComponent<PlayerManager>();
        IntializeWeaponSlots();
    }

    protected override void Start()
    {
        base.Start();
        LoadWeaponOnBothHands();
    }

    private void IntializeWeaponSlots()
    {
        WeaponModelInstantiationSlot[] weaponSlots = GetComponentsInChildren<WeaponModelInstantiationSlot>();
        
        foreach (var weaponSlot in weaponSlots)
        {
            if (weaponSlot.weaponSlot == WeaponModelSlot.RightHand)
            {
                rightHandSlot = weaponSlot;
            }
            else if (weaponSlot.weaponSlot == WeaponModelSlot.LeftHand)
            {
                leftHandSlot = weaponSlot;
            }
        }
    }

    public void LoadWeaponOnBothHands()
    {
        LoadRightWeapon();
        LoadLeftWeapon();
    }

    // RIGHT WEAPON
    public void LoadRightWeapon()
    {
        if (player.playerInventoryManager.currentRightHandWeapon != null)
        {
            // REMOVE OLD WEAPON
            rightHandSlot.UnloadWeapon();

            // BRING IN NEW WEAPON
            rightHandWeaponModel = Instantiate(player.playerInventoryManager.currentRightHandWeapon.weaponModel);
            rightHandSlot.LoadWeapon(rightHandWeaponModel);
            // ASSIGN WEAPONS DAMAGE, TO ITS COLLIDER
            rightWeaponManager = rightHandWeaponModel.GetComponent<WeaponManager>();
            rightWeaponManager.SetWeaponDamge(player, player.playerInventoryManager.currentRightHandWeapon);
        }
    }

    public void SwitchRightWeapon()
    {
        if (!player.IsOwner) return;

        // PLAY SWAPPING ANIMATION                               AnimationName ,isPerformingAction, applyRootMotion, canRotate, canMove
        player.playerAnimatorManager.PlayerTargetActionAnimation("Swap_Weapon_Right", false, true, true, true);

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
                return;
            }
        }

        if (selectedWeapon == null && player.playerInventoryManager.rightHandWeaponIndex <= 2)
        {
            SwitchRightWeapon();
        }
    }

    // LEFT WEAPON
    public void LoadLeftWeapon()
    {
        if (player.playerInventoryManager.currentLeftHandWeapon != null)
        {
            leftHandWeaponModel = Instantiate(player.playerInventoryManager.currentLeftHandWeapon.weaponModel);
            leftHandSlot.LoadWeapon(leftHandWeaponModel);
            // ASSIGN WEAPONS DAMAGE, TO ITS COLLIDER
            leftWeaponManager = leftHandWeaponModel.GetComponent<WeaponManager>();
            leftWeaponManager.SetWeaponDamge(player, player.playerInventoryManager.currentLeftHandWeapon);
        }
    }

    public void SwitchLeftWeapon()
    {

    }
}
