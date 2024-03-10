using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
// SINCE WE WANT TO REFERENCE THIS DATA FOR EVERY SAVE FILE, THIS SCRIPT IS NOT A MONOBEHAVIOUR AND IS INSTEAD SERIALIZABLE
public class CharacterSaveData
{
    [Header("Scene Index")]
    public int sceneIndex = 1;

    [Header("Character Name")]
    public string characterName = "Character";

    [Header("Time Played")]
    public float secondsPlayed;

    [Header("World Coordinates")]
    public float xPosition;
    public float yPosition;
    public float zPosition;

    [Header("Resources")]
    public float currentHealth;
    public float currentMana;
    public float currentStamina;

    [Header("Stats")]
    public int constitution;
    public int intelligence;
    public int endurance;
    public int fortitude;

    [Header("Bosses")]
    public SerializableDictionary<int, bool>bossesAwakened; // THE INT IS THE BOSS I.D, THE BOOL IS THE AWAKENDED/DEFEATED STATUS
    public SerializableDictionary<int, bool>bossesDefeated;

    public CharacterSaveData()
    {
        bossesAwakened = new SerializableDictionary<int, bool>();
        bossesDefeated = new SerializableDictionary<int, bool>();
    }

}
