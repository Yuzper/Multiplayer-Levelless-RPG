using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Netcode;

public class WorldObjectManager : MonoBehaviour
{
    public static WorldObjectManager instance;

    [Header("Objects")]
    [SerializeField] List<NetworkObjectSpawner> networkObjectSpawners;
    [SerializeField] List<GameObject> spawnedInObjects;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void SpawnObject(NetworkObjectSpawner networkObjectSpawner)
    {
        if (NetworkManager.Singleton.IsServer)
        {
            networkObjectSpawners.Add(networkObjectSpawner);
            networkObjectSpawner.AttemptToSpawnObject();
        }
    }

}
