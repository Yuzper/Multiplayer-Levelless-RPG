using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Netcode;

public class PlayerUIManager : MonoBehaviour
{
    public static PlayerUIManager instance;

    [Header("NETWORK JOIN")]
    [SerializeField] bool startGameAsClient;

    [HideInInspector] public PlayerUIHudManager playerUIHudManager;

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

        playerUIHudManager = GetComponentInChildren<PlayerUIHudManager>();
    }

    private void Start()
    {
        DontDestroyOnLoad(gameObject);
    }

    // Update is called once per frame
    void Update()
    {
        if (startGameAsClient)
        {
            startGameAsClient = false;
            // WE MUST FIRST SHUT DOWN, BECAUSE WE HAVE STARTED A HOST DURING THE TITLE SCREEN
            NetworkManager.Singleton.Shutdown();
            // WE THEN RESTART, AS A CLIENT
            NetworkManager.Singleton.StartClient();
        }
    }
}
