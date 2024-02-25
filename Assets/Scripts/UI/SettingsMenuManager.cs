using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Netcode;
using UnityEngine.UI;

public class SettingsMenuManager : MonoBehaviour
{
    public static SettingsMenuManager instance;

    [Header("Menus")]
    [SerializeField] public GameObject settingsMenu;

    [Header("Buttons")]
    [SerializeField] Button BackToEscapeMenuButton;
    [SerializeField] Button Settings1;
    [SerializeField] Button Settings2;
    [SerializeField] Button Settings3;


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

    public void OpenSettingsMenu()
    {
        // OPEN ESCAPE MENU
        settingsMenu.SetActive(true);

        // SELECT THE BACK BUTTON FIRST
        BackToEscapeMenuButton.Select();
    }

    public void CloseSettingsMenu()
    {
        // CLOSE ESCAPE MENU
        settingsMenu.SetActive(false);
    }

}
