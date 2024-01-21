using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Netcode;
using UnityEngine.SceneManagement;
using Unity.VisualScripting;

public class WorldAIManager : MonoBehaviour
{
    public static WorldAIManager instance;

    [Header("DEBUG")]
    [SerializeField] bool despawnCharacters = false;
    [SerializeField] bool respawnCharacters = false;

    [Header("Characters")]
    [SerializeField] GameObject[] aiCharacters;
    [SerializeField] List<GameObject> spawnedCharacters;

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


    private void Start()
    {
        if(NetworkManager.Singleton.IsServer)
        {
            // spawn ALL A.I IN SCENE (will be refactored)
            StartCoroutine(WaitForSceneToLoadThenSpawnCharacters());
        }
    }

    private void Update()
    {
        if(respawnCharacters)
        {
            respawnCharacters = false;
            SpawnAllCharacters();
        }
        if(despawnCharacters)
        {
            despawnCharacters = false;
            DespawnAllCharacters();
        }

    }

    private IEnumerator WaitForSceneToLoadThenSpawnCharacters()
    {
        while (!SceneManager.GetActiveScene().isLoaded)
        {
            yield return null;
        }
        SpawnAllCharacters();
    }

    void SpawnAllCharacters()
    {
        foreach(var character in aiCharacters)
        {
            if (character == null) return;

            GameObject instantiatedCharacter = Instantiate(character);
            instantiatedCharacter.GetComponent<NetworkObject>().Spawn();
            spawnedCharacters.Add(instantiatedCharacter);
        }
    }

    void DespawnAllCharacters()
    {
        foreach (var character in spawnedCharacters)
        {
            if (character == null) return;

            character.GetComponent<NetworkObject>().Despawn();
        }
    }

    //TODO 
    void DisableAllCharacters()
    {
        foreach (var character in spawnedCharacters)
        {
            if (character == null) return;
        }
    }
}
