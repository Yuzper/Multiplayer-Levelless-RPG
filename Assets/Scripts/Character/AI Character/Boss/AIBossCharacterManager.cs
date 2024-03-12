using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Netcode;

public class AIBossCharacterManager : AICharacterManager
{
    public int bossID = 0;
    [SerializeField] bool hasBeenDefeated = false;
    [SerializeField] bool hasBeenAwakended = false;

    [Header("DEBUG")]
    [SerializeField] bool wakeBossUp = false;

    protected override void Update()
    {
        base.Update();

        if (wakeBossUp)
        {
            wakeBossUp = false;
            WakeBoss();
        }
    }


    public override void OnNetworkSpawn()
    {
        base.OnNetworkSpawn();

        if (IsServer)
        {
            // IF OUR SAVE DATA DOES NOT CONTAIN INFORMATION ON THIS BOSS, ADD IT NOW
            if (!WorldSaveGameManager.instance.currentCharacterData.bossesAwakened.ContainsKey(bossID))
            {
                WorldSaveGameManager.instance.currentCharacterData.bossesAwakened.Add(bossID, false);
                WorldSaveGameManager.instance.currentCharacterData.bossesDefeated.Add(bossID, false);
            }
            // OTHERWISE, LOAD THE DATA THAT ALREADY EXISTS ON THIS BOSS
            else
            {
                hasBeenDefeated = WorldSaveGameManager.instance.currentCharacterData.bossesDefeated[bossID];

                if (hasBeenDefeated)
                {
                    aiCharacterNetworkManager.isActive.Value = false;
                }

                if (hasBeenAwakended)
                {

                }
            }
        }
    }


    public override IEnumerator ProcessDeathEvent(bool manuallySelectDeathAnimation = false)
    {
        if (IsOwner)
        {
            //characterNetworkManager.currentHealth.Value = 0;
            isDead.Value = true;
            characterLocomotionManager.canMove = false;
            characterLocomotionManager.canRotate = false;

            // Reset any flags here that need to be reset

            if (!manuallySelectDeathAnimation)
            {
                characterAnimatorManager.PlayerTargetActionAnimation("Dead_01", true);
            }

            // SAVING DATA FOR IF THE BOSS IS DEFEATED
            hasBeenDefeated = true;

            if (!WorldSaveGameManager.instance.currentCharacterData.bossesAwakened.ContainsKey(bossID))
            {
                WorldSaveGameManager.instance.currentCharacterData.bossesAwakened.Add(bossID, true);
                WorldSaveGameManager.instance.currentCharacterData.bossesDefeated.Add(bossID, true);
            }
            // OTHERWISE, LOAD THE DATA THAT ALREADY EXISTS ON THIS BOSS
            else
            {
                WorldSaveGameManager.instance.currentCharacterData.bossesAwakened.Remove(bossID);
                WorldSaveGameManager.instance.currentCharacterData.bossesDefeated.Remove(bossID);
                WorldSaveGameManager.instance.currentCharacterData.bossesAwakened.Add(bossID, true);
                WorldSaveGameManager.instance.currentCharacterData.bossesDefeated.Add(bossID, true);
            }

            WorldSaveGameManager.instance.SaveGame();
        }
        // Play death SFX

        yield return new WaitForSeconds(4);

        // Award players with loot
        // Disable Character model

        // Disable control over player movement
    }

    public void WakeBoss()
    {
        hasBeenAwakended = true;
    }

}
