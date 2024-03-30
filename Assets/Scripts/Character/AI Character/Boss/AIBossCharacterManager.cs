using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Netcode;

public class AIBossCharacterManager : AICharacterManager
{
    public int bossID = 0;

    [Header("Status")]
    public NetworkVariable<bool> bossFightIsActive = new NetworkVariable<bool>(false, NetworkVariableReadPermission.Everyone, NetworkVariableWritePermission.Owner);
    public NetworkVariable<bool> hasBeenDefeated = new NetworkVariable<bool>(false, NetworkVariableReadPermission.Everyone, NetworkVariableWritePermission.Owner);
    public NetworkVariable<bool> hasBeenAwakended = new NetworkVariable<bool>(false, NetworkVariableReadPermission.Everyone, NetworkVariableWritePermission.Owner);
    [SerializeField] string sleepAnimation;
    [SerializeField] string awakenAnimation;

    [Header("States")]
    [SerializeField] BossSleepState sleepState;

    protected override void Awake()
    {
        base.Awake();
    }

    protected override void Update()
    {
        base.Update();
    }


    public override void OnNetworkSpawn()
    {
        base.OnNetworkSpawn();

        bossFightIsActive.OnValueChanged += OnBossFightIsActiveChanged;
        OnBossFightIsActiveChanged(false,bossFightIsActive.Value); // so if you join when the fight is already active, you will get the hp bar

        if (IsOwner)
        {
            sleepState = Instantiate(sleepState);
            currentState = sleepState;
        }


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
                hasBeenDefeated.Value = WorldSaveGameManager.instance.currentCharacterData.bossesDefeated[bossID];
                hasBeenAwakended.Value = WorldSaveGameManager.instance.currentCharacterData.bossesAwakened[bossID];

                // boss has been defeated (was used to enable disable fog walls)
                if (hasBeenDefeated.Value)
                {
                    aiCharacterNetworkManager.isActive.Value = false;
                }

                // boss has been awakend (was used to enable disable fog walls)
                if (hasBeenAwakended.Value)
                {

                }
            }
        }

        if (!hasBeenAwakended.Value)
        {
            characterAnimatorManager.PlayerTargetActionAnimation(sleepAnimation, true);
        }
    }

    public override void OnNetworkDespawn()
    {
        base.OnNetworkDespawn();

        bossFightIsActive.OnValueChanged -= OnBossFightIsActiveChanged;
    }

    public override IEnumerator ProcessDeathEvent(bool manuallySelectDeathAnimation = false)
    {
        if (IsOwner)
        {
            //characterNetworkManager.currentHealth.Value = 0;
            isDead.Value = true;
            characterLocomotionManager.canMove = false;
            characterLocomotionManager.canRotate = false;

            bossFightIsActive.Value = false;

            // Reset any flags here that need to be reset

            if (!manuallySelectDeathAnimation)
            {
                characterAnimatorManager.PlayerTargetActionAnimation("Dead_01", true);
            }

            // SAVING DATA FOR IF THE BOSS IS DEFEATED
            hasBeenDefeated.Value = true;

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

        if (IsOwner)
        {

            if (!hasBeenAwakended.Value)
            {

                characterAnimatorManager.PlayerTargetActionAnimation(awakenAnimation, true);
            }
            bossFightIsActive.Value= true;
            hasBeenAwakended.Value = true;

            currentState = idle;

            if(!WorldSaveGameManager.instance.currentCharacterData.bossesAwakened.ContainsKey(bossID))
            {
                WorldSaveGameManager.instance.currentCharacterData.bossesAwakened.Add(bossID, true );
            } else
            {
                WorldSaveGameManager.instance.currentCharacterData.bossesAwakened.Remove(bossID );
                WorldSaveGameManager.instance.currentCharacterData.bossesAwakened.Add(bossID , true);
            }


        }

    }



    private void OnBossFightIsActiveChanged(bool oldValue, bool newValue)
    {
        // create hp bar for each boss that is in the fight ( if its active)
        // destory any hp bars currently active ( if the boss is no longer active )

        if(bossFightIsActive.Value)
        {
            GameObject bossHealthBar
                = Instantiate(PlayerUIManager.instance.playerUIHudManager.bossHealthBarObject, PlayerUIManager.instance.playerUIHudManager.bossHealthBarParent);

            UI_Boss_HP_Bar bossHPBar = bossHealthBar.GetComponentInChildren<UI_Boss_HP_Bar>();
            bossHPBar.EnableBossHPBar(this);
        }

    }


}
