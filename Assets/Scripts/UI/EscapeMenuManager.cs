using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Netcode;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class EscapeMenuManager : MonoBehaviour
{
    public static EscapeMenuManager instance;

    [Header("Menus")]
    [SerializeField] public GameObject escapeMenu;

    [Header("Buttons")]
    [SerializeField] Button ContinueGameButton;
    [SerializeField] Button ConnectGameButton;
    [SerializeField] Button EscapeMenuSaveGameButton;
    [SerializeField] Button SettingsButton;
    [SerializeField] Button ReturnToMainMenuButton;
    

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

    private void Start()
    {
        DontDestroyOnLoad(gameObject);
    }


    public void StartNetworkAsHost()
    {
        NetworkManager.Singleton.StartHost();
    }

    public void ConnectAsClient()
    {
        PlayerUIManager.instance.SetStartGameAsClient();
    }

    public void DecideOpenOrCloseEscapeMenu()
    {
        // If the menu is open then the escape menu input should close the menu and vise versa.
        if (escapeMenu.activeSelf)
        {
            CloseEscapeMenu();
        }
        else
        {
            OpenEscapeMenu();
        }
    }

    public void OpenEscapeMenu()
    {
        // OPEN ESCAPE MENU
        escapeMenu.SetActive(true);

        // SELECT THE CONTINUE BUTTON FIRST
        ContinueGameButton.Select();
    }

    public void CloseEscapeMenu()
    {
        // CLOSE ESCAPE MENU
        escapeMenu.SetActive(false);
    }


    public void SaveGameEscapeMenu()
    {
        WorldSaveGameManager.instance.SaveGame();
    }

    public void ReturnToMainMenu()
    {
        CloseEscapeMenu();
        NetworkManager.Singleton.Shutdown();
        AsyncOperation asyncOperation = SceneManager.LoadSceneAsync(0); // Index 0 is Main Menu
    }
    
}
