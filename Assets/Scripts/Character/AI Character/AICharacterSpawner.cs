using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Netcode;

public class AICharacterSpawner : MonoBehaviour
{
    [Header("Character")]
    [SerializeField] GameObject characterGameObject;
    [SerializeField] GameObject instantiatedGameObject;

    private void Awake()
    {
        
    }

    private void Start()
    {
        WorldAIManager.instance.SpawnCharacter(this);
        gameObject.SetActive(false);
    }

    public void AttemptToSpawnCharacter()
    {
        if (characterGameObject != null)
        {
            instantiatedGameObject = Instantiate(characterGameObject, transform.position, transform.rotation);
            instantiatedGameObject.GetComponent<NetworkObject>().Spawn();
        }
    }

}
