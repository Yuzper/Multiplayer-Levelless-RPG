using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Netcode;

public class PlayerUIManager : MonoBehaviour
{
    public static PlayerUIManager instance;

    public PlayerManager player;

    [Header("NETWORK JOIN")]
    [SerializeField] bool startGameAsClient;

    [HideInInspector] public PlayerUIHudManager playerUIHudManager;
    [HideInInspector] public PlayerUIPopUpManager playerUIPopUpManager;

    [Header("UI Flags")]
    [Tooltip("Inventory screen etc.")]
    public bool menuWindowIsOpen = false;

    [Tooltip("Item pickups, dialogue pop up, etc.")]
    public bool popUpWindowIsOpen = false;

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
        playerUIPopUpManager = GetComponentInChildren<PlayerUIPopUpManager>();
    }

    public void SetStartGameAsClient()
    {
        startGameAsClient = true;
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
