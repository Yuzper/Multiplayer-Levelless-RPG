using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponManager : MonoBehaviour
{
    public MeleeWeaponDamageCollider meleeDamageCollider;

    private void Awake()
    {
        meleeDamageCollider = GetComponentInChildren<MeleeWeaponDamageCollider>();
    }

    public void SetWeaponDamage(CharacterManager characterWieldingWeapon, WeaponItems weapon)
    {
        meleeDamageCollider.characterCausingDamage = characterWieldingWeapon;
        meleeDamageCollider.physicalDamage = weapon.physicalDamage;
        meleeDamageCollider.magicDamage = weapon.magicDamage;
        meleeDamageCollider.fireDamage = weapon.fireDamage;
        meleeDamageCollider.lightningDamage = weapon.lightningDamage;
        meleeDamageCollider.holyDamage = weapon.holyDamage;

        meleeDamageCollider.light_Attack_01_Modifier = weapon.light_Attack_01_Modifier;
        meleeDamageCollider.light_Attack_02_Modifier = weapon.light_Attack_02_Modifier;

        meleeDamageCollider.heavy_Attack_01_Modifier = weapon.heavy_Attack_01_Modifier;
        meleeDamageCollider.heavy_Attack_02_Modifier = weapon.heavy_Attack_02_Modifier;

        meleeDamageCollider.charge_Attack_01_Modifier = weapon.charge_Attack_01_Modifier;
        meleeDamageCollider.charge_Attack_02_Modifier = weapon.charge_Attack_02_Modifier;

        meleeDamageCollider.jump_Attack_Modifier = weapon.jump_Attack_Modifier;

    }

}
