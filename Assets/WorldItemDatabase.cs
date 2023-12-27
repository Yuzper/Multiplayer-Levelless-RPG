using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class WorldItemDatabase : MonoBehaviour
{
    public static WorldItemDatabase instance;

    public WeaponItems unarmedWeapon;

    [Header("Weapons")]
    [SerializeField] List<WeaponItems> weapons = new List<WeaponItems>();
    
    [Header("Items")]
    // A LIST OF EVERY ITEM WE HAVE IN THE GAME
    private List<Item> items = new List<Item>();

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

        // ADD ALL OF OUR WEAPONS TO THE LIST OF ITEMS
        foreach (var weapon in weapons)
        {
            items.Add(weapon);
        }

        // ASSIGN ALL OF OUR ITEMS A UNIQUE ITEM ID
        for (int i = 0; i < items.Count; i++)
        {
            items[i].itemID = i;
        }
    }

    public WeaponItems GetWeaponByID(int ID)
    {
        return weapons.FirstOrDefault(weapon => weapon.itemID == ID);
    }


}
