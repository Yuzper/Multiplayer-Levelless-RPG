using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpellDrawingManager : MonoBehaviour
{
    public static SpellDrawingManager instance;

    [Header("Menus")]
    [SerializeField] public GameObject spellDrawMenu;

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

    public void OpenSpellDrawingMenu()
    {
        spellDrawMenu.SetActive(true);
    }

    public void CloseSpellDrawingMenu()
    {
        spellDrawMenu.SetActive(false);
    }
}
