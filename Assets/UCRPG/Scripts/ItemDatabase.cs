using System;
using System.Collections;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;

public class ItemDatabase : SerializedMonoBehaviour
{
    [Header("Item Setup")]
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
    
    [Title("Buttons")]
    [Button("Print All", ButtonSizes.Large), GUIColor(1, 1, 1)]
    private void DebugPrintAll()
    {
        foreach (var child in Items)
        {
            Debug.Log($"[DEBUG] - Name: {child.Name}");
        }
    }
    
    void Start()
    {
        
    }

    void Update()
    {
        
    }
}