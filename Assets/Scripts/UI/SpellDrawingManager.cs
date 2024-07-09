using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpellDrawingManager : MonoBehaviour
{
    public static SpellDrawingManager instance;

    [Header("Menus")]
    [SerializeField] public GameObject spellDrawMenu;
    public RuneDrawer runeDrawer;
    public TestingDrawing testingDrawing;

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
        runeDrawer.ClearDrawing();
        testingDrawing.UI_LineRenderer.ResetDrawing();
    }

    public void CloseSpellDrawingMenu()
    {
        spellDrawMenu.SetActive(false);
        PlayerUIManager.instance.player.characterSpellManager.inSpellMode = false;
    }
}
