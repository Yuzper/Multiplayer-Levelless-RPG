using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TutorialManager : MonoBehaviour
{
    public static TutorialManager instance;

    [Header("Tutorials")]
    public GameObject movementTutorial;
    public GameObject lockOnTutorial;
    public GameObject fightingTutorial;
    public GameObject drawingTutorial;
    public GameObject spellTutorial;

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


    public void TurnTutorialOn(string tutorialObject)
    {
        TurnTutorialOff(movementTutorial);
        TurnTutorialOff(lockOnTutorial);
        TurnTutorialOff(fightingTutorial);
        TurnTutorialOff(drawingTutorial);
        TurnTutorialOff(spellTutorial);

        if (tutorialObject == "movement")
        {
            movementTutorial.SetActive(true);
        }
        else if (tutorialObject == "melee")
        {
            lockOnTutorial.SetActive(true);
        }
        else if (tutorialObject == "lockOn")
        {
            fightingTutorial.SetActive(true);
        }
        else if (tutorialObject == "drawing")
        {
            drawingTutorial.SetActive(true);
        }
        else if (tutorialObject == "spell")
        {
            spellTutorial.SetActive(true);
        }
    }


    public void TurnTutorialOff(GameObject tutorialObject)
    {
        tutorialObject.SetActive(false);
    }


}
