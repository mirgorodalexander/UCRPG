using System.Collections;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;

public class Item : MonoBehaviour
{
    [Title("Preferences")]
    public float Chance;
    public int Price;
    public _Type Type;
    public enum _Type { Weapon, Armor, Accessory, Potion, Ammo, Loot };
    void Start()
    {
        
    }

    void Update()
    {
        
    }
}
