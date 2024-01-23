using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponItems : Item
{
    [Header("Weapon Model")]
    public GameObject weaponModel;

    [Header("Weapon Type")]
    public WeaponType weaponType;

    [Header("Weapon Requirements")]
    public int strengthREQ = 0;
    public int dexREQ = 0;
    public int intREQ = 0;

    [Header("Weapon Base Damage")]
    public int physicalDamage = 0;
    public int magicDamage = 0;
    public int fireDamage = 0;
    public int holyDamage = 0;
    public int lightningDamage = 0;

    [Header("Weapon Base Poise Damage")]
    public float poiseDamage = 10;

    [Header("Attack Modifiers")]
    public float light_Attack_01_Modifier = 1.0f;
    public float light_Attack_02_Modifier = 1.5f;
    public float heavy_Attack_01_Modifier = 2.0f;
    public float heavy_Attack_02_Modifier = 2.5f;
    public float charge_Attack_01_Modifier = 3.0f;
    public float charge_Attack_02_Modifier = 3.0f;

    [Header("Stamina Costs")]
    public int baseStaminaCost = 20;
    public float lightAttackStaminaCostMultiplier = 1.0f; // Adjusts the cost to perform this attack from the base cost
    public float heavyAttackStaminaCostMultiplier = 1.5f; // Adjusts the cost to perform this attack from the base cost

    [Header("Actions")]
    public WeaponItemAction oneHandRightMouseAttack;
    public WeaponItemAction oneHandLeftMouseAttack;

    public WeaponItemAction oneHandHeavyRightMouseAttack;
    public WeaponItemAction oneHandHeavyLeftMouseAttack;

    public WeaponItemAction JumpAttack;


}
