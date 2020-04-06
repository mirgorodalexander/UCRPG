using System;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.Serialization;

[CreateAssetMenu(fileName = "Item Database", menuName = "UCRPG/Database/Item", order = 1)]
public class ItemDatabase : ScriptableObject
{
    [TableList] public List<ItemSetupClass> Items;

    [Serializable]
    public class ItemSetupClass
    {
        [TableColumnWidth(50, Resizable = false)]
        public int ID;
        
        [TableColumnWidth(50, Resizable = false)]
        [PreviewField(50, ObjectFieldAlignment.Center)]
        public GameObject Prefab;
        
        [VerticalGroup("Settings")]
        public string Name;
        
        [VerticalGroup("Settings")]
        public float Chance;
        
        [VerticalGroup("Settings")]
        public _Type Type;
        public enum _Type { Weapon, Armor, Accessory, Potion, Ammo, Loot };

        [VerticalGroup("Preferences")]
        public int Amount, Price;

    }

    [Serializable]
    public class DroppedClass
    {
        [PreviewField(50, ObjectFieldAlignment.Left)]
        public GameObject Prefab;
        public float Chance;
    }
}