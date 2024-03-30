using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.Collections.LowLevel.Unsafe;
using UnityEngine;

public class UI_Boss_HP_Bar : UI_StatBar
{
    [SerializeField] AIBossCharacterManager bossCharacter;
    public void EnableBossHPBar(AIBossCharacterManager boss)
    {
        bossCharacter = boss;
        bossCharacter.aiCharacterNetworkManager.currentHealth.OnValueChanged += OnBossHPChanged;
        SetMaxStat(bossCharacter.characterNetworkManager.maxHealth.Value);
        SetStat(bossCharacter.characterNetworkManager.currentHealth.Value);
        GetComponentInChildren<TextMeshProUGUI>().text = bossCharacter.characterName;
    }

    private void OnDestroy()
    {
        bossCharacter.aiCharacterNetworkManager.currentHealth.OnValueChanged -= OnBossHPChanged;
    }

    void OnBossHPChanged(float oldValue, float newValue)
    {
        SetStat(newValue);

        if(newValue <= 0)
        {
            RemoveHPBar(2.5f);
        }
    }

    public void RemoveHPBar(float timeDelay)
    {
        Destroy(gameObject, timeDelay);
    }
}
