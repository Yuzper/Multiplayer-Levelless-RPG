using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Netcode;
using System.Linq;

public class WorldAIManager : MonoBehaviour
{
    public static WorldAIManager instance;

    [Header("Characters")]
    [SerializeField] List<AICharacterSpawner> aiCharacterSpawners;
    [SerializeField] List<AICharacterManager> spawnedCharacters;

    [Header("Bosses")]
    [SerializeField] List<AIBossCharacterManager> spawnedBossCharacters;

    private void Awake()
    {
        if(instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void SpawnCharacter(AICharacterSpawner aiCharacterSpawner)
    {
        if (NetworkManager.Singleton.IsServer)
        {
            aiCharacterSpawners.Add(aiCharacterSpawner);
            aiCharacterSpawner.AttemptToSpawnCharacter();
        }
    }

    public void AddCharacterToSpawnedCharactersList(AICharacterManager character)
    {
        if (spawnedCharacters.Contains(character))
        {
            return;
        }

        spawnedCharacters.Add(character);

        AIBossCharacterManager bossCharacter = character as AIBossCharacterManager;

        if(bossCharacter != null)
        {
            if (spawnedBossCharacters.Contains(bossCharacter))
            {
                return;
            }
            spawnedBossCharacters.Add(bossCharacter);
        }
    }

    public AIBossCharacterManager GetBossCharacterByID(int ID)
    {
        return spawnedBossCharacters.FirstOrDefault(boss => boss.bossID == ID);
    }

    void DespawnAllCharacters()
    {
        foreach (var character in spawnedCharacters)
        {
            if (character == null) return;

            character.GetComponent<NetworkObject>().Despawn();
        }

        spawnedCharacters.Clear();
    }

    //TODO 
    void DisableAllCharacters()
    {
        foreach (var character in spawnedCharacters)
        {
            if (character == null) return;
        }
    }

    public void ResetAllCharacters()
    {
        DespawnAllCharacters();

        foreach (var spawner in aiCharacterSpawners)
        {
            spawner.AttemptToSpawnCharacter();
        }
    }
}
