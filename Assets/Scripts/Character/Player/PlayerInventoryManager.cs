using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInventoryManager : CharacterInventoryManager
{
    public WeaponItems currentMainHandWeapon;
    public WeaponItems currentOffHandWeapon;

    [Header("Quick Slots")]
    public WeaponItems[] weaponsInMainHandSlots = new WeaponItems[3];
    public int mainHandWeaponIndex = 0;

    public WeaponItems[] weaponsInOffHandSlots = new WeaponItems[3];
    public int offHandWeaponIndex = 0;


}
