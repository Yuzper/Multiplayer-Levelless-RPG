using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StompCollider : DamageCollider
{
    AIBossCharacterManager characterManager;
    AIBossCombatManager combatManager;

    private void Awake()
    {
        base.Awake();

        characterManager = GetComponentInParent<AIBossCharacterManager>();
        combatManager = characterManager.aICharacterCombatManager as AIBossCombatManager;
    }
    public void StompAttack()
    {
        GameObject stompVFX = Instantiate(combatManager.stompImpactVFX, transform.position, combatManager.stompImpactVFX.transform.rotation);
        Collider[] colliders = Physics.OverlapSphere(transform.position, combatManager.stompAttackAOERadious, WorldUtilityManager.instance.GetCharacterLayers());
        List<CharacterManager> charactersDamaged = new List<CharacterManager>();

        foreach (var collider in colliders)
        {
            CharacterManager character = collider.GetComponentInParent<CharacterManager>();

            if (character != null)
            {
                if (charactersDamaged.Contains(character) || character == characterManager) continue;

                charactersDamaged.Add(character);

                if(character.IsOwner)
                {
                    // check block

                    TakeDamageEffect damageEffect = Instantiate(WorldCharacterEffectsManager.instance.takeDamageEffect);
                    damageEffect.physicalDamage = combatManager.stompDamage;
                    damageEffect.poiseDamage = combatManager.stompDamage;

                    character.characterEffectsManager.ProcessInstantEffect(damageEffect);
                }
                
            }
        }
    }
}
