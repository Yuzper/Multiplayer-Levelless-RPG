using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enums : MonoBehaviour
{
    
}

                                                ////////////////////////
                                                /// NOTE ON ENUMS!!! ///
                                                ////////////////////////

// If we add in the middle of enum, we most likely have to check all references to that enum that its the correct field
// since adding in the middle of enum will serialize the fields differently now.
// Think of enum as it references indexes in the enum. So if we add a new index 2 all higher indexes will also go one up.

public enum CharacterSlot
{
    CharacterSlot_01,
    CharacterSlot_02,
    CharacterSlot_03,
    CharacterSlot_04,
    CharacterSlot_05,
    CharacterSlot_06,
    CharacterSlot_07,
    CharacterSlot_08,
    CharacterSlot_09,
    CharacterSlot_10,
    NO_SLOT

}

//TODO SETS; might want to extend the amount of charactergroups
// for example if we want enemies to be able to attack enemies
// or if players can attack players
// or if some enemies can attack some enemies... etc.
public enum CharacterGroup
{
    Team01, // is friendly (assumed to be player in general)
    Team02, // enemy
}
public enum WeaponModelSlot
{
    RightHand,
    LeftHand,
    // Right Hips
    // Left Hips
}

public enum WeaponType
{
    Unarmed,
    FistWeapon,
    OneHandedSword,
    TwoHandedSword,
    OneHandedAxe,
    TwoHandedAxe,
    OneHandedMace,
    TwoHandedMace,
    Polearm,
    Dagger,
    Bow,
    Crossbow,
    Wand,
    Staff,
    ThrowingProjectile,
    Misc
}

// THIS IS USED TO CALCULATE DAMAGE BASED ON ATTACK TYPE
public enum AttackType
{ // WHEN REORDING ENUMS ALL VALUES ON INDEXES HIGHER THAN THE ONE CHANGED WILL BE OFFSET!!!
    UnarmedMeleeAttack,
    LightAttack01,
    LightAttack02,
    LightAttack03,
    HeavyAttack01,
    HeavyAttack02,
    ChargedAttack01,
    ChargedAttack02
}