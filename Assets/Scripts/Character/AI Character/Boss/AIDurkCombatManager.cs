using System.Collections;
using System.Collections.Generic;
using UnityEngine;


    public class AIDurkCombatManager : AICharacterCombatManager
    {
        AIDurkCharacterManager durkManager;

        [Header("Damage Collider")]
    [SerializeField] DurkClubDamageCollider clubDamageCollider;
    [SerializeField] DurksStompCollider stompCollider;
        public float stompAttackAOERadius = 1.5f;

        [Header("Damage")]
        [SerializeField] int baseDamage = 25;
        [SerializeField] float attack01DamageModifier = 1.0f;
        [SerializeField] float attack02DamageModifier = 1.4f;
        [SerializeField] float attack03DamageModifier = 1.6f;
        public float stompDamage = 25;

        [Header("VFX")]
        public GameObject durkImpactVFX;

        protected override void Awake()
        {
            base.Awake();

            durkManager = GetComponent<AIDurkCharacterManager>();
        }

        public void SetAttack01Damage()
        {
            aiCharacter.characterSoundFXManager.PlayAttackGrunt();
            clubDamageCollider.physicalDamage = baseDamage * attack01DamageModifier;
        }

        public void SetAttack02Damage()
        {
            aiCharacter.characterSoundFXManager.PlayAttackGrunt();
            clubDamageCollider.physicalDamage = baseDamage * attack02DamageModifier;
        }

        public void SetAttack03Damage()
        {
            aiCharacter.characterSoundFXManager.PlayAttackGrunt();
            clubDamageCollider.physicalDamage = baseDamage * attack03DamageModifier;
        }

        public void OpenClubDamageCollider()
        {          
            clubDamageCollider.EnableDamageCollider();
            durkManager.characterSoundFXManager.PlaySoundFX(WorldSoundFXManager.instance.ChooseRandomSFXFromArray(durkManager.durkSoundFXManager.clubWhooshes));
        }

        public void CloseClubDamageCollider()
        {
            clubDamageCollider.DisableDamageCollider();
        }

        public void ActivateDurkStomp()
        {
            stompCollider.StompAttack();
            durkManager.durkSoundFXManager.PlayStompImpactSoundFX();
        }

        public override void PivotTowardsTarget(AICharacterManager aiCharacter)
        {
            //  PLAY A PIVOT ANIMATION DEPENDING ON VIEWABLE ANGLE OF TARGET
            if (aiCharacter.isPerformingAction)
                return;

            if (viewableAngle >= 61 && viewableAngle <= 110)
            {
                aiCharacter.characterAnimatorManager.PlayerTargetActionAnimation("Turn_R_90", true);
            }
            else if (viewableAngle <= -61 && viewableAngle >= -110)
            {
                aiCharacter.characterAnimatorManager.PlayerTargetActionAnimation("Turn_L_90", true);
            }
            else if (viewableAngle >= 146 && viewableAngle <= 180)
            {
                aiCharacter.characterAnimatorManager.PlayerTargetActionAnimation("Turn_R_180", true);
            }
            else if (viewableAngle <= -146 && viewableAngle >= -180)
            {
                aiCharacter.characterAnimatorManager.PlayerTargetActionAnimation("Turn_L_180", true);
            }
        }
    
}