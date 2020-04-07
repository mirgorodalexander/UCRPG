using System;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.Serialization;

[CreateAssetMenu(fileName = "Data", menuName = "UCRPG/Database/Weapon", order = 1)]
public class WeaponDatabase : ScriptableObject
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
        public string Description;

        [FormerlySerializedAs("Power")] [VerticalGroup("Preferences")]
        public int ATK, SPD;
        
    }
}
