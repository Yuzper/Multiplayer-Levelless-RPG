using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enums : MonoBehaviour
{
    
}

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
    Sword,
    Dagger,
    Bow,
    Staff
}

// THIS IS USED TO CALCULATE DAMAGE BASED ON ATTACK TYPE
public enum AttackType
{
    LightAttack01,
    LightAttack02,
    HeavyAttack01,
    ChargedAttack01,
    ChargedAttack02
}