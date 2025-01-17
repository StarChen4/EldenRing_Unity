using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class WorldItemDatabase : MonoBehaviour
{
    public static WorldItemDatabase instance;

    public WeaponItem unarmedWeapon;
    
    [Header("Weapons")]
    [SerializeField] List<WeaponItem> weapons = new List<WeaponItem>();
    
    // a list of every item we have in the game
    [Header("Items")]
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
        
        // add all weapons to the list of items
        foreach (var weapon in weapons)
        {
            items.Add(weapon);
        }
        
        // assign all the items an unique item ID
        for (int i = 0; i < items.Count; i++)
        {
            items[i].itemID = i;
        }
    }

    public WeaponItem GetWeaponByID(int ID)
    {
        return weapons.FirstOrDefault(weapon => weapon.itemID == ID);
    }
}
