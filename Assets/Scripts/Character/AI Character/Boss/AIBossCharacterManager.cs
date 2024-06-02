using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Netcode;

public class AIBossCharacterManager : AICharacterManager
{
    public int bossID = 0;

    [Header("Music")]
    [SerializeField] AudioClip bossIntroClip;
    [SerializeField] AudioClip bossBattleLoopClip;

    [Header("Status")]
    public NetworkVariable<bool> bossFightIsActive = new NetworkVariable<bool>(false, NetworkVariableReadPermission.Everyone, NetworkVariableWritePermission.Owner);
    public NetworkVariable<bool> hasBeenDefeated = new NetworkVariable<bool>(false, NetworkVariableReadPermission.Everyone, NetworkVariableWritePermission.Owner);
    public NetworkVariable<bool> hasBeenAwakended = new NetworkVariable<bool>(false, NetworkVariableReadPermission.Everyone, NetworkVariableWritePermission.Owner);
    [SerializeField] string sleepAnimation;
    [SerializeField] string awakenAnimation;

    [Header("Phase Shift")]
    public float minimumHealthPercentageToShift = 50;
    [SerializeField] string phaseShiftAnimation = "Phase_Change_01";
    [SerializeField] CombatStanceState phase02CombatStanceState;

    [Header("States")]
    [SerializeField] BossSleepState sleepState;

    //  WHEN THIS A.I IS SPAWNED, CHECK OUR SAVE FILE (DICTIONARY)
    //  IF THE SAVE FILE DOES NOT CONTAIN A BOSS MONSTER WITH THIS I.D ADD IT
    //  IF IT IS PRESENT, CHECK IF THE BOSS HAS BEEN DEFEATED
    //  IF THE BOSS HAS BEEN DEFEATED, DISABLE THIS GAMEOBJECT
    //  IF THE BOSS HAS NOT BEEN DEFEATED, ALLOW THIS OBJECT TO CONTINUE TO BE ACTIVE

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
            //  IF OUR SAVE DATA DOES NOT CONTAIN INFORMATION ON THIS BOSS, ADD IT NOW
            if (!WorldSaveGameManager.instance.currentCharacterData.bossesAwakened.ContainsKey(bossID))
            {
                WorldSaveGameManager.instance.currentCharacterData.bossesAwakened.Add(bossID, false);
                WorldSaveGameManager.instance.currentCharacterData.bossesDefeated.Add(bossID, false);
            }
            //  OTHERWISE, LOAD THE DATA THAT ALREADY EXISTS ON THIS BOSS
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

                    aiCharacterNetworkManager.isActive.Value = false;
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
        PlayerUIManager.instance.playerUIPopUpManager.SendBossDefeatedPopUp("GREAT FOE FELLED");
        if (IsOwner)
        {
            characterNetworkManager.currentHealth.Value = 0;
            isDead.Value = true;
            bossFightIsActive.Value = false;

            // Reset any flags here that need to be reset

            //  RESET ANY FLAGS HERE THAT NEED TO BE RESET
            //  NOTHING YET

            //  IF WE ARE NOT GROUNDED, PLAY AN AERIAL DEATH ANIMATION

            if (!manuallySelectDeathAnimation)
            {
                characterAnimatorManager.PlayerTargetActionAnimation("Dead_01", true);
            }

            hasBeenDefeated.Value = true;

            //  IF OUR SAVE DATA DOES NOT CONTAIN INFORMATION ON THIS BOSS, ADD IT NOW
            if (!WorldSaveGameManager.instance.currentCharacterData.bossesAwakened.ContainsKey(bossID))
            {
                WorldSaveGameManager.instance.currentCharacterData.bossesAwakened.Add(bossID, true);
                WorldSaveGameManager.instance.currentCharacterData.bossesDefeated.Add(bossID, true);
            }
            //  OTHERWISE, LOAD THE DATA THAT ALREADY EXISTS ON THIS BOSS
            else
            {
                WorldSaveGameManager.instance.currentCharacterData.bossesAwakened.Remove(bossID);
                WorldSaveGameManager.instance.currentCharacterData.bossesDefeated.Remove(bossID);
                WorldSaveGameManager.instance.currentCharacterData.bossesAwakened.Add(bossID, true);
                WorldSaveGameManager.instance.currentCharacterData.bossesDefeated.Add(bossID, true);
            }

            WorldSaveGameManager.instance.SaveGame();
        }

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
            WorldSoundFXManager.instance.PlayBossTrack(bossIntroClip, bossBattleLoopClip);
            GameObject bossHealthBar
                = Instantiate(PlayerUIManager.instance.playerUIHudManager.bossHealthBarObject, PlayerUIManager.instance.playerUIHudManager.bossHealthBarParent);

            UI_Boss_HP_Bar bossHPBar = bossHealthBar.GetComponentInChildren<UI_Boss_HP_Bar>();
            bossHPBar.EnableBossHPBar(this);
        }
        else
        {
            WorldSoundFXManager.instance.StopBossMusic();
        }
    }

    public void PhaseShift()
    {
        characterAnimatorManager.PlayerTargetActionAnimation(phaseShiftAnimation, true);
        combatStance = Instantiate(phase02CombatStanceState);
        currentState = combatStance;
    }
}